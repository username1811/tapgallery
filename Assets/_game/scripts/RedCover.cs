using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCover : MonoBehaviour
{
    public Tween redTween;
    public SpriteRenderer spriteRenderer;
    public Color originColor;

    private void Awake()
    {
        originColor = spriteRenderer.color;
    }

    public void OnInitt()
    {
        spriteRenderer.color = originColor;
        redTween?.Kill();
    }

    public void TurnRed(float duration)
    {
        redTween?.Kill();
        spriteRenderer.color = originColor;
        redTween = spriteRenderer.DOColor(new Color(1f,0.4f,0.25f,0.8f), duration).SetEase(Ease.InSine).From().OnComplete(() =>
        {
            redTween=null;
        }) ;
    }
}
