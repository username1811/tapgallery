﻿using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomDirectionManager : Singleton<RandomDirectionManager>
{
    public List<ArrowTile> arrowTiles => LevelManager.Ins.currentLevel.tiles;
    [ShowInInspector]
    public Dictionary<int, List<ArrowTile>> dict = new Dictionary<int, List<ArrowTile>>();
    public int stuckCount;
    public bool isFixingStuck;
    public int acceptPercent;


    [Button]
    public void FixStuck()
    {
        isFixingStuck = true;
        List<ArrowTile> tempArrowTiles = new List<ArrowTile>(LevelManager.Ins.currentLevel.tiles);
        StartCoroutine(IEAutoPlay());
        IEnumerator IEAutoPlay()
        {
            while (true)
            {
                ArrowTile arrowTile = tempArrowTiles.FirstOrDefault(x => x.gameObject.activeInHierarchy && x.IsCanEat());
                if (arrowTile == null) break;
                PoolManager.Ins.Despawn(PoolType.ArrowTile, arrowTile.gameObject);
                tempArrowTiles.Remove(arrowTile);
                yield return null;
            }
            Debug.Log("tempArrowTiles count = " + tempArrowTiles.Count);
            stuckCount = 0;
            List<ArrowTile> stuckRightTiles = new();
            if(tempArrowTiles.Count > 0) //stuck level 
            {
                foreach(var arrowTile in  tempArrowTiles)
                {
                    if(arrowTile.directionType == DirectionType.Right)
                    {
                        Debugger.DrawCircle(arrowTile.transform.position, 0.3f, Color.black, 1f);
                        RaycastHit2D[] hits = Physics2D.RaycastAll(arrowTile.transform.position, Vector2.right, 100f, GameManager.Ins.arrowTileMask);
                        foreach(RaycastHit2D hit in hits)
                        {
                            ArrowTile needToFixTile = hit.collider.GetComponent<ArrowTile>();
                            needToFixTile.directionType = DirectionType.Right;
                            needToFixTile.RefreshDirection();
                            Debugger.DrawCircle(arrowTile.transform.position, 0.3f, Color.blue, 3f);
                            if (!stuckRightTiles.Contains(needToFixTile))
                            {
                                stuckRightTiles.Add(needToFixTile);
                            }
                        }
                    }
                }
            }
            foreach(var arrowTile in arrowTiles)
            {
                arrowTile.gameObject.SetActive(true);
            }
            stuckCount = stuckRightTiles.Count;
            isFixingStuck = false;
            yield return null;
        }
    }

    [Button]
    public void InitDict()
    {
        dict.Clear();
        for (int i = 1; i < 50; i++)
        {
            int indexx = i;
            if (!arrowTiles.Any(x => x.sortingOrder == indexx)) break;
            dict[indexx] = new List<ArrowTile>(arrowTiles.Where(x => x.sortingOrder == indexx));
        }
    }

    [Button]
    public void RandomDirection()
    {
        foreach (var kvp in dict)
        {
            int key = kvp.Key;
            List<ArrowTile> lineTiles = kvp.Value;
            //chọn vị trí ngăn cách trái phải cho line
            int splitLeftRightIndex = UnityEngine.Random.Range(0, lineTiles.Count);
            //đã xác định vị trí split, bên trái hướng sang trái, bên phải hướng sang phải
            List<ArrowTile> leftTiles = new();
            for (int i = 0; i < splitLeftRightIndex; i++)
            {
                leftTiles.Add(lineTiles[i]);
            }
            List<ArrowTile> rightTiles = new();
            for (int i = splitLeftRightIndex; i < lineTiles.Count; i++)
            {
                rightTiles.Add(lineTiles[i]);
            }
            //xác định số lượng tile đi theo hướng đó
            int randomLeftTileAmount = UnityEngine.Random.Range(0, leftTiles.Count);
            int randomRightTileAmount = UnityEngine.Random.Range(0, rightTiles.Count);
            //set hướng chuẩn
            for (int i = 0; i < leftTiles.Count; i++)
            {
                leftTiles[i].directionType = Random.Range(0, 2) == 0 ? DirectionType.Left : GetDirectionUpOrDown(leftTiles[i]);
            }
            for (int i = 0; i < rightTiles.Count; i++)
            {
                rightTiles[i].directionType = Random.Range(0, 2) == 0 ? DirectionType.Right : GetDirectionUpOrDown(rightTiles[i]);
            }

            lineTiles.ForEach(x => x.RefreshDirection());
        }
    }

    public DirectionType GetDirectionUpOrDown(ArrowTile tile)
    {
        bool isOpposite = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(tile.transform.position, Vector2.up, 100f, GameManager.Ins.arrowTileMask);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == tile.gameObject) continue;
            ArrowTile arrowTile = hit.collider.GetComponent<ArrowTile>();
            if (arrowTile == null) continue;
            if (arrowTile.directionType == DirectionType.Left || arrowTile.directionType == DirectionType.Right) continue;
            if (arrowTile.directionType == DirectionType.Down)
            {
                isOpposite = true;
                break;
            }
        }
        if (isOpposite) { 
            return DirectionType.Down;
        }
        else
        {
            int halfSortingOrder = dict.Count / 2;
            if(tile.sortingOrder <= halfSortingOrder) // nửa trên ưu tiên hướng lên trên
            {
                return UnityEngine.Random.Range(0, 4) < 3 ? DirectionType.Up : DirectionType.Down;
            }
            else // nửa dưới ưu tiên hướng xuống dưới
            {
                return UnityEngine.Random.Range(0, 4) < 3 ? DirectionType.Down : DirectionType.Up;
            }
        }
    }

    [Button]
    public void RandomDirectionAndSaveAllLevels()
    {
        StartCoroutine(IERandom());
        IEnumerator IERandom()
        {
            while (DataManager.Ins.playerData.currentLevelIndex < LevelManager.Ins.levelWrapperrr.levels.Count)
            {
                yield return null;
                yield return new WaitForSeconds(0.5f);
                if (LevelManager.Ins.currentLevelInfooo.pixelDatas.Any(x=>x.directionType != DirectionType.Up))
                {
                }
                else
                {
                    stuckCount = 999;
                    int tryCount = 50;
                    while (stuckCount > LevelManager.Ins.currentLevel.levelInfooo.pixelDatas.Count * (float)acceptPercent / 100f && tryCount > 0)
                    {
                        RandomDirection();
                        FixStuck();
                        yield return new WaitUntil(() => !isFixingStuck);
                        tryCount -= 1;
                    }
                    LevelManager.Ins.currentLevel.SaveDirectionToLevelInfoo();
                    Debug.Log("done random direction " + LevelManager.Ins.currentLevel.gameObject.name);
                    yield return new WaitForSeconds(0.5f);
                }
                LevelManager.Ins.LoadNextLevel();
            }
            yield return null;
        }
    }
}
