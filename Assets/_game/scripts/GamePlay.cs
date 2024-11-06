using DG.Tweening;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public RectTransform minimapRectTF;
    public BoosterBombUI boosterBombUI;
    public BoosterMagnetUI boosterMagnetUI;
    public List<Transform> boosterButtons = new List<Transform>();  
    public Image blackImg;
    public Hearts hearts;


    private void Start()
    {
        return;
        minimapRectTF.AddComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<CheatPanel>();
        });
    }

    public override void Open()
    {
        base.Open();
        MinimapManager.Ins.originalMinimapPos = minimapRectTF.position;
        ShowBoosterBombUI(false);
        ShowBoosterMagnetUI(false);
        ShowBoosterButtons(true);
        blackImg.gameObject.SetActive(false);
        hearts.OnLoadLevel(5);
    }

    public void OnLoadLevel()
    {

    }

    public void ShowBoosterBombUI(bool isShow, Action OnComplete=null)
    {
        boosterBombUI.gameObject.SetActive(isShow);
        boosterBombUI.Show();
        OnComplete?.Invoke();
    }

    public void ShowBoosterMagnetUI(bool isShow, Action OnComplete = null)
    {
        boosterMagnetUI.gameObject.SetActive(isShow);
        OnComplete?.Invoke();
    }

    public void ShowBoosterButtons(bool isShow, Action OnComplete=null)
    {
        if (isShow)
        {
            foreach(var b in boosterButtons)
            {
                b.transform.localScale = Vector3.zero;
                b.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    OnComplete?.Invoke();
                });
            }
        }
        else
        {
            foreach (var b in boosterButtons)
            {
                b.transform.localScale = Vector3.one;
                b.transform.DOScale(0f, 0.2f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    OnComplete?.Invoke();
                });
            }
        }
    }

    public void OnWin()
    {
        return;
        blackImg.gameObject.SetActive(true);
        blackImg.color = new Color(0f, 0f, 0f, 0f);
        blackImg.DOColor(new Color(0f, 0f, 0f, 0.95f), 1.3f).SetEase(Ease.OutQuad);
    }


    //BUTTON
    public void ButtonBack()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
    }

    public void ButtonHint()
    {
        BoosterManager.Ins.UseBooster(BoosterType.Hint);
    }

    public void ButtonBomb()
    {
        BoosterManager.Ins.UseBooster(BoosterType.Bomb);
    }

    public void ButtonMagnet()
    {
        BoosterManager.Ins.UseBooster(BoosterType.Magnet);
    }
}
