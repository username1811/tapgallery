using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Stage Infooo")]
public class StageInfooo : ScriptableObject
{
    public List<PixelData> pixelDatas = new List<PixelData>();
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