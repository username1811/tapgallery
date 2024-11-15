using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Data Spin")]

public class DataSpin : ScriptableObject
{
    public List<SpinItem> dataList = new List<SpinItem>();

    public SpinItem GetDataByID(int idItem)
    {
        return dataList.Find(x => x.id == idItem);
    }
}


[System.Serializable]
public class SpinItem
{
    public int id;
    public DataSpinType typeBooster;
    public Sprite icon;
    public int Amount;
}

public enum DataSpinType
{
    Hint, Bomb, Magnet, Gold
}


