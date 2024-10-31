#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitcher
{
    private const string ScenesFolderPath = "Assets/_Game/scenes/";

    [MenuItem("Scenes/Loading #1", priority = 1)]
    public static void OpenLoad()
    {
        OpenScene("Loading");
    }

    [MenuItem("Scenes/Loading #1", true)]
    public static bool OpenLoadValidate()
    {
        return OpenSceneValidate("Loading");
    }

    [MenuItem("Scenes/Home #2", priority = 2)]
    public static void OpenHome()
    {
        OpenScene("Home");
    }

    [MenuItem("Scenes/Home #2", true)]
    public static bool OpenHomeValidate()
    {
        return OpenSceneValidate("Home");
    }

    [MenuItem("Scenes/Game #3", priority = 3)]
    public static void OpenGame()
    {
        OpenScene("Game");
    }

    [MenuItem("Scenes/Game #3", true)]
    public static bool OpenGameValidate()
    {
        return OpenSceneValidate("Game");
    }

    [MenuItem("Scenes/LevelDesign #4", priority = 4)]
    public static void OpenLevelDesign()
    {
        OpenScene("LevelDesign");
    }

    [MenuItem("Scenes/LevelDesign #4", true)]
    public static bool OnLevelDesignValidate()
    {
        return OpenSceneValidate("LevelDesign");
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(ScenesFolderPath + sceneName + ".unity");
        }
    }

    private static bool OpenSceneValidate(string sceneName)
    {
        return System.IO.File.Exists(ScenesFolderPath + sceneName + ".unity");
    }
}

#endif