using UnityEngine;
using System.Collections.Generic;
using System;

public class AnimationTransitions : MonoBehaviour
{
    public static string IGNORED_ANIMATION_NAME = "";

    private static string _FacingDirectionToString(Character.FacingDirectionType facingDirection) => facingDirection == Character.FacingDirectionType.Left ? "L" : "R";

    private static int _AnimatorStateToHash(string stateName, Character.FacingDirectionType facingDirection) => Animator.StringToHash($"Base Layer.{stateName}_{_FacingDirectionToString(facingDirection)}");

    private static int _TransitionToHash(string source, string destination) => Animator.StringToHash($"{source}->{destination}");

    [Serializable]
    private struct TransitionOneShot
    {
        [SerializeField]
        private string _source;

        [SerializeField]
        private string _destination;

        [SerializeField]
        private string _oneShot;

        public int TransitionHash => _TransitionToHash(_source, _destination);

        public string Source => _source;
        public string Destination => _destination;
        public string OneShot => _oneShot;
    }

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private List<TransitionOneShot> _transitions = new List<TransitionOneShot>();

    private interface Phase
    {
        void Update();
        void OnPlay(string originalAnimation, string newAnimation);
        void OnFacingDirectionChanged(Character.FacingDirectionType newFacingDirection);
    }

    private enum PhaseType
    {
        Initial,
        Loop,
        OneShot
    }

    private string _desiredAnimationName = IGNORED_ANIMATION_NAME;
    private string _currentOneShotAnimation = IGNORED_ANIMATION_NAME;
    private Phase _currentPhase;
    private List<Phase> _phaseInstances = new List<Phase>();
    private Character.FacingDirectionType _currentFacingDirection = Character.FacingDirectionType.Right;
    private Dictionary<int, string> _transitionMap = new Dictionary<int, string>();

    private void Awake()
    {
        if (_animator == null)
        {
            Debug.LogWarning($"Animator is not assigned in {nameof(AnimationTransitions)} on {gameObject.name}. Attempting to find Animator component.");
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError($"Animator component not found on {gameObject.name}. Please assign an Animator in the inspector.");
                enabled = false;
                return;
            }
        }

        _phaseInstances.Add(new InitialPhase(this));
        _phaseInstances.Add(new LoopPhase(this));
        _phaseInstances.Add(new OneShotPhase(this));
        _currentPhase = _phaseInstances[(int)PhaseType.Initial];

