using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSquare : MonoBehaviour
{
    public ArrowTile arrowTile;
    public SpriteRenderer spriteRenderer;

    public void OnInit(ArrowTile arrowTile)
    {
        this.arrowTile = arrowTile;
        this.spriteRenderer.color = MinimapManager.Ins.initColor;
        this.transform.position = arrowTile.transform.position + (Vector3)MinimapManager.Ins.offsetFromOriginalLevel;
    }

    public void ChangeColor()
    {
        this.spriteRenderer.color = arrowTile.arrow.spriteRenderer.color;
    }
}
