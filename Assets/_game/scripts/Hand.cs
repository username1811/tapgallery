using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Tween rotateTween;
    public Tween scaleTween;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Anim()
    {
        rotateTween?.Kill();
        scaleTween?.Kill();
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 15));
        this.transform.localScale = Vector3.one;
        rotateTween = this.transform.DOLocalRotate(new Vector3(0, 0, 20f), 0.8f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
        scaleTween = this.transform.DOScale(1.3f, 0.8f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
