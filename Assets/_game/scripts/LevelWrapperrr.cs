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

    [Button]
    public void SortByHard()
    {
        SortByPixelAmount();
        List<LevelInfooo> newLevels = new();
        int groupCount = levels.Count / 5;
        for (int i = 0; i < groupCount; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                newLevels.Add(levels[i+groupCount*j]);
            }
        }
        levels = newLevels;
    }
}
