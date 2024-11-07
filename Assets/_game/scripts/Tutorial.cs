using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Searchable]
public class Tutorial : Singleton<Tutorial>
{
    public List<Vector2> tilePoses = new();
    public Vector2 currentTilePos;
    public int nextCount;
    public bool isFinish;

    public void OnInitt(bool isTut)
    {
        if (!isTut) return;
        currentTilePos = tilePoses[0];
        nextCount = tilePoses.Count - 1;
        ShowHand(true);
        isFinish = false;
    }

    public void Next()
    {
        if (nextCount <= 0)
        {
            isFinish = true;
            ShowHand(false);
            return;
        }
        nextCount -= 1;
        currentTilePos = tilePoses[tilePoses.IndexOf(currentTilePos) + 1];
        ShowHand(true);
    }

    public bool IsCurrentTile(ArrowTile arrowTile)
    {
        return Vector2.Distance(arrowTile.transform.position, currentTilePos) < 0.05f;
    }

    public void ShowHand(bool isShow)
    {
        UIManager.Ins.GetUI<GamePlay>().hand.gameObject.SetActive(isShow);
        UIManager.Ins.GetUI<GamePlay>().hand.Anim();
        UIManager.Ins.GetUI<GamePlay>().hand.GetComponent<RectTransform>().position = CameraManager.Ins.cam.WorldToScreenPoint(currentTilePos + new Vector2(0.25f, -0.25f));
    }

}
