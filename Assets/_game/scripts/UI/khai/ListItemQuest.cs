using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemQuest : MonoBehaviour
{
    public List<ItemTask> listTaskItem;
    public GameObject itemLevelPrefab;
    public Transform contentParent;
    public DailyQuestData dailyQuestData;

    private void Start()
    {
        GetListItem();
    }
    public void GetListItem()
    {


        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        listTaskItem.Clear();

        for(int i = 0; i< dailyQuestData.dataList.Count; i++)
        {
            GameObject itemObj = Instantiate(itemLevelPrefab, contentParent);
            ItemTask itemUI = itemObj.GetComponent<ItemTask>();

            itemUI.InitData(dailyQuestData.dataList[i]);

            listTaskItem.Add(itemUI);
        }
    }
}
