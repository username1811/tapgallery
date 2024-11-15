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

    public Transform Content;


    public override void Open()
    {
        base.Open();
        Init();
        Refresh();

        getListGallery();
    }

    public void Init()
    {
        if (isInited) return;
        isInited = true;
    }

    [Button]
    public void Refresh()
    {
  
    }

    public void getListGallery()
    {
        List<string> listGallery = DataManager.Ins.playerData.passedLevelNames;

        listLevelInfo.Clear();

        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

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
                LevelPassItem newItem = Instantiate(itemLevel, Content);

                newItem.InitImage(listLevelInfo[i]);
            }
        }
    }


    public void BackBtn()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
        Close(0);
    }

}
