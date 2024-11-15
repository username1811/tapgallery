using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeLevelsPopUp : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI title;
    public List<LevelUI> levelUIs = new List<LevelUI>();
    public GridLayoutGroup grid;
    public ThemeInfo themeInfo;


    public void OnInitt(ThemeInfo themeInfo)
    {
        this.themeInfo = themeInfo; 
        this.img.sprite = themeInfo.sprite;
        this.title.text = themeInfo.themeType.ToString();
        InitLevelUis();
    }

    public void InitLevelUis()
    {
        this.levelUIs.Clear();
        PoolManager.Ins.GetPool(PoolType.LevelUI).ReturnAll();
        foreach(var levelInfo in themeInfo.levelInfooos)
        {
            LevelUI levelUI = PoolManager.Ins.Spawn<LevelUI>(PoolType.LevelUI);
            levelUI.OnInitt(levelInfo);
            levelUI.transform.SetParent(grid.transform);
            levelUI.transform.localScale = Vector3.one;
            levelUIs.Add(levelUI);
        }  
    }

    public void ButtonClose()
    {
        UIManager.Ins.GetUI<ThemeCollection>().ShowThemeLevelsPopUp(false);
    }

}
