using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    public RectTransform minimapRectTF;



    private void Start()
    {
        minimapRectTF.AddComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<CheatPanel>();
        });
    }

    public override void Open()
    {
        base.Open();
        MinimapManager.Ins.originalMinimapPos = minimapRectTF.position;
    }

    public void OnLoadLevel()
    {

    }

    public void ButtonBack()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.OpenUI<Home>();
        });
    }
}
