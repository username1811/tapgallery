using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : UICanvas
{
    public GameObject titleObj;
    public GameObject buttonContinueObj;

    public override void Open()
    {
        base.Open();
        //ShowElements();
    }

    public void ShowElements()
    {
        titleObj.SetActive(false);
        buttonContinueObj.SetActive(false);
        titleObj.transform.localScale = Vector3.one * 0.2f;
        titleObj.SetActive(true);
        titleObj.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            buttonContinueObj.transform.localScale = Vector3.one * 0.2f;
            buttonContinueObj.SetActive(true);
            buttonContinueObj.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        });
    }

    public void ButtonContinue()
    {
        LevelManager.Ins.currentLevel.AddHeart(3);
        LevelManager.Ins.isEndLevel = false;
        UIManager.Ins.CloseUI<Revive>();
    }

    public void ButtonRestart()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadCurrentLevel();
        });
    }
}
