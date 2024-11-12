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
    public LevelInfooo levelInfooo;
    public int heartAmount;
    private int initialHeartAmount;

    public void OnInit(int heartAmount)
    {
        this.heartAmount = heartAmount; 
        this.initialHeartAmount = heartAmount;
        remainTilesCount = tiles.Count;
        /*foreach (var tile in tiles)
        {
            tile.OnInitt();
        }*/
    }

    public void AddHeart(int amount)
    {
        heartAmount += amount;
        if (heartAmount > initialHeartAmount)
        {
            heartAmount = initialHeartAmount;
        }
        LevelManager.Ins.CheckWinLose();
        UIManager.Ins.GetUI<GamePlay>().hearts.AddHeart(amount);
    }

    public void SubtractHeart(int amount)
    {
        heartAmount -= amount;
        if (heartAmount < 0)
        {
            heartAmount = 0;
        }
        LevelManager.Ins.CheckWinLose();
        UIManager.Ins.GetUI<GamePlay>().hearts.SubtractHeart(amount);
    }


    [Button]
    public void SaveDirectionToLevelInfoo()
    {
        foreach(var tile in tiles) {
            tile.pixelData.directionType = tile.directionType;
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(levelInfooo);
#endif
    }
}
