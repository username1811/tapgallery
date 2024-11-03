using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterManager : Singleton<BoosterManager>
{
    public Booster currentBooster;
    public bool isUsingBooster => currentBooster != null;

    [SerializeReference]
    public List<Booster> boosters = new List<Booster>();    

    public void OnLoadLevel()
    {
        currentBooster = null;
    }

    private void Update()
    {
        currentBooster?.OnUpdate();
    }

    [Button]
    public void UseBooster(BoosterType boosterType)
    {
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
        Debug.Log("booster use " + this.boosterType);
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void FinishUse()
    {
        Debug.Log("booster finish use " + this.boosterType);
        BoosterManager.Ins.currentBooster = null;
    }

}

public class BoosterHint : Booster
{
    public override void Use()
    {
        base.Use();
        CameraManager.Ins.AnimZoomToEatableTile(() =>
        {
            ArrowTile.GetEatableTile().greenCover.TurnGreen();
        });
        FinishUse();
        Debug.Log("hint use");
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
        ClickManager.isCanClick = false;
        isSpawnBomb = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isSpawnBomb) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, GameManager.Ins.arrowTileMask);
            if (hit.collider != null)
            {
                ArrowTile arrowTile = hit.collider.GetComponent<ArrowTile>();
                Bomb bomb = PoolManager.Ins.Spawn<Bomb>(PoolType.Bomb);
                bomb.OnInitt(arrowTile, () =>
                {
                    FinishUse();
                });
                isSpawnBomb = true;
            }
        }
    }

    public override void FinishUse()
    {
        base.FinishUse();
        ClickManager.isCanClick = true;
    }
}

public class BoosterMagnet : Booster
{
    public override void Use()
    {
        base.Use();
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
