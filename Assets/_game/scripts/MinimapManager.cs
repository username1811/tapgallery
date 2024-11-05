using DG.Tweening;
using Sirenix.OdinInspector;
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
    float minX,maxX, minY, maxY;    
    float objectWidth => maxX - minX;
    float objectHeight => maxY - minY;
    public LevelInfooo levelInfooo;
    public Wave wave;
    public bool isWin;


    public void OnOpenHome()
    {
        wave.gameObject.SetActive(false);
        if (!isWin)
        {
            CreateGreyLevel(LevelManager.Ins.GetLevelInfo(DataManager.Ins.playerData.currentLevelIndex), 0);
            InitLimitPosition();
            MoveToCenter(false);
            RefreshCamDistance(false, 5f);
        }
        else
        {
            CreateGreyLevel(LevelManager.Ins.GetLevelInfo(DataManager.Ins.playerData.currentLevelIndex-1), 0);
            InitLimitPosition();
            MoveToCenter(false);
            RefreshCamDistance(false, 5f);
            ShowColor();
        }
        isWin = false;
    }

    public void CreateGreyLevel(LevelInfooo levelInfo, int stageIndex)
    {
        this.levelInfooo = levelInfo;
        minimapSquares.Clear();
        PoolManager.Ins.GetPool(PoolType.MinimapSquare).ReturnAll();
        StageInfooo stageInfooo = levelInfo.stages[stageIndex];
        GameObject stageObj = new GameObject("stage_" + stageInfooo.name);
        foreach (var pixelData in stageInfooo.pixelDatas)
        {
            MinimapSquare minimapSquare = PoolManager.Ins.Spawn<MinimapSquare>(PoolType.MinimapSquare);
            minimapSquare.transform.SetParent(this.transform);
            minimapSquare.OnInit(pixelData);
            minimapSquares.Add(minimapSquare);
        }
    }
    public void InitLimitPosition()
    {
        minX = maxX = levelInfooo.stages[0].pixelDatas[0].coordinate.x;
        minY = maxY = levelInfooo.stages[0].pixelDatas[0].coordinate.y;

        foreach (var pixelData in levelInfooo.stages[0].pixelDatas)
        {
            Vector3 position = pixelData.coordinate;

            if (position.x < minX) minX = position.x;
            if (position.x > maxX) maxX = position.x;
            if (position.y < minY) minY = position.y;
            if (position.y > maxY) maxY = position.y;
        }
        /*float padding = 5f;
        minX -= padding;
        maxX += padding;
        minY -= padding;
        maxY += padding;*/
    }


    public void MoveToCenter(bool isAnim = false, Action OnComplete = null)
    {
        Vector3 targetMove = new Vector3((maxX + minX) / 2f, (maxY + minY) / 2f, -18f) + (Vector3)offsetFromOriginalLevel;
        targetMove.z = -18f;
        if (isAnim)
        {
            Ease ease = Ease.OutSine;
            cam.transform.DOMove(targetMove, 1f).SetEase(ease).OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
        }
        else
        {
            cam.transform.position = targetMove;
        }
        Debug.Log("minimap cam move to center, targetmove = " + targetMove.ToString());
    }

    public void RefreshCamDistance(bool isAnim, float offsetY, Action OnComplete = null)
    {
        // Tính toán tỉ lệ khung hình của màn hình
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetOrthoSize = Mathf.Max(objectWidth, objectHeight) * screenAspect + offsetY;

        if (isAnim)
        {
            float duration = isAnim ? 1f : 0f;
            Ease ease = Ease.OutSine;
            DOVirtual.Float(cam.orthographicSize, targetOrthoSize, duration, v =>
            {
                cam.orthographicSize = v;
            }).SetEase(ease).OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
        }
        else
        {
            cam.orthographicSize = targetOrthoSize;
            OnComplete?.Invoke();
        }
    }

    [Button]
    public void ShowColor()
    {
        wave.gameObject.SetActive(false);
        wave.transform.position = new Vector3(minX-1f, minY-1f, 0) + (Vector3)offsetFromOriginalLevel;
        wave.gameObject.SetActive(true);
        Vector3 targetMove = new Vector3(maxX+1f, maxY+1f, 0) + (Vector3)offsetFromOriginalLevel;
        wave.transform.DOMove(targetMove, 4f).SetEase(Ease.Linear).OnComplete(() =>
        {
            wave.gameObject.SetActive(false);
        });
    }

    [Button]
    public void ResetColorGrey()
    {
        foreach(var minimapSquare in minimapSquares)
        {
            minimapSquare.spriteRenderer.color = initColor;
        }
    }
}
