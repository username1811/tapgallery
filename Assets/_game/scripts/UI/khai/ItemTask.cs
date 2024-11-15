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


    public void InitData(DailyItem dailyItem)
    {
        description.text = dailyItem.Description;
        GoalValue.text = dailyItem.GoalValue.ToString();

    }
}
