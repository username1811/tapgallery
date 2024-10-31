using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatInput : Singleton<CheatInput> 
{
    public bool isCheatDateTime;

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
    }
}
