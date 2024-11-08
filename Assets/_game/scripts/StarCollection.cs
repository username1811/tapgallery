using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StarCollection : UICanvas
{
    public bool isInited;

    public override void Open()
    {
        base.Open();
        Init();
        Refresh();
    }

    public void Init()
    {
        if (isInited) return;
        isInited = true;
    }

    [Button]
    public void Refresh()
    {
  
    }

}
