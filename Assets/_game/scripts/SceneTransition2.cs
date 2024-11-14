using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition2 : MonoBehaviour
{
    [Header("block UI : ")]
    public GameObject blockUI;

    public CanvasGroup canvasGroup;
    public float halfDuration;

    public void FadeOut(Action OnComplete)
    {
        canvasGroup.alpha = 0f;
        DOVirtual.Float(0, 1f, halfDuration, v =>
        {
            canvasGroup.alpha = v;
        }).SetEase(Ease.OutSine).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public void FadeIn(Action OnComplete)
    {
        canvasGroup.alpha = 1f;
        DOVirtual.Float(1f, 0f, halfDuration * 0.5f, v =>
        {
            canvasGroup.alpha = v;
        }).SetEase(Ease.InSine).SetDelay(halfDuration*0.1f).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }
}
