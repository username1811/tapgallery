using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Level : MonoBehaviour
{
    public List<ArrowTile> tiles = new List<ArrowTile>();
    public int remainTilesCount;
    public StageInfooo stageInfooo;

    public void OnInit()
    {
        remainTilesCount = tiles.Count;
        /*foreach (var tile in tiles)
        {
            tile.OnInitt();
        }*/
    }
    [Button]
    public void SaveDirectionToStageInfoo()
    {
        foreach(var tile in tiles) {
            tile.pixelData.directionType = tile.directionType;
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(stageInfooo);
#endif
    }
}
