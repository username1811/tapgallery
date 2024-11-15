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
    [Title("conffetti:")]
    public GameObject confetti;
    [Title("anim picture:")]
    public Vector3[] points = new Vector3[] { };
    public RectTransform pictureLeader;
    public float animPictureDuration;
    public Vector2 pictureLeaderOldPos;


    public static bool isWaitAnim = true;


    private void Start()
    {
        pictureLeaderOldPos = minimapRectTF.position;
    }

    public override void Open()
    {
        base.Open();
        Init();
        Refresh();
        CameraManager.Ins.cam.transform.position = Vector3.zero;
        StarAnimManager.Ins.OnOpenHome();
        AnimElements();
        isWin = false;
        isWaitAnim = true;
        ShowConfetti(false);
        minimapRectTF.gameObject.SetActive(true);
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
        MinimapManager.Ins.OnOpenHome(isWin, () => { MovePicture2(); });
    }

    public void RefreshPicturePositions()
    {
        minimapRectTF.anchoredPosition = new Vector2(0, minimapRectTF.anchoredPosition.y);
        nextPictureOutImg.rectTransform.anchoredPosition = new Vector2(0, nextPictureOutImg.rectTransform.anchoredPosition.y);
        minimapRectTF.transform.SetAsLastSibling();
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
        Vector2 oldPos = minimapRectTF.anchoredPosition;
        minimapRectTF.position = buttonGalerryRectTF.position;
        Vector2 targetMove = minimapRectTF.anchoredPosition;
        minimapRectTF.anchoredPosition = oldPos;
        Transform oldParent = minimapRectTF.transform.parent;
        minimapRectTF.transform.SetParent(this.transform);
        minimapRectTF.transform.localScale = Vector3.one;
        minimapRectTF.transform.DOScale(0.22f, 0.8f).SetEase(Ease.OutQuad);
        minimapRectTF.DOAnchorPos(targetMove, 0.8f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            BlockUI.Ins.UnBlock();
            minimapRectTF.transform.SetParent(oldParent);
            minimapRectTF.anchoredPosition = oldPos;
            minimapRectTF.transform.SetAsFirstSibling();
            minimapRectTF.transform.localScale = Vector3.one;
            ScaleButtonGallery();
        });
    }

    public void ScaleButtonGallery()
    {
        buttonGalerryRectTF.transform.DOScale(1.1f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    public void ScaleButtonStar()
    {
        buttonStarRectTF.transform.DOScale(1.1f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    public void AnimElements()
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

    public void ShowConfetti(bool isShow)
    {
        confetti.SetActive(isShow);
    }


    public void Follow()
    {
        StartCoroutine(IEFollow());
        IEnumerator IEFollow()
        {
            float timer = animPictureDuration+1f;
            while (timer > 0f)
            {
                minimapRectTF.position = pictureLeader.position;
                timer -= Time.deltaTime;
                yield return null;
            }
            minimapRectTF.position = pictureLeaderOldPos;
            yield return null;
        }
    }

    [Button]
    public void MovePicture2(Action OnComplete = null)
    {
        minimapRectTF.gameObject.SetActive(true);
        minimapRectTF.transform.localScale = Vector3.one;


        pictureLeader.position = pictureLeaderOldPos;
        Vector2 oldPos = minimapRectTF.anchoredPosition;
        Transform oldParent = minimapRectTF.transform.parent;
        minimapRectTF.transform.SetParent(this.transform);
        Vector2 targetMove = buttonGalerryRectTF.position;
        Debug.Log("target move : " +  targetMove);

        Follow();

        pictureLeader.DOAnchorPosX(targetMove.x, animPictureDuration).SetEase(Ease.InSine).OnComplete(() =>
        {
            StopAllCoroutines();
            BlockUI.Ins.UnBlock();
            minimapRectTF.transform.SetParent(oldParent);
            minimapRectTF.anchoredPosition = oldPos;
            minimapRectTF.transform.SetAsLastSibling();
            minimapRectTF.transform.localScale = Vector3.one;
            minimapRectTF.gameObject.SetActive(false);
            pictureLeader.position = pictureLeaderOldPos;
            minimapRectTF.position = pictureLeaderOldPos;
            ScaleButtonGallery();
            OnComplete?.Invoke();
        });
        pictureLeader.DOAnchorPosY(targetMove.y, animPictureDuration+Time.deltaTime*2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            pictureLeader.position = pictureLeaderOldPos;
        });
        minimapRectTF.DOScale(0.22f, animPictureDuration).OnComplete(() =>
        {
            minimapRectTF.transform.localScale = Vector3.one;
        });
        minimapRectTF.DORotate(new Vector3(0, 0, 359f), animPictureDuration, RotateMode.FastBeyond360).SetEase(Ease.InSine).OnComplete(() =>
        {
            minimapRectTF.rotation = Quaternion.Euler(Vector3.zero);
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
