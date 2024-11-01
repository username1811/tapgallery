using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatInput : Singleton<CheatInput> 
{
    public bool isCheatDateTime; 
    public bool isCheatIAP;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnManager.Ins.Spawn();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
            {
                LevelManager.Ins.LoadNextLevel();
            });
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LevelManager.Ins.WinImmediately();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RandomDirectionManager.Ins.RandomDirection();
            RandomDirectionManager.Ins.FixStuck();
            LevelManager.Ins.currentLevel.SaveDirectionToStageInfoo();
        }
    }
}
