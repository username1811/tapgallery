using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSquare : MonoBehaviour
{
    public PixelData pixelData;
    public SpriteRenderer spriteRenderer;

    public void OnInit(PixelData pixelData)
    {
        this.pixelData = pixelData;
        this.spriteRenderer.color = MinimapManager.Ins.initColor;
        this.transform.position = (Vector3)pixelData.coordinate + (Vector3)MinimapManager.Ins.offsetFromOriginalLevel;
    }

    public void OnWave()
    {
        ChangeColor();
        Scale();
    }

    public void ChangeColor()
    {
        this.spriteRenderer.color = pixelData.color;
    }

    public void Scale()
    {
        this.transform.localScale = Vector3.one * 0.2f;
        this.transform.DOScale(1f, 0.25f).SetEase(Ease.OutSine);
    }
}
