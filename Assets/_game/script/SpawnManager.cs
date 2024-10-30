using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public List<ArrowTile> arrowTiles = new List<ArrowTile>();
    public int width;
    public int height;


    private void Start()
    {
        Spawn();
    }

    [Button]
    public void Spawn()
    {
        PoolManager.Ins.DespawnAll();
        arrowTiles.Clear();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j);
                ArrowTile arrowTile = PoolManager.Ins.Spawn<ArrowTile>(PoolType.ArrowTile);
                arrowTile.transform.position = pos;
                DirectionType randomDirectionType = (DirectionType)UnityEngine.Random.Range(0, 4);
                arrowTile.OnInitt(pos, randomDirectionType, height - j);
                arrowTiles.Add(arrowTile);
            }
        }
        RandomDirectionManager.Ins.InitDict();
        DOVirtual.DelayedCall(Time.deltaTime, () =>
        {
            RandomDirectionManager.Ins.RandomDirection();
            RandomDirectionManager.Ins.FixStuck();
            CameraManager.Ins.InitLimitPosition();
        });
    }
}
