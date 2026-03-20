using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField, Min(0.0f)] private float _fadeOutTimeOnPlayerDied = 0.1f;
    [SerializeField, Min(0.0f)] private float _fadeOutTimeOnAllPlayersGoal = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnPlayerDied += _OnPlayerDied;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _OnPlayerDied()
    {
        StartCoroutine(_CoOnPlayerDied());
    }

    private IEnumerator _CoOnPlayerDied()
    {
        yield return new WaitForSeconds(_fadeOutTimeOnPlayerDied);
        _audioSource.Stop();
    }
}