        // Build the transition map for quick lookup
        foreach (var transition in _transitions)
        {
            int hash = transition.TransitionHash;
            if (!_transitionMap.ContainsKey(hash))
                _transitionMap.Add(hash, transition.OneShot);
            else
                Debug.LogWarning($"Duplicate transition from {transition.Source} to {transition.Destination} found in {nameof(AnimationTransitions)} on {gameObject.name}. Only the first one will be used.");
        }
    }

    public void Play(string animationName)
    {
        if (_desiredAnimationName == animationName)
            return; // Already in the desired loop, do nothing

        if (animationName == IGNORED_ANIMATION_NAME)
            return; // Ignore empty animation names

        var originalAnimation = _desiredAnimationName;
        _desiredAnimationName = animationName;
        _currentPhase.OnPlay(originalAnimation, animationName);
    }

    public void SetFacingDirection(Character.FacingDirectionType newFacingDirection)
    {
        if (_currentFacingDirection == newFacingDirection)
            return; // No change in facing direction
        _currentFacingDirection = newFacingDirection;
        _currentPhase.OnFacingDirectionChanged(newFacingDirection);
    }

    public void SetAnimatorUpdateMode(AnimatorUpdateMode updateMode)
    {
        if (_animator != null)
            _animator.updateMode = updateMode;
    }

    private void LateUpdate()
    {
        _currentPhase.Update();
    }

    private void _ChangePhase(PhaseType newPhase)
    {
        _currentPhase = _phaseInstances[(int)newPhase];
    }

    private void _PlayAnimationFromScratch(string animationName)
    {
        if (_animator == null)
            return;

        int hash = _AnimatorStateToHash(animationName, _currentFacingDirection);
        _animator.Play(hash, 0, 0.0f);
    }

    private void _PlayAnimationInherit(string animationName)
    {
        if (_animator == null)
            return;

        int hash = _AnimatorStateToHash(animationName, _currentFacingDirection);
        var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        var timeFraction = Mathf.Repeat(normalizedTime, 1.0f);
        _animator.Play(hash, 0, timeFraction);
    }

    // ==========
    // 初期状態
    // ==========
    private class InitialPhase : Phase
    {
        private readonly AnimationTransitions _context;
        public InitialPhase(AnimationTransitions context)
        {
            _context = context;
        }
        public void Update()
        {
            // No specific update logic for initial phase
        }
        public void OnPlay(string originalAnimation, string newAnimation)
        {
            _context._PlayAnimationFromScratch(newAnimation);
            _context._ChangePhase(PhaseType.Loop);
        }

        public void OnFacingDirectionChanged(Character.FacingDirectionType newFacingDirection)
        {
            // No specific logic for facing direction change in initial phase
        }
    }

    // ==========
    // ループ状態
    // ==========
    private class LoopPhase : Phase
    {
        private readonly AnimationTransitions _context;
        public LoopPhase(AnimationTransitions context)
        {
            _context = context;
        }
        public void Update()
        {
            // No specific update logic for loop phase
        }
        public void OnPlay(string originalAnimation, string newAnimation)
        {
            // Check if there's a one-shot transition from the current loop to the new loop
            int transitionHash = _TransitionToHash(originalAnimation, newAnimation);
            if (_context._transitionMap.TryGetValue(transitionHash, out string oneShotAnimation))
            {
                // Play the one-shot animation
                _context._PlayAnimationFromScratch(oneShotAnimation);
                _context._ChangePhase(PhaseType.OneShot);
                _context._currentOneShotAnimation = oneShotAnimation;
            }
            else
            {
                // No one-shot transition, directly switch to the new loop
                _context._PlayAnimationFromScratch(newAnimation);
            }
        }
        public void OnFacingDirectionChanged(Character.FacingDirectionType newFacingDirection)
        {
            // When facing direction changes, re-play the current animation to reflect the new direction
            _context._PlayAnimationInherit(_context._desiredAnimationName);
        }
    }

    // ==========
    // ワンショット状態
    // ==========
    private class OneShotPhase : Phase
    {
        private readonly AnimationTransitions _context;

        public OneShotPhase(AnimationTransitions context)
        {
            _context = context;
        }
        public void Update()
        {
            var stateInfo = _context._animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime < 1.0f)
                return; // OneShot animation is still playing

            // OneShot animation has finished, transition back to the desired loop
            _context._currentOneShotAnimation = IGNORED_ANIMATION_NAME;
            _context._PlayAnimationFromScratch(_context._desiredAnimationName);
            _context._ChangePhase(PhaseType.Loop);
        }
        public void OnPlay(string originalAnimation, string newAnimation)
        {
            // Check if there's a one-shot transition from the current loop to the new loop
            int transitionHash = _TransitionToHash(originalAnimation, newAnimation);
            if (_context._transitionMap.TryGetValue(transitionHash, out string oneShotAnimation))
            {
                // Play the one-shot animation
                _context._currentOneShotAnimation = oneShotAnimation;
                _context._PlayAnimationFromScratch(oneShotAnimation);
            }
            else
            {
                // No one-shot transition, directly switch to the new loop
                _context._PlayAnimationFromScratch(newAnimation);
                _context._ChangePhase(PhaseType.Loop);
            }
        }

        public void OnFacingDirectionChanged(Character.FacingDirectionType newFacingDirection)
        {
            // When facing direction changes, re-play the current one-shot animation to reflect the new direction
            _context._PlayAnimationInherit(_context._currentOneShotAnimation);
        }
    }
}
