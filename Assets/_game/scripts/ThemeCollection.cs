using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeCollection : UICanvas
{
    public VerticalLayoutGroup VerticalLayoutGroup;
    public RectTransform contentRectTF;
    public bool isInited;
    public List<ThemeUI> themeUIs = new();
    public ThemeLevelsPopUp themeLevelsPopUp;

    public override void Open()
    {
        base.Open();
        Initt();
        RefreshThemeUIs();
        RefreshContentHeight();
        ShowThemeLevelsPopUp(false);
    }

    public void Initt()
    {
        if (isInited) return;
        isInited = true;
    }

    public void RefreshThemeUIs()
    {
        themeUIs.Clear();
        PoolManager.Ins.GetPool(PoolType.ThemeUI).ReturnAll();
        foreach (var themeInfo in ThemeManager.Ins.themeInfos)
        {
            ThemeUI themeUI = PoolManager.Ins.Spawn<ThemeUI>(PoolType.ThemeUI);
            themeUI.OnInit(themeInfo);
            themeUI.transform.SetParent(VerticalLayoutGroup.transform);
            themeUI.transform.localScale = Vector3.one;
            themeUIs.Add(themeUI);
        }
    }

    public void RefreshContentHeight()
    {
        contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, 400f * themeUIs.Count + VerticalLayoutGroup.spacing * (themeUIs.Count-1));
    }

    public void ShowThemeLevelsPopUp(bool isShow)
    {
        themeLevelsPopUp.gameObject.SetActive(isShow);
    }

    public void ButtonBack()
    {
        UIManager.Ins.CloseUI<ThemeCollection>();
    }
}
