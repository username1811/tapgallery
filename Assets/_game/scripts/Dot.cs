using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int id;
    public SpriteRenderer spriteRenderer;
    public int sortingOrder;
    

    public void OnInit(int id, Color color, int sortingOrder)
    {
        this.id = id;
        spriteRenderer.color = color;
        this.sortingOrder = sortingOrder;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    public void Scale()
    {
        this.transform.DOScale(0.5f, 0.15f).SetEase(Ease.Linear).SetLoops(2,LoopType.Yoyo);
    }
}
