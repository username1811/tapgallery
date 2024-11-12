using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : UICanvas
{
    public bool isInited = false;
    public RectTransform minimapRectTF;
    public Image nextPictureOutImg;
    public Image nextPictureInImg;
    public bool isWin;
    public RectTransform starRectTF;
    [Title("top:")]
    public RectTransform topRectTF;
    [Title("bot:")]
    public RectTransform buttonGalerryRectTF;
    public RectTransform buttonStarRectTF;
    public RectTransform buttonPlayRectTF;
    [Title("left:")]
    [Title("right:")]

    public static bool isWaitAnim = true;


    public override void Open()
    {
        base.Open();
        Init();
        Refresh();
        CameraManager.Ins.cam.transform.position = Vector3.zero;
        StarAnimManager.Ins.OnOpenHome();
        Anim();
        isWin = false;
        isWaitAnim = true;
    }

    public void Init()
    {
        if (isInited) return;
        isInited = true;
    }

    [Button]
    public void Refresh()
    {
        RefreshPicturePositions();
        RefreshNextPictureSprite();
        MinimapManager.Ins.OnOpenHome(isWin, MovePictures);
    }

    public void RefreshPicturePositions()
    {
        minimapRectTF.anchoredPosition = new Vector2(0, minimapRectTF.anchoredPosition.y);
        nextPictureOutImg.rectTransform.anchoredPosition = new Vector2(UIManager.Ins.screenWidth, nextPictureOutImg.rectTransform.anchoredPosition.y);
    }

    public void RefreshNextPictureSprite()
    {
        Debug.Log("curelevindexz " + DataManager.Ins.playerData.currentLevelIndex.ToString());
        Texture2D texture2d = LevelManager.Ins.GetLevelInfo(DataManager.Ins.playerData.currentLevelIndex).texture2d;
        nextPictureInImg.sprite = SpriteUtility.GetSpriteFromSolidColor(MinimapManager.Ins.initColor, texture2d);

        nextPictureInImg.rectTransform.sizeDelta = nextPictureOutImg.rectTransform.sizeDelta / 2 * 1.34f;
        if (texture2d.width > texture2d.height)
        {
            nextPictureInImg.ResizeImgKeepWidth();
        }
        else
        {
            nextPictureInImg.ResizeImgKeepHeight();
        }
    }

    public void MovePictures()
    {
        minimapRectTF.DOAnchorPos(new Vector2(-UIManager.Ins.screenWidth, minimapRectTF.anchoredPosition.y), 0.8f).SetEase(Ease.OutQuad);
        nextPictureOutImg.rectTransform.DOAnchorPos(new Vector2(0, nextPictureOutImg.rectTransform.anchoredPosition.y), 0.8f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            BlockUI.Ins.UnBlock();
        });
    }

    public void ScaleButtonStar()
    {
        buttonStarRectTF.transform.DOScale(1.1f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    public void Anim()
    {
        float oldYbot = buttonGalerryRectTF.anchoredPosition.y;
        float oldYtop = topRectTF.anchoredPosition.y;
        float delay = 0.1f;
        //reset
        buttonGalerryRectTF.anchoredPosition = new Vector2(buttonGalerryRectTF.anchoredPosition.x, -160f);
        buttonStarRectTF.anchoredPosition = new Vector2(buttonStarRectTF.anchoredPosition.x, -160f);
        buttonPlayRectTF.anchoredPosition = new Vector2(buttonPlayRectTF.anchoredPosition.x, -160f);
        topRectTF.anchoredPosition = new Vector2(topRectTF.anchoredPosition.x, 300f);
        //anim
        float waitTime = isWaitAnim ? 0.5f : 0f;
        DOVirtual.DelayedCall(waitTime, () =>
        {
            buttonGalerryRectTF.DOAnchorPos(new Vector2(buttonGalerryRectTF.anchoredPosition.x, oldYbot), 0.5f).SetEase(Ease.OutBack);
            buttonPlayRectTF.DOAnchorPos(new Vector2(buttonPlayRectTF.anchoredPosition.x, oldYbot), 0.5f).SetEase(Ease.OutBack).SetDelay(delay * 1);
            buttonStarRectTF.DOAnchorPos(new Vector2(buttonStarRectTF.anchoredPosition.x, oldYbot), 0.5f).SetEase(Ease.OutBack).SetDelay(delay * 2);
            topRectTF.DOAnchorPos(new Vector2(topRectTF.anchoredPosition.x, oldYtop), 0.5f).SetEase(Ease.OutBack).SetDelay(delay * 1);
        });
    }

    public void ButtonPlay()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadCurrentLevel();
        });
    }

    public void ButtonGallery()
    {

    }

    public void ButtonStar()
    {

    }

    public void ButtonSettings()
    {

    }

}
