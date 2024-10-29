using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTile : MonoBehaviour
{
    [Title("Move:")]
    public float moveDistance;
    public float moveDuration;
    public AnimationCurve moveCurve;
    public bool isMoving;
    [Title("Sorting Order:")]
    public int sortingOrder;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer edgeSpriteRenderer;
    [Title("Arrow:")]
    public Arrow arrow;
    [Title("Cover:")]
    public Cover cover;


    public void OnInitt(Vector3 pos, DirectionType directionType, int sortingOrder)
    {
        //pos
        this.transform.position = pos;
        //direction
        SetDirection(directionType);
        //sortingorder
        this.sortingOrder = sortingOrder;
        spriteRenderer.sortingOrder = sortingOrder * 10;
        edgeSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 5;
        arrow.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 5;
        cover.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 6;
        //color
        arrow.spriteRenderer.color = ColorManager.Ins.GetRandomColor();
        //arrow
        arrow.OnInit();
    }

    [Button]
    public void ResetPosition()
    {
        this.transform.position = Vector3.zero;
    }

    [Button]
    public void Move()
    {
        isMoving = true;
        ArrowTile stuckArrowTile = GetArrowTileOnTheRoad(arrow.dir);
        if (stuckArrowTile == null)
        {
            this.transform.DOMove((Vector2)this.transform.position + moveDistance * arrow.dir, moveDuration).SetEase(moveCurve).OnComplete(() =>
            {
                isMoving = false;
                PoolManager.Ins.Despawn(PoolType.ArrowTile, this.gameObject);
            });
        }
        else
        {
            Vector2 oldPos = this.transform.position;
            cover.TurnRed();
            this.transform.DOMove(stuckArrowTile.transform.position, moveDuration).SetEase(Ease.InSine).OnComplete(() =>
            {
                this.transform.DOMove(oldPos, moveDuration).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    isMoving = false;
                });
            });
        }
    }

    public ArrowTile GetArrowTileOnTheRoad(Vector2 dir)
    {
        LayerMask arrowTileMask = GameManager.Ins.arrowTileMask;
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, dir, 100f, arrowTileMask);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        Debug.Log(arrowTileMask.ToString());
        Debug.Log(dir);
        Debug.DrawRay(this.transform.position, dir * 100, Color.red, 2f);
        foreach(var hit in hits)
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                return hit.collider.GetComponent<ArrowTile>();
            }
        }
        return null;
    }

    public void SetDirection(DirectionType directionType)
    {
        arrow.directionType = directionType;
    }

    private void OnMouseDown()
    {
        Move();
    }
}
