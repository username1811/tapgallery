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
    public List<ArrowTile> remainTiles = new List<ArrowTile>();
    public StageInfooo stageInfooo;

    public void OnInit()
    {
        remainTiles = new List<ArrowTile>(tiles);
        /*foreach (var tile in tiles)
        {
            tile.OnInitt();
        }*/
    }
#if UNITY_EDITOR
    [Button]
    public void SaveDirectionToStageInfoo()
    {
        foreach(var tile in tiles) {
            tile.pixelData.directionType = tile.directionType;
        }
        EditorUtility.SetDirty(stageInfooo);
    }
#endif
}