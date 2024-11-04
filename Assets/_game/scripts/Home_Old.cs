using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Home_Old : UICanvas
{
    public List<LevelUI> levelUIs = new List<LevelUI>();
    public GridLayoutGroup grid;
    public RectTransform scrollView;
    public RectTransform content;
    public bool isInited = false;



    public override void Open()
    {
        base.Open();
        Init();
        Refresh();
    }

    public void Init()
    {
        if (isInited) return;
        InitLevelUIs();
        RefreshGridCellSize();
        InitScrollViewHeight();
        InitContentHeight();
        isInited = true;
    }

    private void InitScrollViewHeight()
    {
        scrollView.sizeDelta = new Vector2(scrollView.sizeDelta.x, UIManager.Ins.screenHeight-480f);
    }

    private void InitContentHeight()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Ceil((float)levelUIs.Count/3f) * (grid.cellSize.y + grid.spacing.y) + 100f);
    }

    public void InitLevelUIs()
    {
        foreach(var levelInfo in LevelManager.Ins.levelWrapperrr.levels)
        {
            LevelUI levelUI = PoolManager.Ins.Spawn<LevelUI>(PoolType.LevelUI);
            levelUI.OnInitt(levelInfo);
            levelUIs.Add(levelUI);
        }
    }

    public void RefreshGridCellSize()
    {
        float padding = grid.padding.top;
        float spacing = grid.spacing.x;
        float cellSize = (UIManager.Ins.screenWidth - padding * 2 - spacing * 2)/3;
        grid.cellSize = new Vector2(cellSize, cellSize);
    }

    [Button]
    public void Refresh()
    {
        foreach(LevelUI levelUI in levelUIs)
        {
            levelUI.gameObject.SetActive(true);
            levelUI.transform.SetParent(grid.transform);
            levelUI.transform.localScale = Vector3.one;
            DOVirtual.DelayedCall(Time.deltaTime, () =>
            {
                levelUI.Refresh();
            });
        }
    }

}
