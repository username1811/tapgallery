using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Burst.CompilerServices;

public class Bomb : MonoBehaviour
{
    public List<ArrowTile> explodeArrowTiles = new();
    public int radius;




    public void OnInitt(ArrowTile arrowTile, Action OnComplete = null)
    {
        this.transform.localScale = Vector3.one;
        this.transform.position = arrowTile.transform.position;
        arrowTile.OnExplode(false);
        InitExplodeArrowTiles(arrowTile);
        AnimExplode(() =>
        {
            Explode();
            OnComplete?.Invoke();
        });
    }

    public void Explode()
    {
        foreach (var tile in explodeArrowTiles)
        {
            tile.OnExplode(true);
        }
        Transform bombVFX = PoolManager.Ins.Spawn<Transform>(PoolType.BombVFX);
        bombVFX.transform.position = this.transform.position;
        DOVirtual.DelayedCall(3f, () =>
        {
            PoolManager.Ins.Despawn(PoolType.BombVFX, bombVFX.gameObject);
        });
        PoolManager.Ins.Despawn(PoolType.Bomb, this.gameObject);
    }

    public void AnimExplode(Action OnComplete)
    {
        this.transform.DOScale(1.2f, 0.4f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public void InitExplodeArrowTiles(ArrowTile arrowTile)
    {
        explodeArrowTiles.Clear();
        Vector2 boxCenter = arrowTile.transform.position; // Tâm của hộp
        Vector2 boxSize = Vector2.one * GetBoxSize(); // Kích thước của hộp (rộng, cao)
        float angle = 45f; // Góc xoay của hộp
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle);
        foreach (Collider2D col in colliders)
        {
            ArrowTile hitArrowTile = col.GetComponent<ArrowTile>();
            if (!explodeArrowTiles.Contains(hitArrowTile) && hitArrowTile != arrowTile)
            {
                explodeArrowTiles.Add(hitArrowTile);
            }
        }
    }

    public float GetBoxSize()
    {
        return Mathf.Sqrt(2) * (radius - 0.5f + 0.05f);
    }
}
