using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gallery : UICanvas
{
    public bool isInited;
    public LevelWrapperrr levelWrapperrr;

    private List<LevelInfooo> listLevelInfo = new List<LevelInfooo>();

    public LevelPassItem itemLevel;

    public RectTransform contentRectTF;


    public override void Open()
    {
        base.Open();
        Init();
        getListGallery();
        Refresh();
        //AnimOpen();
        contentRectTF.anchoredPosition = new Vector2(0, -50f);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        //AnimClose();
    }

    public void AnimOpen()
    {
        this.rectTransform.anchoredPosition = new Vector2(-UIManager.Ins.screenWidth, 0);
        this.rectTransform.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutSine);
        UIManager.Ins.GetUI<Home>().rectTransform.anchoredPosition = Vector2.zero;
        UIManager.Ins.GetUI<Home>().rectTransform.DOAnchorPos(new Vector2(UIManager.Ins.screenWidth, 0), 0.5f).SetEase(Ease.OutSine);
    }

    public void AnimClose()
    {
        this.rectTransform.DOAnchorPos(new Vector2(-UIManager.Ins.screenWidth, 0), 0.4f).SetEase(Ease.OutSine);
        UIManager.Ins.GetUI<Home>().rectTransform.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutSine);
    }

    public void Init()
    {
        if (isInited) return;
        isInited = true;
    }

    [Button]
    public void Refresh()
    {
        RefreshContentHeight();
    }

    public void getListGallery()
    {
        List<string> listGallery = DataManager.Ins.playerData.passedLevelNames;

        listLevelInfo.Clear();

        PoolManager.Ins.GetPool(PoolType.LevelPassItem).ReturnAll();

        if (listGallery.Count > 0)
        {
            foreach (string levelName in listGallery)
            {
                LevelInfooo levelInfo = levelWrapperrr.levels.Find(l => l.name == levelName);

                if (levelInfo != null)
                {
                    listLevelInfo.Add(levelInfo);
                }
            }

            for (int i = 0; i < listLevelInfo.Count; i++)
            {
                LevelPassItem newItem = PoolManager.Ins.Spawn<LevelPassItem>(PoolType.LevelPassItem);
                newItem.transform.SetParent(contentRectTF.transform);
                newItem.transform.localScale = Vector3.one;
                newItem.InitImage(listLevelInfo[i]);
            }
        }
    }

    public void RefreshContentHeight()
    {
        contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, (Mathf.Ceil(listLevelInfo.Count / 3)+1) * 340f + 60 * (Mathf.Ceil(listLevelInfo.Count / 3)) + 100f);
    }


    public void BackBtn()
    {
        UIManager.Ins.CloseUI<Gallery>();
    }

}
