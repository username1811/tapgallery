#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.Mathematics.math;

public class ConvertTextureToLevelData : OdinEditorWindow
{
    [MenuItem("Level Design/ConvertTextureToData")]
    private static void OpenWindow()
    {
        GetWindow<ConvertTextureToLevelData>().Show();
    }

    [FolderPath(RequireExistingPath = true)]
    public string textureFolderPath;
    [FolderPath(RequireExistingPath = true)]
    public string levelInfoFolderPath;
    [FolderPath(RequireExistingPath = true)]
    public string stageInfoFolderPath;

    [Button]
    public void Convert()
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

            TextureToLevelData(texture);
        }

        AssetDatabase.Refresh();
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void TextureToLevelData(Texture2D texture, bool isRefresh = false)
    {
        string texturePath = AssetDatabase.GetAssetPath(texture);
        string fileName = Path.GetFileName(texturePath).Split('.')[0];
        string folderPath = Path.GetDirectoryName(texturePath);
        string folderName = Path.GetFileName(folderPath);
        string stageName = $"{folderName}_{fileName}.asset";
        string levelName = stageName;
        string stagePath = Path.Combine(stageInfoFolderPath, stageName);
        string levelPath = Path.Combine(levelInfoFolderPath, levelName);

        LevelInfooo levelInfooo = AssetDatabase.LoadAssetAtPath<LevelInfooo>(levelPath);
        StageInfooo stageInfooo = AssetDatabase.LoadAssetAtPath<StageInfooo>(stagePath);


        if (levelInfooo == null)
        {
            levelInfooo = CreateInstance<LevelInfooo>();
            AssetDatabase.CreateAsset(levelInfooo, levelPath);
            stageInfooo = CreateInstance<StageInfooo>();
            AssetDatabase.CreateAsset(stageInfooo, stagePath);
            AssetDatabase.SaveAssets();
        }

        stageInfooo.texture2d = texture;
        stageInfooo.pixelDatas = new();

        int2 size = new(texture.width, texture.height);
        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Color pixel = pixels[x + size.x * y];
                if (pixel.a == 0) continue;
                float2 c = new float2(x, y);
                PixelData pixelData = new PixelData(pixel, c, size.y - y, 0);
                stageInfooo.pixelDatas.Add(pixelData);
            }
        }

        levelInfooo.stages = new();
        levelInfooo.stages.Add(stageInfooo);

        EditorUtility.SetDirty(levelInfooo);
        EditorUtility.SetDirty(stageInfooo);
        AssetDatabase.Refresh();

        if (isRefresh)
        {
            AssetDatabase.Refresh();
        }
    }
}
#endif