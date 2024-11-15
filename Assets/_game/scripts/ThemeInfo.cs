using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Theme Info")]
public class ThemeInfo : ScriptableObject
{
    public ThemeType themeType;
    public Sprite sprite;
    public List<LevelInfooo> levelInfooos = new List<LevelInfooo>();
}

public enum ThemeType
{
    Animals, Christmas, Pokemon, Space, SuperHero, 
}