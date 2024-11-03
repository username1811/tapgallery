using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public RectTransform minimapRectTF;
    public BoosterMagnetUI boosterMagnetUI;
    public GameObject boosterButtons;


    private void Start()
    {
        minimapRectTF.AddComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<CheatPanel>();
        });
    }

    public override void Open()
    {
        base.Open();
        MinimapManager.Ins.originalMinimapPos = minimapRectTF.position;
        ShowBoosterMagnetUI(false);
        ShowBoosterButtons(true);
    }

    public void OnLoadLevel()
    {

    }

    public void ButtonBack()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
    }

    public void ShowBoosterMagnetUI(bool isShow)
    {
        boosterMagnetUI.gameObject.SetActive(isShow);
    }

    public void ShowBoosterButtons(bool isShow)
    {
        boosterButtons.gameObject.SetActive(isShow);
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
