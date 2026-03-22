using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FailureMusicPlayer : MonoBehaviour
{
    [SerializeField]
    private float _delayBeforePlaying = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnPlayerDied += _OnFailure;
    }

    private void _OnFailure()
    {
        StartCoroutine(_CoPlay());
    }

    private IEnumerator _CoPlay()
    {
        yield return new WaitForSeconds(_delayBeforePlaying);
        _audioSource.Play();
    }
}
