using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoosterHintUI : BoosterUI
{
    public override void Use()
    {
        base.Use();
        FinishUse();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void FinishUse()
    {
        base.FinishUse();

    }
}
