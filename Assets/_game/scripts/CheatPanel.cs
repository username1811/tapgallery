using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class CheatPanel : UICanvas
{

    public CanvasGroup gameplayCanvasGroup;

    public TextMeshProUGUI iapText;
    public TextMeshProUGUI timeText;

    public TMP_InputField zoomSpeedInput;

    public override void Awake()
    {
        base.Awake();
        gameplayCanvasGroup = UIManager.Ins.GetUI<GamePlay>().gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void Open()
    {
        base.Open();
        rectTransform.anchoredPosition = Vector3.zero;
        iapText.text = CheatInput.Ins.isCheatIAP ? "Cheat IAP: ON" : "Cheat IAP: OFF";
    }

    /*public void ChangeBgColor()
    {
        if (bg == null)
        {
            bg = GameObject.FindGameObjectWithTag("gameplay-bg").GetComponent<Image>();
        }
        if (bg != null)
        {
            bg.sprite = null;
            bg.color = fcp.color;
            bgColor = bg.color;
        }
    }*/

    public void BtnShowHideUI()
    {
        if (gameplayCanvasGroup.alpha > 0.5f)
        {
            gameplayCanvasGroup.alpha = 0f;
        }
        else
        {
            gameplayCanvasGroup.alpha = 1f;
        }
    }

    public void ButtonWin()
    {
        LevelManager.Ins.WinImmediately();
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void ButtonLose()
    {
        LevelManager.Ins.LoseImmediately();
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void Button9999Gold()
    {
        DataManager.Ins.playerData.gold = 99999;
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void ButtonCheatIAP()
    {
        CheatInput.Ins.isCheatIAP = !CheatInput.Ins.isCheatIAP;
        iapText.text = CheatInput.Ins.isCheatIAP ? "Cheat IAP: ON" : "Cheat IAP: OFF";
    }

    public void ButtonCheatTime()
    {
        CheatInput.Ins.isCheatDateTime = !CheatInput.Ins.isCheatDateTime;
        timeText.text = CheatInput.Ins.isCheatDateTime ? "Cheat time: ON" : "Cheat time: OFF";
    }

    public void OnEndEditCheatZoomSpeed()
    {
        string inputStr = zoomSpeedInput.text;

        if (string.IsNullOrEmpty(inputStr))
        {
            return;
        }

        CameraManager.Ins.zoomMouseSpeed =float.Parse(inputStr);

        MoveFarAway();
    }

    public void BtnClose()
    {
        OnEndEditCheatZoomSpeed();
        MoveFarAway();
    }

    public void MoveFarAway()
    {
        rectTransform.anchoredPosition = new Vector3(99999f, 0, 0);
    }
}
