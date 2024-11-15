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


    public void OnInitt(LevelInfooo levelInfooo)
    {
        this.levelInfooo = levelInfooo;
        Refresh();
    }

    public void OnClick()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadLevel(levelInfooo);    
        });
    }

    public void RefreshImg(bool isPassed)
    {
        pixelImg.sprite = isPassed? SpriteUtility.GetSpriteFromTexture2D(levelInfooo.texture2d) : SpriteUtility.GetSpriteFromSolidColor(new Color(0.7f , 0.7f, 0.7f, 1), levelInfooo.texture2d);
        pixelImg.rectTransform.sizeDelta = img.rectTransform.sizeDelta/2;
        if (levelInfooo.texture2d.width > levelInfooo.texture2d.height)
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
        bool isPassed = true;
        ThemeInfo themeInfo = ThemeManager.Ins.currentThemeInfo;
        ThemeLevelsData themeLevelsData = DataManager.Ins.playerData.themeLevelsDatas.FirstOrDefault(x => x.themeType == themeInfo.themeType);
        if (themeLevelsData == null) {
            isPassed = false;
        }
        else
        {
            isPassed = themeLevelsData.passedIndexs.Contains(themeInfo.levelInfooos.IndexOf(this.levelInfooo));
        }

        RefreshImg(isPassed);
    }
}