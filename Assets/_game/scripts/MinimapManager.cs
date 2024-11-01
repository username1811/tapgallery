using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager>
{
    public Vector2 offsetFromOriginalLevel;
    public Color initColor;
    public List<MinimapSquare> minimapSquares = new List<MinimapSquare>();
    public Camera cam;
    public Vector2 originalMinimapPos = Vector2.zero;
    public RectTransform minimap => UIManager.Ins.GetUI<GamePlay>().minimapRectTF;

    public void OnLoadLevel()
    {
        minimapSquares.Clear();
        foreach (var tile in LevelManager.Ins.currentLevel.tiles)
        {
            MinimapSquare minimapSquare = PoolManager.Ins.Spawn<MinimapSquare>(PoolType.MinimapSquare);
            minimapSquare.OnInit(tile);
            minimapSquare.transform.SetParent(this.transform);
            minimapSquares.Add(minimapSquare);
            tile.minimapSquare = minimapSquare;
        }
        cam.transform.position = CameraManager.Ins.cam.transform.position + (Vector3)offsetFromOriginalLevel;
        ResetMinimapTransform();
        RefreshCamDistance();
    }

    public void OnWin(Action OnComplete)
    {
        Vector2 targetMove = new Vector2(-UIManager.Ins.screenWidth / 2, -UIManager.Ins.screenHeight / 2);
        minimap.DOAnchorPos(targetMove, 1.4f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
        minimap.DOScale(3f, 1.4f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });
    }

    public void ResetMinimapTransform()
    {
        if (originalMinimapPos != Vector2.zero) minimap.position = originalMinimapPos;
        minimap.transform.localScale = Vector3.one;
    }

    public void RefreshCamDistance()
    {
        float objectWidth = CameraManager.Ins.maxX- CameraManager.Ins.minX;
        float objectHeight = CameraManager.Ins.maxY- CameraManager.Ins.minY;

        // Tính toán tỉ lệ khung hình của màn hình
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Đặt orthographicSize của camera dựa trên chiều lớn hơn giữa chiều rộng và chiều cao
        cam.orthographicSize = Mathf.Max(objectWidth, objectHeight) * screenAspect;
    }
}
