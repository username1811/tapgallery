﻿using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class LevelManager : Singleton<LevelManager>
{
    public Level currentLevel;
    public LevelWrapperrr levelWrapperrr;
    public bool isEndLevel;
    public int levelIndexToReturn;
    public string debugLevelListStr;


    public void DestroyCurrentLevel()
    {
        if (currentLevel != null)
        {
            PoolManager.Ins.DespawnAll();
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
    }

    public int GetLoopLevelIndex(int levelIndex)
    {
        if (levelIndex >= levelWrapperrr.levels.Count)
            return ((levelIndex - levelIndexToReturn) % (levelWrapperrr.levels.Count - levelIndexToReturn) + levelIndexToReturn);
        else return levelIndex;
    }


    public void LoadLevel(int levelIndex)
    {
        isEndLevel = false;
        DestroyCurrentLevel();
        CreateStageFromLevelInfooo(GetLevelInfo(levelIndex), 0);
        currentLevel.OnInit();
        /*UIManager.Ins.OpenUI<Gameplay>();
        UIManager.Ins.GetUI<Gameplay>().OnLoadLevel();
        //FIREBASE
        //checkpoint start
        if (DataManager.Ins.playerData.currentLevelIndex > DataManager.Ins.playerData.maxCheckPointStartIndex)
        {
            FirebaseManager.Ins.CheckPointStart(DataManager.Ins.playerData.currentLevelIndex);
            DataManager.Ins.playerData.maxCheckPointStartIndex = DataManager.Ins.playerData.currentLevelIndex;
        }
        FirebaseManager.Ins.LevelStart(DataManager.Ins.playerData.currentLevelIndex);*/
    }

    public void LoadNextLevel()
    {
        DataManager.Ins.playerData.currentLevelIndex += 1;
        LoadLevel(DataManager.Ins.playerData.currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(DataManager.Ins.playerData.currentLevelIndex);
    }

    public void CheckWinLose()
    {
        StartCoroutine(IECheckWinLose()); 
        IEnumerator IECheckWinLose()
        {
            yield return null;
            if (isEndLevel)
            {
                yield break;
            }
            bool isWin = currentLevel.remainTiles.Count <= 0;
            if (isWin)//win
            {
                isEndLevel = true;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
                    {
                        LevelManager.Ins.LoadNextLevel();
                    });
                });
            }
            /*else if(isLose) //lose
            {
                isEndLevel = true;
            }*/
        }
    }

    public void WinImmediately()
    {
        /*foreach (var trackingItem in currentLevel.trackingItemCountList)
        {
            trackingItem.amount = 0;
        }*/
        CheckWinLose();
    }

    public void LoseImmediately()
    {
        /*currentLevel.moveLeft = 0;*/
        CheckWinLose();
    }

    public LevelInfooo GetLevelInfo(int levelIndex)
    {
        return levelWrapperrr.levels[GetLoopLevelIndex(levelIndex)];

    }

    public void CreateStageFromLevelInfooo(LevelInfooo levelInfo, int stageIndex)
    {
        StageInfooo stageInfooo = levelInfo.stages[stageIndex];
        GameObject stageObj = new GameObject("stage_" + stageInfooo.name);
        currentLevel = stageObj.AddComponent<Level>();
        currentLevel.stageInfooo = stageInfooo;
        foreach (var pixelData in stageInfooo.pixelDatas)
        {
            Vector2 pos = pixelData.coordinate;
            ArrowTile arrowTile = PoolManager.Ins.Spawn<ArrowTile>(PoolType.ArrowTile);
            arrowTile.transform.position = pos;
            arrowTile.OnInitt(pixelData);
            arrowTile.transform.SetParent(currentLevel.transform);
            currentLevel.tiles.Add(arrowTile);
        }
        RandomDirectionManager.Ins.InitDict();
        DOVirtual.DelayedCall(Time.deltaTime*2f, () =>
        {
            CameraManager.Ins.OnLoadLevel();
        });
    }

    /*[Button]
    public void ExportLevelList()
    {
        debugLevelListStr = "";
        for (int i = 0; i < levelSO.levelDatas.Count; i++)
        {
            LevelData levelData = levelSO.levelDatas[i];
            debugLevelListStr += (i + 1).ToString() + " " + levelData.levelPrefabName.ToString() + " " + levelData.tileCountStr;
            foreach (var str in levelData.tileCollideStrs)
            {
                debugLevelListStr += " " + str;
            }
            debugLevelListStr += "\n";
        }
    }*/
}
