using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestUI : UICanvas
{

    public override void Open()
    {
        base.Open();

    }


    public void BackBtn()
    {

        UIManager.Ins.OpenUI<Home>();

        UIManager.Ins.CloseUI<DailyQuestUI>();
    }

}
