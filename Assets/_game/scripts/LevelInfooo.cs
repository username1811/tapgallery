using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Infooo")]
public class LevelInfooo : ScriptableObject
{
    public Texture2D texture2d;
    public List<PixelData> pixelDatas = new List<PixelData>();
    public int heartAmount;
    public bool isTut;
    public bool isTheme;

    public Sprite GetSprite()
    {
        return Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }
}

[Serializable]
public class PixelData
{
    public Color color;
    public Vector2 coordinate;
    public int sortingOrder;
    public DirectionType directionType;

    public PixelData(Color color, Vector2 coordinate, int sortingOrder, DirectionType directionType)
    {
        this.color = color;
        this.coordinate = coordinate;
        this.sortingOrder = sortingOrder;
        this.directionType = directionType;
    }
}