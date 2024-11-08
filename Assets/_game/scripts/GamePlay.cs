using DG.Tweening;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Hand hand;
    public GameObject buttonBackObj;
    public RectTransform topRectTF;
    public RectTransform botRectTF;
    public TextMeshProUGUI boosterHintAmountText;
    public TextMeshProUGUI boosterBombAmountText;
    public TextMeshProUGUI boosterMagnetAmountText;


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
    }

    public void OnLoadLevel()
    {
        MinimapManager.Ins.originalMinimapPos = minimapRectTF.position;
        ShowBoosterBombUI(false);
        ShowBoosterMagnetUI(false);
        ShowBoosterButtons(true);
        blackImg.gameObject.SetActive(false);
        hearts.OnLoadLevel(LevelManager.Ins.currentLevel.heartAmount);
        buttonBackObj.SetActive(!LevelManager.Ins.currentLevelInfooo.isTut);
        Anim();
        Refresh();
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
        Vector3 resetScale = isShow ? Vector3.zero : Vector3.one;
        float targetScale = isShow ? 1f : 0f;
        float duration = isShow ? 0.35f : 0.2f;

        foreach (var b in boosterButtons)
        {
            b.transform.localScale = resetScale;
            b.transform.DOScale(targetScale, duration).SetEase(Ease.OutSine).OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
        }
    }

    public void Anim()
    {
        float oldYtop = topRectTF.anchoredPosition.y;
        float oldYbot = botRectTF.anchoredPosition.y;
        float delay = 0.1f;
        //reset
        topRectTF.anchoredPosition = new Vector2(topRectTF.anchoredPosition.x, 300f);
        botRectTF.anchoredPosition = new Vector2(botRectTF.anchoredPosition.x, -600f);
        //anim
        float waitTime = 0.4f;
        DOVirtual.DelayedCall(waitTime, () =>
        {
            topRectTF.DOAnchorPos(new Vector2(topRectTF.anchoredPosition.x, oldYtop), 0.5f).SetEase(Ease.OutBack).SetDelay(delay * 1);
            botRectTF.DOAnchorPos(new Vector2(botRectTF.anchoredPosition.x, oldYbot), 0.5f).SetEase(Ease.OutBack).SetDelay(delay * 1);
        });
    }


    public void OnWin()
    {
        return;
        blackImg.gameObject.SetActive(true);
        blackImg.color = new Color(0f, 0f, 0f, 0f);
        blackImg.DOColor(new Color(0f, 0f, 0f, 0.95f), 1.3f).SetEase(Ease.OutQuad);
    }

    public void OnLose()
    {
        return;
        blackImg.gameObject.SetActive(true);
        blackImg.color = new Color(0f, 0f, 0f, 0f);
        blackImg.DOColor(new Color(0f, 0f, 0f, 0.95f), 1.3f).SetEase(Ease.OutQuad);
    }

    public void Refresh()
    {
        boosterHintAmountText.text = DataManager.Ins.playerData.boosterHintAmount.ToString();
        boosterBombAmountText.text = DataManager.Ins.playerData.boosterBombAmount.ToString();
        boosterMagnetAmountText.text = DataManager.Ins.playerData.boosterMagnetAmount.ToString();
    }


    //BUTTON
    public void ButtonBack()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
    }

    public void ButtonSettings()
    {

    }

    public void ButtonHint()
    {
        if(DataManager.Ins.playerData.boosterHintAmount > 0)
        {
            BoosterManager.Ins.UseBooster(BoosterType.Hint);
        }
    }

    public void ButtonBomb()
    {
        if (DataManager.Ins.playerData.boosterBombAmount > 0)
        {
            BoosterManager.Ins.UseBooster(BoosterType.Bomb);
        }
    }

    public void ButtonMagnet()
    {
        if (DataManager.Ins.playerData.boosterMagnetAmount > 0)
        {
            BoosterManager.Ins.UseBooster(BoosterType.Magnet);
        }
    }
}
