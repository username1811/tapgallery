using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;



public class BoosterBombUI : BoosterUI
{
    public RectTransform buttonBombFakeRectTF;
    public RectTransform buttonCancelRectTF;
    public Transform img;
    public TextMeshProUGUI tutText;

    public override void Use()
    {
        base.Use();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
       
    }

    public override void FinishUse()
    {
        base.FinishUse();
    }

    public void Show()
    {
        //btnbombfake
        buttonBombFakeRectTF.anchoredPosition = new Vector2(0, buttonBombFakeRectTF.anchoredPosition.y);
        buttonBombFakeRectTF.DOAnchorPos(new Vector2(-327f, buttonBombFakeRectTF.anchoredPosition.y), 0.4f).SetEase(Ease.OutQuad);
        //btncancel
        buttonCancelRectTF.anchoredPosition = new Vector2(0, buttonCancelRectTF.anchoredPosition.y);
        buttonCancelRectTF.DOAnchorPos(new Vector2(327f, buttonCancelRectTF.anchoredPosition.y), 0.4f).SetEase(Ease.OutQuad);
        //img
        img.transform.localScale = new Vector3(0.01f, 1, 1);
        img.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutQuad);
        //text
        Color c = tutText.color;
        c.a = 0.01f;
        tutText.color = c;
        tutText.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.1f);
    }

    public void ButtonCancel()
    {
        if(BoosterManager.Ins.currentBooster is BoosterBomb boosterBomb)
        {
            boosterBomb.OnCancel();
        }
    }
}
