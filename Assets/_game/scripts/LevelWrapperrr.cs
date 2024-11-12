using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Wrapperrr")]
public class LevelWrapperrr : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<LevelInfooo> tutLevels;
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<LevelInfooo> levels;

    [Button]
    public void SortByPixelAmount()
    {
        levels.Sort((x, y) => x.pixelDatas.Count.CompareTo(y.pixelDatas.Count));
    }
}
