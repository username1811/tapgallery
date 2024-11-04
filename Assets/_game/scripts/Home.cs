using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Home : UICanvas
{
    public bool isInited = false;


    public override void Open()
    {
        base.Open();
        Init();
        MinimapManager.Ins.OnOpenHome();
        Refresh();
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

    public void ButtonPlay()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadCurrentLevel();
        });
    }

}
