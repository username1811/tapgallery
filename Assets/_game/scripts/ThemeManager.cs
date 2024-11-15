using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager> 
{
    public List<ThemeInfo> themeInfos = new List<ThemeInfo>();
    public ThemeInfo currentThemeInfo;

    public void OnClickThemeUI(ThemeType themeType)
    {
        currentThemeInfo = themeInfos.FirstOrDefault(x => x.themeType == themeType);
        UIManager.Ins.GetUI<ThemeCollection>().ShowThemeLevelsPopUp(true);
        UIManager.Ins.GetUI<ThemeCollection>().themeLevelsPopUp.OnInitt(currentThemeInfo);    
    }

}
