using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCover : MonoBehaviour
{
    public Tween greenTween;
    public SpriteRenderer spriteRenderer;
    public Color targetColor;
    public float duration;

    public void OnInitt()
    {
        spriteRenderer.color = new Color(0.22f, 1f, 0f, 0f);
        greenTween?.Kill();
    }

    [Button]
    public void OnHint()
    {
        greenTween?.Kill();
        greenTween = spriteRenderer.DOColor(targetColor, duration).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
        {
            greenTween = null;
        });
    }

    [Button]
    public void OnMove()
    {
        greenTween?.Kill();
        greenTween = spriteRenderer.DOColor(targetColor, duration).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            greenTween = null;
        });
    }
}
