using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterMagnetUI : BoosterUI
{
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
