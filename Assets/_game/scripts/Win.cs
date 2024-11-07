using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : UICanvas
{
    public GameObject titleObj;
    public GameObject buttonContinueObj;
    public GameObject confettiObj;

    public override void Open()
    {
        base.Open();
        ShowElements();
        ShowConfetti();
        if(!LevelManager.Ins.currentLevelInfooo.isTut) DataManager.Ins.playerData.currentLevelIndex += 1;
        if (LevelManager.Ins.currentLevelInfooo.isTut) DataManager.Ins.playerData.isPassedTutLevel = true;
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

    public void ShowConfetti()
    {
        confettiObj.SetActive(false);
        StartCoroutine(IEShowConfetti());
        IEnumerator IEShowConfetti()
        {
            while(true)
            {
                confettiObj.SetActive(true);
                yield return new WaitForSeconds(7f);
                confettiObj.SetActive(false);
                yield return null;
            }
        }
    }

    public void ButtonContinue()
    {
        if (!LevelManager.Ins.currentLevelInfooo.isTut) UIManager.Ins.GetUI<Home>().isWin = true;
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
    }
}
