#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static Unity.Mathematics.math;

public class TextureToStageWindow : OdinEditorWindow
{
    [MenuItem("Level Design/Texture To Stage")]
    private static void OpenWindow()
    {
        GetWindow<TextureToStageWindow>().Show();
    }

    [FolderPath(RequireExistingPath = true)]
    public string textureFolderPath;
    [FolderPath(RequireExistingPath = true)]
    public string stageFolderPath;

    [Button]
    public void TexturesToStages()
    {
        string[] guids = AssetDatabase.FindAssets("t:texture2D", new string[] { textureFolderPath });

        foreach (string guid in guids)
        {
            string texturePath = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

            if (texture == null)
            {
                Debug.LogError("Cannot load texture");
                continue;
            }

            TextureToStage(texture);
        }

        AssetDatabase.Refresh();
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void TextureToStage(Texture2D texture, bool isRefresh = false)
    {
        string texturePath = AssetDatabase.GetAssetPath(texture);
        string fileName = Path.GetFileName(texturePath).Split('.')[0];
        string folderPath = Path.GetDirectoryName(texturePath);
        string folderName = Path.GetFileName(folderPath);
        string stageName = $"stage_{folderName}_{fileName}.asset";
        string stagePath = Path.Combine(stageFolderPath, stageName);

        StageInfo stageInfo = AssetDatabase.LoadAssetAtPath<StageInfo>(stagePath);

        if (stageInfo == null)
        {
            stageInfo = CreateInstance<StageInfo>();
            AssetDatabase.CreateAsset(stageInfo, stagePath);
            AssetDatabase.SaveAssets();
        }

        int2 size = new(texture.width, texture.height);
        float2 offset = (float2)(size - 1) * 0.5f;
        Color[] pixels = texture.GetPixels();
        List<Color> colors = new();
        List<int> nuts = new();
        List<float2> coordinates = new();
        Dictionary<int, int> amountById = new();

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Color pixel = pixels[x + size.x * y];
                if (pixel.a == 0) continue;
                if (!colors.Contains(pixel)) colors.Add(pixel);
                int id = colors.IndexOf(pixel);
                nuts.Add(id);
                if (!amountById.ContainsKey(id)) amountById.Add(id, 1);
                else amountById[id]++;
                float2 c = new float2(x, y) - offset;
                coordinates.Add(c);
            }
        }

        int mostCenteredIndex = GetMostCenteredIndex(coordinates);

        List<int> bolts = new(nuts);
        var p = amountById.OrderByDescending(pair => pair.Value).First();
        bolts.Remove(p.Key);
        bolts.Shuffle();
        bolts.Insert(mostCenteredIndex, -1);

        LayerInfo layerInfo = new();
        layerInfo.colors = colors;
        layerInfo.nuts = nuts;
        layerInfo.bolts = bolts;
        layerInfo.coordinates = coordinates;

        stageInfo.layers = new()
        {
            layerInfo
        };

        stageInfo.AddCenterRainbow();

        EditorUtility.SetDirty(stageInfo);

        if (isRefresh)
        {
            AssetDatabase.Refresh();
        }
    }

    private int GetMostCenteredIndex(List<float2> coordinates)
    {
        int result = 0;
        float minDistance = distancesq(coordinates[0], new float2(0f, 0f));

        for (int i = 1; i < coordinates.Count; i++)
        {
            float distance = distancesq(coordinates[i], new float2(0f, 0f));

            if (distance < minDistance)
            {
                result = i;
                minDistance = distance;
            }
        }

        return result;
    }
}
#endif