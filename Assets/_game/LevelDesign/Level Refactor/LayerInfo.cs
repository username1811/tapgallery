using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class LayerInfo
{
    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<Color> colors;
    public List<int> nuts, bolts;
    public List<float2> coordinates;

    public LayerInfo()
    {
        colors = new();
        nuts = new();
        bolts = new();
        coordinates = new();
    }

    public float2 GetSize()
    {
        float2 max = coordinates[0];
        float2 min = coordinates[0];
        foreach (var c in coordinates)
        {
            if (c.x > max.x) max.x = c.x;
            if (c.y > max.y) max.y = c.y;
            if (c.x < min.x) min.x = c.x;
            if (c.y < min.y) min.y = c.y;
        }
        return max - min + new float2(1f, 1f);
    }

    public float2 GetMaxCoordinates()
    {
        float2 max = coordinates[0];
        foreach (var c in coordinates)
        {
            if (c.x > max.x) max.x = c.x;
            if (c.y > max.y) max.y = c.y;
        }
        return max;
    }

    public float2 GetMinCoordinates()
    {
        float2 min = coordinates[0];
        foreach (var c in coordinates)
        {
            if (c.x < min.x) min.x = c.x;
            if (c.y < min.y) min.y = c.y;
        }
        return min;
    }

    [Button]
    public void MostAmountColor()
    {
        Dictionary<int, int> amountById = new();

        foreach (var nutId in nuts)
        {
            if (!amountById.ContainsKey(nutId))
            {
                amountById[nutId] = 1;
            }
            else
            {
                amountById[nutId]++;
            }
        }

        var p = amountById.OrderByDescending(pair => pair.Value).First();

        Debug.Log(p);
    }

    [Button]
    public void AddCenterRainbow()
    {
        int centerIndex = 0;

        for (int i = 0; i < bolts.Count; i++)
        {
            if (bolts[i] == -1)
            {
                centerIndex = i;
                break;
            }
        }

        bolts = new List<int>(nuts);
        bolts.Shuffle();

        int nextToCenterIndex = centerIndex + 1;

        for (int i = 0; i < bolts.Count; i++)
        {
            if (bolts[i] == nuts[nextToCenterIndex])
            {
                bolts[i] = bolts[nextToCenterIndex];
                break;
            }
        }

        bolts[centerIndex] = -1;
        bolts[nextToCenterIndex] = -1;
        //nuts[nextToCenterIndex] = NutBase.RainbowId;
    }
}
