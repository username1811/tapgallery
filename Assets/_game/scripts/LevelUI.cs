using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Image img;
    public Image pixelImg;
    public LevelInfooo levelInfooo;
    public StageInfooo stageInfooo => levelInfooo.stages[0];


    public void OnInitt(LevelInfooo levelInfooo)
    {
        this.levelInfooo = levelInfooo;
    }

    public void OnClick()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadLevel(LevelManager.Ins.levelWrapperrr.levels.IndexOf(levelInfooo));
        });
    }

    public void RefreshImg(bool isPassed)
    {
        pixelImg.sprite = isPassed? SpriteUtility.GetSpriteFromTexture2D(stageInfooo.texture2d) : SpriteUtility.GetSpriteFromSolidColor(new Color(0.7f , 0.7f, 0.7f, 1), stageInfooo.texture2d);
        pixelImg.rectTransform.sizeDelta = img.rectTransform.sizeDelta/2;
        if (stageInfooo.texture2d.width > stageInfooo.texture2d.height)
        {
            pixelImg.ResizeImgKeepWidth();
        }
        else
        {
            pixelImg.ResizeImgKeepHeight();
        }
    }

    public void Refresh()
    {
        RefreshImg(DataManager.Ins.playerData.passedLevelNames.Contains(this.levelInfooo.name));
    }
}