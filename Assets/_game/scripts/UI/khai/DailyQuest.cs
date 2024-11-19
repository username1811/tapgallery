using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Data DailyQuest")]
public class DailyQuestData : ScriptableObject
{
    public List<DailyItem> dataList = new List<DailyItem>();

    public DailyItem GetDataByID(int idItem)
    {
        return dataList.Find(x => x.id == idItem);
    }

}

[System.Serializable]
public class DailyItem
{
    public int id;
    public string Description;
    public int CurrentValue;
    public int GoalValue;
    public bool IsCompleted => CurrentValue >= GoalValue;
    public int RewardAmount;
    public DataReward DataReward;
}

public enum DataReward
{
    Gold, Ticket, Magnet, Hint, Bomb
}
