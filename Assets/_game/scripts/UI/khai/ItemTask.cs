using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTask : MonoBehaviour
{
    public Image task;
    public Image taskDone;

    public Text description;
    public Text GoalValue;
    public Text CurrentValue;

    public bool IsCompleted;
    public int RewardAmount;

    public Image fillImage;


    public void InitData(DailyItem dailyItem)
    {
        int goalValue = dailyItem.GoalValue;
        int currentValue = 0;

        description.text = dailyItem.Description;


        if (dailyItem.id == 1)
        {
            //currentValue = DataManager.Ins.playerData.goldDay;
            currentValue = 100;

        }if(dailyItem.id == 2)
        {
            //currentValue = DataManager.Ins.playerData.levelPassDay;
            currentValue = 4;
        }

        if (currentValue >= goalValue)
        {
            task.gameObject.SetActive(false);
            taskDone.gameObject.SetActive(true);
        }
        else
        {
            task.gameObject.SetActive(true);
            taskDone.gameObject.SetActive(false);
        }

        CurrentValue.text = currentValue.ToString();
        GoalValue.text = goalValue.ToString();

        float fillAmount = (float)currentValue / goalValue;
        fillImage.fillAmount = fillAmount;
    }
}
