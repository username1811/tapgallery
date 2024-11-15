using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    [Header("block UI : ")]
    public GameObject blockUI;

    [Header("top bot : ")]
    public Image top;
    public Image bot;
    public float halfDuration;

    [Header("logo:")]
    public Image logo;
    public float angle;
    public CanvasGroup logoCanvasGroup;
    public Tween rotateLogoTween;
    public Tween fadeLogoTween;
    public float screenHeight => UIManager.Ins.screenHeight;
    public Action OnCompleteFadeOut;
    public RectTransform D;
    public RectTransform O;
    public RectTransform N;
    public RectTransform E;




    public override void Open()
    {
        base.Open();
        if (LevelManager.Ins.currentLevelInfooo.isTut) DataManager.Ins.playerData.isPassedTutLevel = true;
        if (!LevelManager.Ins.currentLevelInfooo.isTut && !LevelManager.Ins.currentLevelInfooo.isTheme) {
            DataManager.Ins.playerData.currentLevelIndex += 1;
            DataManager.Ins.playerData.passedLevelNames.Add(LevelManager.Ins.currentLevelInfooo.name);
        }
        if (LevelManager.Ins.currentLevelInfooo.isTheme)
        {
            ThemeInfo themeInfo = ThemeManager.Ins.currentThemeInfo;
            ThemeLevelsData themeLevelsData = DataManager.Ins.playerData.themeLevelsDatas.FirstOrDefault(x=>x.themeType== themeInfo.themeType);
            if (themeLevelsData==null) {
                themeLevelsData = new ThemeLevelsData(themeInfo.themeType);
                DataManager.Ins.playerData.themeLevelsDatas.Add(themeLevelsData);
            }
            themeLevelsData.passedIndexs.Add(themeInfo.levelInfooos.IndexOf(LevelManager.Ins.currentLevelInfooo));
        }
        OnCompleteFadeOut = () =>
        {
            if (!LevelManager.Ins.currentLevelInfooo.isTut && !LevelManager.Ins.currentLevelInfooo.isTheme) UIManager.Ins.GetUI<Home>().isWin = true;
            if (!LevelManager.Ins.currentLevelInfooo.isTut && !LevelManager.Ins.currentLevelInfooo.isTheme)
            {
                PoolManager.Ins.DespawnAll();
                UIManager.Ins.OpenUI<Home>();
                this.transform.SetAsLastSibling();
                AnimDone();
            }
            if (LevelManager.Ins.currentLevelInfooo.isTheme)
            {
                PoolManager.Ins.DespawnAll();
                UIManager.Ins.OpenUI<Home>();
                UIManager.Ins.OpenUI<ThemeCollection>();
                UIManager.Ins.GetUI<ThemeCollection>().ShowThemeLevelsPopUp(true);
                UIManager.Ins.GetUI<ThemeCollection>().themeLevelsPopUp.OnInitt(ThemeManager.Ins.currentThemeInfo);
                this.transform.SetAsLastSibling();
            }
        };
        FadeOut(() =>
        {
            OnCompleteFadeOut?.Invoke();
            DOVirtual.DelayedCall(1.2f, () =>
            {
                FadeIn(() =>
                {
                    UIManager.Ins.CloseUI<Win>();
                });
            });
        });
    }

    public void AnimDone()
    {
        float delay = 0.1f;
        float distance = 33f;
        float duration = 0.2f;
        D.DOAnchorPos(new Vector2(D.anchoredPosition.x, distance), duration).SetEase(Ease.OutSine).SetDelay(delay*0).SetLoops(2, LoopType.Yoyo);
        O.DOAnchorPos(new Vector2(O.anchoredPosition.x, distance), duration).SetEase(Ease.OutSine).SetDelay(delay*1).SetLoops(2, LoopType.Yoyo);
        N.DOAnchorPos(new Vector2(N.anchoredPosition.x, distance), duration).SetEase(Ease.OutSine).SetDelay(delay*2).SetLoops(2, LoopType.Yoyo);
        E.DOAnchorPos(new Vector2(E.anchoredPosition.x, distance), duration).SetEase(Ease.OutSine).SetDelay(delay*3).SetLoops(2, LoopType.Yoyo);
    }

    public void FadeOut(Action OnComplete)
    {
        //topbot
        top.rectTransform.sizeDelta = new Vector2(0, 0);
        bot.rectTransform.sizeDelta = new Vector2(0, 0);
        DOVirtual.Float(0, screenHeight / 2, halfDuration, v =>
        {
            top.rectTransform.sizeDelta = new Vector2(0, v);
            bot.rectTransform.sizeDelta = new Vector2(0, v);
        }).SetEase(Ease.OutSine).OnComplete(() =>
        {
            fadeLogoTween?.Kill();
            rotateLogoTween?.Kill();
            SetLogoAlpha(1);
            SetLogoRotation(0);
            OnComplete?.Invoke();
        });
        //logo
        logo.gameObject.SetActive(true);
        SetLogoAlpha(0);
        fadeLogoTween = logoCanvasGroup.DOFade(1, halfDuration * 0.5f).SetEase(Ease.OutSine).SetDelay(halfDuration * 0.5f);
        SetLogoRotation(angle);
        rotateLogoTween = logo.transform.DORotate(Vector3.zero, halfDuration * 0.5f).SetEase(Ease.OutSine).SetDelay(halfDuration * 0.5f);
    }

    public void FadeIn(Action OnComplete)
    {
        //topbot
        top.rectTransform.sizeDelta = new Vector2(0, screenHeight / 2);
        bot.rectTransform.sizeDelta = new Vector2(0, screenHeight / 2);
        DOVirtual.Float(screenHeight / 2, 0, halfDuration * 0.7f, v =>
        {
            top.rectTransform.sizeDelta = new Vector2(0, v);
            bot.rectTransform.sizeDelta = new Vector2(0, v);
        }).SetEase(Ease.InSine).OnComplete(() =>
        {
            fadeLogoTween?.Kill();
            rotateLogoTween?.Kill();
            SetLogoAlpha(0);
            SetLogoRotation(angle);
            logo.gameObject.SetActive(false);
            OnComplete?.Invoke();
        });
        //logo
        SetLogoAlpha(1);
        fadeLogoTween = logoCanvasGroup.DOFade(0, halfDuration * 0.7f * 0.5f).SetEase(Ease.InSine);
        SetLogoRotation(0);
        rotateLogoTween = logo.transform.DORotate(new Vector3(0, 0, angle), halfDuration * 0.7f * 0.5f).SetEase(Ease.InSine);
    }

    public void SetLogoAlpha(float a)
    {
        //logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, a);
        logoCanvasGroup.alpha = a;
    }

    public void SetLogoRotation(float angleZ)
    {
        logo.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleZ));
    }

}
