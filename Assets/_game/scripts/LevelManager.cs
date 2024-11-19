using DG.Tweening;
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
    public LevelInfooo currentLevelInfooo;
    public static bool isLoadedLevel;


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

    public Action OnCompleteLoadLevel = () =>
    {
        RandomDirectionManager.Ins.InitDict();
        DOVirtual.DelayedCall(Time.deltaTime * 1f, () =>
        {
            CameraManager.Ins.OnLoadLevel();
            BoosterManager.Ins.OnLoadLevel();
            UIManager.Ins.OpenUI<GamePlay>();
            UIManager.Ins.GetUI<GamePlay>().OnLoadLevel();
            isLoadedLevel = true;
        });
    };

    public void LoadTut(int levelIndex)
    {
        isLoadedLevel = false;
        isEndLevel = false;
        DestroyCurrentLevel();
        currentLevelInfooo = GetTutLevelInfo(levelIndex);
        CreateLevelFromLevelInfooo(currentLevelInfooo, 0);
        currentLevel.OnInit(currentLevelInfooo.heartAmount);
        OnCompleteLoadLevel?.Invoke();
    }

    public void LoadLevel(int levelIndex)
    {
        isLoadedLevel = false;
        isEndLevel = false;
        DestroyCurrentLevel();
        currentLevelInfooo = GetLevelInfo(levelIndex);
        CreateLevelFromLevelInfooo(currentLevelInfooo, 0);
        currentLevel.OnInit(currentLevelInfooo.heartAmount);
        OnCompleteLoadLevel?.Invoke();
        /*//FIREBASE
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
            bool isWin = currentLevel.remainTilesCount <= 0;
            bool isLose = currentLevel.heartAmount <= 0;
            if (isWin)//win
            {
                isEndLevel = true;
                UIManager.Ins.OpenUI<Win>();
                UIManager.Ins.GetUI<GamePlay>().OnWin();
                DataManager.Ins.playerData.passedLevelNames.Add(currentLevelInfooo.name);

                if (DataManager.Ins.CheckIsToDay() == true)
                {
                    DataManager.Ins.PassLevelToDay();

                    Debug.Log("dung la trong ngay");
                }
            }
            else if (isLose) //lose
            {
                isEndLevel = true;
                UIManager.Ins.OpenUI<Revive>();
                UIManager.Ins.GetUI<GamePlay>().OnLose();
            }
        }
    }

    public void WinImmediately()
    {
        /*foreach (var trackingItem in currentLevel.trackingItemCountList)
        {
            trackingItem.amount = 0;
        }*/
        currentLevel.remainTilesCount = 0;
        CheckWinLose();
    }

    public void LoseImmediately()
    {
        /*currentLevel.moveLeft = 0;*/
        CheckWinLose();
    }

    public LevelInfooo GetTutLevelInfo(int levelIndex)
    {
        return levelWrapperrr.tutLevels[GetLoopLevelIndex(levelIndex)];
    }

    public LevelInfooo GetLevelInfo(int levelIndex)
    {
        return levelWrapperrr.levels[GetLoopLevelIndex(levelIndex)];
    }

    public void CreateLevelFromLevelInfooo(LevelInfooo levelInfo, int levelIndex)
    {
        LevelInfooo levelInfoo = levelInfo;
        GameObject levelObj = new GameObject("level_" + levelInfoo.name);
        currentLevel = levelObj.AddComponent<Level>();
        currentLevel.levelInfooo = levelInfoo;
        foreach (var pixelData in levelInfoo.pixelDatas)
        {
            Vector2 pos = pixelData.coordinate;
            ArrowTile arrowTile = PoolManager.Ins.Spawn<ArrowTile>(PoolType.ArrowTile);
            arrowTile.transform.position = pos;
            arrowTile.OnInitt(pixelData);
            arrowTile.transform.SetParent(currentLevel.transform);
            currentLevel.tiles.Add(arrowTile);
        }
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
