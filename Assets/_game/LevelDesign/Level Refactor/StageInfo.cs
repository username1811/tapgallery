using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "stage_", menuName = "Scriptable Objects/Level/Stage Info")]
public class StageInfo : ScriptableObject
{
    public Sprite stageSprite;
    public int duration = 300;
    [ListDrawerSettings(NumberOfItemsPerPage = 10)]
    public List<LayerInfo> layers;

    public int GetNutAmount()
    {
        int amount = 0;
        foreach (var layer in layers)
        {
            amount += layer.nuts.Count;
        }
        return amount;
    }

    public float2 GetSize()
    {
        float2 min = layers[0].GetMinCoordinates();
        float2 max = layers[0].GetMaxCoordinates();
        foreach (var layer in layers)
        {
            if (layer.GetMinCoordinates().x < min.x) min.x = layer.GetMinCoordinates().x;
            if (layer.GetMinCoordinates().y < min.y) min.y = layer.GetMinCoordinates().y;
            if (layer.GetMaxCoordinates().x > max.x) max.x = layer.GetMaxCoordinates().x;
            if (layer.GetMaxCoordinates().y > max.y) max.y = layer.GetMaxCoordinates().y;
        }
        return max - min + new float2(1f, 1f);
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void ModifyNutPosition(float2 modifier)
    {
        foreach (var layer in layers)
        {
            for (int i = 0; i < layer.coordinates.Count; i++)
            {
                layer.coordinates[i] += modifier;
            }
        }
    }

    [Button]
    public void AddCenterRainbow()
    {
        layers[0].AddCenterRainbow();
        //EditorUtility.SetDirty(this);
        //AssetDatabase.Refresh();
    }

    [Button]
    public void CheckStageValid()
    {
        LayerInfo layer = layers[0];

        Dictionary<int, int> amountByBoltId = new Dictionary<int, int>();

        for (int i = 0; i < layer.bolts.Count; i++)
        {
            if (layer.bolts[i] < 0) continue;

            if (amountByBoltId.ContainsKey(layer.bolts[i]))
            {
                amountByBoltId[layer.bolts[i]]++;
            }
            else
            {
                amountByBoltId.Add(layer.bolts[i], 1);
            }
        }

        Dictionary<int, int> amountByNutId = new Dictionary<int, int>();

        for (int i = 0; i < layer.nuts.Count; i++)
        {
            if (layer.nuts[i] < 0) continue;

            if (amountByNutId.ContainsKey(layer.nuts[i]))
            {
                amountByNutId[layer.nuts[i]]++;
            }
            else
            {
                amountByNutId.Add(layer.nuts[i], 1);
            }
        }

        var sortedBolt = amountByBoltId.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        var sortedNut = amountByNutId.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        Dictionary<int, string> logById = new Dictionary<int, string>();

        foreach (var pair in amountByNutId)
        {
            logById.Add(pair.Key, $"nut: {pair.Value}");
        }

        foreach (var pair in amountByBoltId)
        {
            logById[pair.Key] += $" - bolt: {pair.Value}";
        }

        foreach (var pair in logById)
        {
            Debug.Log($"{pair.Key} - {pair.Value}");
        }

    }
}
