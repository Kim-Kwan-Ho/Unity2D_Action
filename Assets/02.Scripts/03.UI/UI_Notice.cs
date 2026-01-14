using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Notice : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _noticeText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private float _blinkSpeed = 0.5f;

    private Coroutine _currentNoticeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        HideNotice();
    }

    public void SetNotice(ENoticeType noticeType, string message, float displayTime)
    {
        if (_currentNoticeCoroutine != null)
        {
            StopCoroutine(_currentNoticeCoroutine);
        }

        _noticeText.text = message;
        _currentNoticeCoroutine = StartCoroutine(CoShowNotice(noticeType, displayTime));
    }
    public void HideNotice()
    {
        _canvasGroup.alpha = 0f;
    }
    private IEnumerator CoShowNotice(ENoticeType noticeType, float displayTime)
    {
        yield return StartCoroutine(CoFadeIn());

        switch (noticeType)
        {
            case ENoticeType.Normal:
                break;
            case ENoticeType.Disappear:
                yield return new WaitForSeconds(displayTime);
                yield return StartCoroutine(CoFadeOut());
                break;
        }

        _currentNoticeCoroutine = null;
    }

    private IEnumerator CoFadeIn()
    {
        _canvasGroup.alpha = 0f;
        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime * _fadeSpeed;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
    }

    private IEnumerator CoFadeOut()
    {
        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
            yield return null;
        }
        _canvasGroup.alpha = 0f;
    }

    private IEnumerator CoBlink()
    {
        float targetAlpha = 0.3f;
        while (_canvasGroup.alpha > targetAlpha)
        {
            _canvasGroup.alpha -= Time.deltaTime / _blinkSpeed;
            yield return null;
        }
        _canvasGroup.alpha = targetAlpha;

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime / _blinkSpeed;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
    }

}
