using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Infooo")]
public class LevelInfooo : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<StageInfooo> stages;
}
