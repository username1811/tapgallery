using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeUI : MonoBehaviour
{
    public ThemeType themeType;
    public Image img;
    public TextMeshProUGUI title;

    public void OnInit(ThemeInfo themeInfo)
    {
        this.themeType = themeInfo.themeType;
        this.img.sprite = themeInfo.sprite;
        this.title.text = themeInfo.themeType.ToString();
    }

    public void OnClick()
    {
        ThemeManager.Ins.OnClickThemeUI(themeType);
    }
}
