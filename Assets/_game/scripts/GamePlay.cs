using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public RectTransform minimapRectTF;

    public override void Open()
    {
        base.Open();
        MinimapManager.Ins.originalMinimapPos = minimapRectTF.position;
    }

    public void OnLoadLevel()
    {

    }
}
