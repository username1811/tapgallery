using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level_wrapper", menuName = "Scriptable Objects/Level/Level Wrapper")]
public class LevelWrapper : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<LevelInfooo> levels;
}
