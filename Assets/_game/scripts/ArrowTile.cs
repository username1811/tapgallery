using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTile : MonoBehaviour
{
    [Title("Move:")]
    public float moveDistance;
    public float moveDuration;
    public AnimationCurve moveCurve;
    public bool isMoving;
    public DirectionType directionType;
    public Sequence stuckSequence;
    [Title("Sorting Order:")]
    public int sortingOrder;
    public SpriteRenderer spriteRenderer;
    [Title("Arrow:")]
    public Color color;
    public Arrow arrow;
    [Title("Cover:")]
    public RedCover redCover;
    public GreenCover greenCover;
    public Vector2 dir => arrow.dir;
    public bool isRed => redCover.redTween.IsPlaying();
    public bool isGreen => greenCover.greenTween.IsPlaying();
    [Title("pixel:")]
    public PixelData pixelData;
    [Title("trail:")]
    public TrailRenderer trailRenderer;

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
        isMoving = false;
        this.transform.localScale = Vector3.one;
        //pos
        this.transform.position = pixelData.coordinate;
        //direction
        SetDirection(pixelData.directionType);
        //sortingorder
        this.sortingOrder = pixelData.sortingOrder;
        spriteRenderer.sortingOrder = sortingOrder * 10;
        trailRenderer.sortingOrder = spriteRenderer.sortingOrder-1;
        greenCover.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 3;
        arrow.shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 4;
        arrow.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 5;
        redCover.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 6;
        //color
        this.color = pixelData.color;
        arrow.spriteRenderer.color = color;
        //arrow
        arrow.OnInit();
        //cover
        redCover.OnInitt();
        greenCover.OnInitt();
    }

    public void IncreaseSortingOrder(int x)
    {
        this.sortingOrder += x;
        spriteRenderer.sortingOrder += x;
        trailRenderer.sortingOrder += x;
        greenCover.spriteRenderer.sortingOrder += x;
        arrow.shadowSpriteRenderer.sortingOrder += x;
        arrow.spriteRenderer.sortingOrder += x;
        redCover.spriteRenderer.sortingOrder += x;
    }

    [Button]
    public void ResetPosition()
    {
        this.transform.position = Vector3.zero;
    }

    public void OnSuccessRemove()
    {
        Dot dot = PoolManager.Ins.Spawn<Dot>(PoolType.Dot);
        dot.transform.position = this.transform.position - Vector3.up * 0.16f;
        dot.OnInit(arrow.spriteRenderer.color, 1);
        dot.Scale();
        LevelManager.Ins.currentLevel.remainTilesCount -= 1;
        LevelManager.Ins.CheckWinLose();
        if (Tutorial.Ins.IsCurrentTile(this)) Tutorial.Ins.Next();
    }

    public void OnFinishStuck()
    {
        if (isMoving) ClickManager.isCanClick = true;
        isMoving = false;
    }

    public void MoveStuck(ArrowTile stuckArrowTile)
    {
        Vector2 oldPos = this.transform.position;
        Vector2 targetMove = (Vector2)stuckArrowTile.transform.position - dir.normalized;
        float moveDistance = (targetMove - (Vector2)this.transform.position).magnitude;
        float newMoveDuration = Mathf.Max(0.4f, moveDistance / 33f * moveDuration * 1.8f);

        stuckSequence = DOTween.Sequence()
            .Append(this.transform.DOMove(targetMove, newMoveDuration).SetEase(Ease.InSine))
            .Append(this.transform.DOMove(oldPos, newMoveDuration).SetEase(Ease.OutSine))
            .OnComplete(() =>
            {
                OnFinishStuck();
                stuckSequence = null;
            });

        redCover.TurnRed(newMoveDuration * 2f);
        CameraManager.Ins.Punch(dir);
        isMoving = moveDistance > 0.2f;
        ClickManager.isCanClick = !isMoving;
        if (isMoving)
        {
            Debugger.DrawCircle(stuckArrowTile.transform.position, 0.3f, Color.red, 3f);
        }
        LevelManager.Ins.currentLevel.SubtractHeart(1);
    }

    public void MoveThrough()
    {
        isMoving = true;
        this.transform.DOMove((Vector2)this.transform.position + moveDistance * dir, moveDuration).SetEase(moveCurve).OnComplete(() =>
        {
            PoolManager.Ins.Despawn(PoolType.ArrowTile, this.gameObject);
            isMoving = false;
        });
        OnSuccessRemove();
        greenCover.OnMove();
    }

    [Button]
    public void Move()
    {
        ArrowTile stuckArrowTile = GetArrowTileOnTheRoad(dir);
        if (stuckSequence != null)
        {
            stuckSequence?.Kill();
            OnFinishStuck();
        }
        if (!IsCanEat())
        {
            MoveStuck(stuckArrowTile);
        }
        else
        {
            MoveThrough();
        }
    }

    public ArrowTile GetArrowTileOnTheRoad(Vector2 dir)
    {
        LayerMask arrowTileMask = GameManager.Ins.arrowTileMask;
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, dir, 100f, arrowTileMask);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != this.gameObject)
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
        if (CameraManager.Ins.isDragWhenIsMoving) return;
        if (LevelManager.Ins.isEndLevel) return;
        if (UIHover.isHoverUI) return;
        if (BoosterManager.Ins.isUsingBooster) return;
        if (LevelManager.Ins.currentLevelInfooo.isTut && !Tutorial.Ins.IsCurrentTile(this) && !Tutorial.Ins.isFinish) return;
        Move();
    }

    public static ArrowTile GetEatableTile()
    {
        return LevelManager.Ins.currentLevel.tiles.FirstOrDefault(x => x.gameObject.activeInHierarchy && !x.isMoving && x.IsCanEat());
    }

    public void OnExplode(bool isShowVFX)
    {
        this.transform.DOScale(0, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            PoolManager.Ins.Despawn(PoolType.ArrowTile, this.gameObject);
        });
        OnSuccessRemove();
    }

    [Button]
    public void OnMoveByMagnet()
    {
        IncreaseSortingOrder(1);
        MoveThrough();
    }

    public void OnCrash()
    {
        OnExplode(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (!LevelManager.isLoadedLevel) return;
        if (collision.gameObject.CompareTag(Constant.TAG_ARROW_TILE))
        {
            ArrowTile crashArrowTile = collision.gameObject.GetComponent<ArrowTile>();
            if(!crashArrowTile.isMoving)
            {
                crashArrowTile.OnCrash();
            }
        }
    }
}
