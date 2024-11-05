using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterMagnetUI : BoosterUI
{
    public Transform circle;
    public Transform buttonCancelTF;

    private void OnEnable()
    {
        circle.transform.localScale = Vector3.zero;
        circle.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine);
        buttonCancelTF.transform.localScale = Vector3.zero;
        buttonCancelTF.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine);
    }

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

    public void ButtonDirection(int directionType)
    {
        BoosterMagnet boosterMagnet = BoosterManager.Ins.currentBooster as BoosterMagnet;
        boosterMagnet.OnChooseDirection((DirectionType)directionType);
    }

    public void ButtonCancel()
    {
        BoosterMagnet boosterMagnet = BoosterManager.Ins.currentBooster as BoosterMagnet;
        boosterMagnet.OnCalcel(); 
    }
}
