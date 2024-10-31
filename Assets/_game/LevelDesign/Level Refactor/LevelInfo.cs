using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level_", menuName = "Scriptable Objects/Level/Level Info")]
public class LevelInfo : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<StageInfo> stages;
}
