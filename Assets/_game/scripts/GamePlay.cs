using DG.Tweening;
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
    public GameObject boosterButtons;
    public Image blackImg;


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
    }

    public void OnLoadLevel()
    {

    }

    public void ShowBoosterBombUI(bool isShow)
    {
        boosterBombUI.gameObject.SetActive(isShow);
    }

    public void ShowBoosterMagnetUI(bool isShow)
    {
        boosterMagnetUI.gameObject.SetActive(isShow);
    }

    public void ShowBoosterButtons(bool isShow)
    {
        boosterButtons.gameObject.SetActive(isShow);
    }

    public void OnWin()
    {
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
