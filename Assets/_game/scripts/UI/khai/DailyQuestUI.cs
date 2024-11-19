using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestUI : UICanvas
{
    public DailyQuestData questData;
    public Image taskImg;
    public Image taskDone;

    public Image fillProgres;
    public Text currentValue;
    public Text GoalValue;


    public override void Open()
    {
        base.Open();

    }


    public void BackBtn()
    {

        UIManager.Ins.OpenUI<Home>();

        UIManager.Ins.CloseUI<DailyQuestUI>();
    }

    public void showTask()
    {
        int listTask = questData.dataList.Count;
        int taskComplete = DataManager.Ins.playerData.TaskComplete;
    }

}
