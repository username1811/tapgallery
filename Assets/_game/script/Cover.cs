using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    Tween redTween;
    public SpriteRenderer spriteRenderer;
    public Color originColor;

    private void Start()
    {
        originColor = spriteRenderer.color;
    }

    public void TurnRed()
    {
        redTween?.Kill();
        spriteRenderer.color = originColor;
        redTween = spriteRenderer.DOColor(Color.red, 1).SetEase(Ease.InSine).From().OnComplete(() =>
        {
            redTween=null;
        }) ;
    }
}
