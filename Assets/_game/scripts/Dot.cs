using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int sortingOrder;
    

    public void OnInit(Color color, int sortingOrder)
    {
        spriteRenderer.color = color;
        this.sortingOrder = sortingOrder;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    public void Scale()
    {
        this.transform.DOScale(0.5f, 0.15f).SetEase(Ease.Linear).SetLoops(2,LoopType.Yoyo);
    }
}
