using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 pos = new Vector2(i, j);
                ArrowTile arrowTile = PoolManager.Ins.Spawn<ArrowTile>(PoolType.ArrowTile);
                arrowTile.transform.position = pos;
                DirectionType randomDirectionType = (DirectionType)UnityEngine.Random.Range(0, 4);
                arrowTile.OnInitt(pos, randomDirectionType, 5-j);
            }
        }
    }
}
