using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterManager : Singleton<BoosterManager>
{
    public Booster currentBooster;
    public bool isUsingBooster = false;

    [SerializeReference]
    public List<Booster> boosters = new List<Booster>();    

    public void OnLoadLevel()
    {
        currentBooster = null;
        isUsingBooster = false;
    }

    private void Update()
    {
        currentBooster?.OnUpdate();
    }

    [Button]
    public void UseBooster(BoosterType boosterType)
    {
        if (isUsingBooster) return; // đang dùng booster thì k dùng nữa
        currentBooster = GetBooster(boosterType);
        currentBooster?.Use();
    }

    public Booster GetBooster(BoosterType boosterType)
    {
        return boosters.FirstOrDefault(x => x.boosterType == boosterType);
    }



}

public enum BoosterType
{
    Hint, Bomb, Magnet
}

[Serializable]
public class Booster
{
    public BoosterType boosterType;
    public Sprite icon;

    public virtual void Use()
    {
        BoosterManager.Ins.isUsingBooster = true;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void FinishUse()
    {
        BoosterManager.Ins.currentBooster = null;
        BoosterManager.Ins.isUsingBooster = false;
    }

}

public class BoosterHint : Booster
{
    public override void Use()
    {
        base.Use();
        DataManager.Ins.playerData.boosterHintAmount -= 1;
        UIManager.Ins.GetUI<GamePlay>().Refresh();
        CameraManager.Ins.AnimZoomToEatableTile(() =>
        {
            ArrowTile.GetEatableTile().greenCover.OnHint();
        });
        FinishUse();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void FinishUse()
    {
        base.FinishUse();
    }
}

public class BoosterBomb : Booster
{
    public bool isSpawnBomb;

    public override void Use()
    {
        base.Use();
        //CameraManager.Ins.MoveToCenter(true);
        CameraManager.Ins.OnUseBomb();
        ClickManager.isCanClick = false;
        
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(false, () =>
        {
        });
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterBombUI(true);
        isSpawnBomb = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isSpawnBomb) return;
        if (CameraManager.Ins.isDraggingOver1) return;
        if (CameraManager.Ins.isDragWhenIsMoving) return;
        if (CameraManager.Ins.isDoingAnim) return;
        if (LevelManager.Ins.isEndLevel) return;
        if (UIHover.isHoverUI) return;

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, GameManager.Ins.arrowTileMask);
            if (hit.collider != null)
            {
                ArrowTile arrowTile = hit.collider.GetComponent<ArrowTile>();
                if(arrowTile != null && !arrowTile.isMoving)
                {
                    Bomb bomb = PoolManager.Ins.Spawn<Bomb>(PoolType.Bomb);
                    bomb.OnInitt(arrowTile, () =>
                    {
                        FinishUse();
                    });
                    DataManager.Ins.playerData.boosterBombAmount -= 1;
                    UIManager.Ins.GetUI<GamePlay>().ShowBoosterBombUI(false, () =>
                    {
                        UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(true);
                        UIManager.Ins.GetUI<GamePlay>().Refresh();
                    });
                    isSpawnBomb = true;
                }
                else
                {
                    OnCancel();
                }
            }
            else
            {
                OnCancel();
            }
        }
    }

    public override void FinishUse()
    {
        base.FinishUse();
        ClickManager.isCanClick = true;
    }

    public void OnCancel()
    {
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterBombUI(false, () =>
        {
            UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(true);
        });
        FinishUse();
    }
}

public class BoosterMagnet : Booster
{
    public List<ArrowTile> moveArrowTiles = new();

    public override void Use()
    {
        base.Use();
        CameraManager.Ins.MoveToCenter(true);
        CameraManager.Ins.RefreshCamDistance(true, 5f, () =>
        {
        });
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(false, () =>
        {
            UIManager.Ins.GetUI<GamePlay>().ShowBoosterMagnetUI(true);
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void FinishUse()
    {
        base.FinishUse();
    }

    public void OnChooseDirection(DirectionType directionType)
    {
        CameraManager.Ins.OnChooseMagnetDirection(directionType, () =>
        {
            InitMoveArrowTiles(directionType);
            Magnet();
        });
        DataManager.Ins.playerData.boosterMagnetAmount -= 1;
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterMagnetUI(false, () =>
        {
            UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(true);
            UIManager.Ins.GetUI<GamePlay>().Refresh();
        });
    }

    public void OnCalcel()
    {
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterMagnetUI(false);
        UIManager.Ins.GetUI<GamePlay>().ShowBoosterButtons(true);
        FinishUse();
    }

    public void InitMoveArrowTiles(DirectionType directionType)
    {
        moveArrowTiles.Clear();
        moveArrowTiles = LevelManager.Ins.currentLevel.tiles
            .Where(x => x.gameObject.activeInHierarchy && !x.isMoving && x.directionType == directionType)
            .ToList();

        if (directionType == DirectionType.Up)
        {
            moveArrowTiles = moveArrowTiles.OrderByDescending(tile => tile.transform.position.y).ToList();
        }
        else if (directionType == DirectionType.Down)
        {
            moveArrowTiles = moveArrowTiles.OrderBy(tile => tile.transform.position.y).ToList();
        }
        else if (directionType == DirectionType.Left)
        {
            moveArrowTiles = moveArrowTiles.OrderBy(tile => tile.transform.position.x).ToList();
        }
        else if (directionType == DirectionType.Right)
        {
            moveArrowTiles = moveArrowTiles.OrderByDescending(tile => tile.transform.position.x).ToList();
        }
    }

    public void Magnet()
    {
        BoosterManager.Ins.StartCoroutine(IEMagnet());
        IEnumerator IEMagnet()
        {
            foreach(var tile in  moveArrowTiles)
            {
                tile.OnMoveByMagnet();
                yield return new WaitForSeconds(0.06f);
            }
            FinishUse();
            yield return null;
        }
    }
}
