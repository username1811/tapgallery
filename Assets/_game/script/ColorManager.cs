using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    public List<Color> colors = new();

    public Color GetRandomColor()
    {
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }
}