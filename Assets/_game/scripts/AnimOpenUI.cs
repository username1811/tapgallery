using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOpenUI : MonoBehaviour
{
    public Tween scaleTween;

    private void OnEnable()
    {
        scaleTween?.Kill();
        this.transform.localScale = 0.2f * Vector3.one;
        scaleTween = this.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }
}
