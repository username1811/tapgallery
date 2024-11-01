using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class ArrowTile : MonoBehaviour
{
    [Title("Move:")]
    public float moveDistance;
    public float moveDuration;
    public AnimationCurve moveCurve;
    public bool isMoving;
    public DirectionType directionType;
    [Title("Sorting Order:")]
    public int sortingOrder;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer edgeSpriteRenderer;
    [Title("Arrow:")]
    public Color color;
    public Arrow arrow;
    [Title("Cover:")]
    public Cover cover;
    [Title("Minimap:")]
    public MinimapSquare minimapSquare;
    public Vector2 dir => arrow.dir;
    public bool isRed => cover.redTween.IsPlaying();
    public PixelData pixelData;


    private void OnValidate()
    {
        RefreshDirection();
    }

    public void RefreshDirection()
    {
        arrow.directionType = this.directionType;
        arrow.OnInit();
    }

    public void OnInitt(PixelData pixelData)
    {
        this.pixelData = pixelData; 
        //pos
        this.transform.position = pixelData.coordinate;
        //direction
        SetDirection(pixelData.directionType);
        //sortingorder
        this.sortingOrder = pixelData.sortingOrder;
        spriteRenderer.sortingOrder = sortingOrder * 10;
        edgeSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 5;
        arrow.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 5;
        cover.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 6;
        //color
        this.color = pixelData.color;
        arrow.spriteRenderer.color = color;
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
        ArrowTile stuckArrowTile = GetArrowTileOnTheRoad(dir);
        if (!IsCanEat())
        {
            Vector2 oldPos = this.transform.position;
            Vector2 targetMove = (Vector2)stuckArrowTile.transform.position - dir.normalized;
            float moveDistance = (targetMove - (Vector2)this.transform.position).magnitude;
            float newMoveDuration = Mathf.Max(0.4f, moveDistance / 33f * moveDuration * 1.8f);
            this.transform.DOMove(targetMove, newMoveDuration).SetEase(Ease.InSine).OnComplete(() =>
            {
                this.transform.DOMove(oldPos, newMoveDuration).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    if (isMoving) ClickManager.isCanClick = true;
                    isMoving = false;
                });
            });
            cover.TurnRed(newMoveDuration*2f);
            CameraManager.Ins.Punch(dir);
            isMoving = moveDistance > 0.2f;
            ClickManager.isCanClick = !isMoving;
        }
        else
        {
            isMoving = true;
            this.transform.DOMove((Vector2)this.transform.position + moveDistance * dir, moveDuration).SetEase(moveCurve).OnComplete(() =>
            {
                isMoving = false;
                PoolManager.Ins.Despawn(PoolType.ArrowTile, this.gameObject);
            });
            Dot dot = PoolManager.Ins.Spawn<Dot>(PoolType.Dot);
            dot.transform.position = this.transform.position - Vector3.up * 0.186f;
            dot.OnInit(0, arrow.spriteRenderer.color, 1);
            dot.Scale();
            minimapSquare.ChangeColor();
            LevelManager.Ins.currentLevel.remainTiles.Remove(this);
            LevelManager.Ins.CheckWinLose();
        }
    }

    public ArrowTile GetArrowTileOnTheRoad(Vector2 dir)
    {
        LayerMask arrowTileMask = GameManager.Ins.arrowTileMask;
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, dir, 100f, arrowTileMask);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach(var hit in hits)
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                return hit.collider.GetComponent<ArrowTile>();
            }
        }
        return null;
    }

    public bool IsCanEat()
    {
        ArrowTile stuckArrowTile = GetArrowTileOnTheRoad(dir);
        return !(stuckArrowTile != null && !stuckArrowTile.isMoving);
    }

    public void SetDirection(DirectionType directionType)
    {
        this.directionType = directionType;
        arrow.directionType = directionType;
    }

    private void OnMouseUp()
    {
        if (!ClickManager.isCanClick) return;
        if (CameraManager.Ins.isDraggingOver1) return;
        if (CameraManager.Ins.isDoingAnim) return;
        if (LevelManager.Ins.isEndLevel) return;
        Move();
    }
}
