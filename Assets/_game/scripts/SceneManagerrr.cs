using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerrr : Singleton<SceneManagerrr>
{
    public bool isLoadingNewScene;
    public SceneType currentSceneType;
    public SceneTransition sceneTransition;


    public void ChangeScene(SceneType sceneType, Action OnComplete, bool isFadeOut=true, bool isFadeIn=true)
    {
        StartCoroutine(IEChangeScene(sceneType, OnComplete, isFadeOut, isFadeIn));
    }

    IEnumerator IEChangeScene(SceneType sceneType, Action OnComplete, bool isFadeOut, bool isFadeIn)
    {
        isLoadingNewScene = true;
        sceneTransition.blockUI.SetActive(true);
        // Fade Out
        if (isFadeOut)
        {
            bool isDoneFadeOut = false;
            sceneTransition.gameObject.SetActive(true);
            sceneTransition.FadeOut(() =>
            {
                isDoneFadeOut = true;
            });
            yield return new WaitUntil(() => isDoneFadeOut);
        }
        else
        {
            sceneTransition.gameObject.SetActive(false);
        }

        PoolManager.Ins.DespawnAll();
        DOTween.KillAll();
        UIManager.Ins.CloseAll();

        // Load Scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneTypeToString(sceneType));
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName(SceneTypeToString(sceneType));
        yield return new WaitUntil(() => scene.IsValid() && scene.isLoaded);
        currentSceneType = sceneType;


        OnComplete?.Invoke();

        if (isFadeOut)
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Fade In
        if (isFadeIn)
        {
            bool isDoneFadeIn = false;
            sceneTransition.gameObject.SetActive(true);
            sceneTransition.FadeIn(() =>
            {
                isDoneFadeIn = true;
            });
            yield return new WaitUntil(() => isDoneFadeIn);
        }
        else
        {
            sceneTransition.gameObject.SetActive(false);
        }

        sceneTransition.blockUI.SetActive(false);
        sceneTransition.gameObject.SetActive(false);
        isLoadingNewScene = false;

        yield return null;
    }

    private string SceneTypeToString(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.Loading:
                return "Loading";
            case SceneType.Game:
                return "Game";
            default:
                return "";
        }
    }

    [Button]
    public void Test()
    {
        ChangeScene(SceneType.Game, null, true, true);
    }

    public void UnloadScene(SceneType sceneType)
    {
        SceneManager.UnloadSceneAsync(SceneTypeToString(sceneType));
    }
}

public enum SceneType
{
    Loading, Game
}
