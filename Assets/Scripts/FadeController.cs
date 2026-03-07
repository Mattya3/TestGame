using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private Image _fadeImage;

    [SerializeField, Min(0.0f)]
    private float _fadeTime;

    [SerializeField, Min(0.0f)]
    private float _waitAfterFade;

    private void Awake()
    {
        _fadeImage.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// フェードアウト処理を実行します。
    /// </summary>
    public IEnumerator FadeOut()
    {
        _ActivateCanvas();

        float timer = 0f;
        while (timer < _fadeTime)
        {
            timer += Time.deltaTime;
            _SetAlpha(timer / _fadeTime);
            yield return null;
        }
        _SetAlpha(1f);
        yield return new WaitForSeconds(_waitAfterFade);
    }

    private void _SetAlpha(float alpha)
    {
        Color c = _fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        _fadeImage.color = c;
    }

    private void _ActivateCanvas()
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>(true);
        parentCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}
