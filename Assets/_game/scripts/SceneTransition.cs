using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [Header("top bot : ")]
    public Image top;
    public Image bot;
    public float halfDuration;

    [Header("logo:")]
    public Image logo;
    public float angle;
    public Tween rotateLogoTween;
    public Tween fadeLogoTween;
    public CanvasGroup logoCanvasGroup;
    public float screenHeight => UIManager.Ins.screenHeight;


    public void FadeOut(Action OnComplete)
    {
        //topbot
        top.rectTransform.sizeDelta = new Vector2(0, 0);
        bot.rectTransform.sizeDelta = new Vector2(0, 0);
        DOVirtual.Float(0, screenHeight / 2, halfDuration, v =>
        {
            top.rectTransform.sizeDelta = new Vector2(0, v);
            bot.rectTransform.sizeDelta = new Vector2(0, v);
        }).SetEase(Ease.OutSine).OnComplete(() =>
        {
            fadeLogoTween?.Kill();
            rotateLogoTween?.Kill();
            SetLogoAlpha(1);
            SetLogoRotation(0);
            OnComplete?.Invoke();
        });
        //logo
        logo.gameObject.SetActive(true);
        SetLogoAlpha(0);
        fadeLogoTween = logoCanvasGroup.DOFade(1, halfDuration * 0.5f).SetEase(Ease.OutSine).SetDelay(halfDuration * 0.5f);
        SetLogoRotation(angle);
        rotateLogoTween = logo.transform.DORotate(Vector3.zero, halfDuration * 0.5f).SetEase(Ease.OutSine).SetDelay(halfDuration * 0.5f);
    }

    public void FadeIn(Action OnComplete)
    {
        //topbot
        top.rectTransform.sizeDelta = new Vector2(0, screenHeight / 2);
        bot.rectTransform.sizeDelta = new Vector2(0, screenHeight / 2);
        DOVirtual.Float(screenHeight / 2, 0, halfDuration * 0.7f, v =>
        {
            top.rectTransform.sizeDelta = new Vector2(0, v);
            bot.rectTransform.sizeDelta = new Vector2(0, v);
        }).SetEase(Ease.InSine).OnComplete(() =>
        {
            fadeLogoTween?.Kill();
            rotateLogoTween?.Kill();
            SetLogoAlpha(0);
            SetLogoRotation(angle);
            logo.gameObject.SetActive(false);
            OnComplete?.Invoke();
        });
        //logo
        SetLogoAlpha(1);
        fadeLogoTween = logoCanvasGroup.DOFade(0, halfDuration * 0.8f * 0.5f).SetEase(Ease.InSine);
        SetLogoRotation(0);
        rotateLogoTween = logo.transform.DORotate(new Vector3(0, 0, angle), halfDuration * 0.8f * 0.5f).SetEase(Ease.InSine);
    }

    public void SetLogoAlpha(float a)
    {
        //logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, a);
        logoCanvasGroup.alpha = a;
    }

    public void SetLogoRotation(float angleZ)
    {
        logo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleZ));
    }

}
