using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HomeSwitcher : Singleton<HomeSwitcher>
{
    public TabType currentTabType;
    public bool isSwitching;
    public float duration;
    public bool isInited;
    public RectTransform container;


    private void Start()
    {
        
    }

    public void ShowAll()
    {
        container.gameObject.SetActive(true);
        ShowAllUICanvases();
    }

    public void HideAll()
    {
        container.gameObject.SetActive(false);
        HideAllUICanvases();
        Resett();
    }

    public void ShowAllUICanvases()
    {
        Debug.Log("homeswitcher show all");
        UIManager.Ins.OpenUI<Gallery>();
        DOVirtual.DelayedCall(Time.deltaTime * 3, () =>
        {
            UIManager.Ins.GetUI<Gallery>().rectTransform.anchoredPosition = new Vector2(-UIManager.Ins.screenWidth, 0);
        });
        UIManager.Ins.OpenUI<StarCollection>();
        DOVirtual.DelayedCall(Time.deltaTime * 3, () =>
        {
            UIManager.Ins.GetUI<StarCollection>().rectTransform.anchoredPosition = new Vector2(+UIManager.Ins.screenWidth, 0);
        });
        UIManager.Ins.GetUI<Home>().rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void HideAllUICanvases()
    {
        Debug.Log("homeswitcher hide all");
        UIManager.Ins.GetUI<Gallery>().rectTransform.anchoredPosition = Vector2.zero;
        UIManager.Ins.CloseUI<Gallery>();
        UIManager.Ins.GetUI<StarCollection>().rectTransform.anchoredPosition = Vector2.zero;
        UIManager.Ins.CloseUI<StarCollection>();
    }

    public void ShowTab(TabType tabType)
    {
        if (currentTabType == tabType) return;
        isSwitching = true;

        Action OnCompleteSwitching = () =>
        {
            isSwitching = false;
            currentTabType = tabType;
        };

        int offset = (int)tabType - (int)currentTabType;
        float offsetPixels = UIManager.Ins.screenWidth * offset;
        Debug.Log("offset pixels: " + offsetPixels);
        container.DOAnchorPosX(container.anchoredPosition.x - offsetPixels, duration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            OnCompleteSwitching?.Invoke();
        });

    }

    public void Resett()
    {
        container.anchoredPosition = Vector2.zero;
        currentTabType = TabType.Home;
    }
}

public enum TabType
{
    Gallery, Home, StarCollection
}

