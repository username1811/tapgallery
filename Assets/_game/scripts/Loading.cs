using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image fillImage;
    public AnimationCurve loadCurve;
    public float duration;
    private AsyncOperation asyncOperation;

    private void Start()
    {
        StartLoading();
    }


    public void StartLoading()
    {
        // Animate the loading bar
        fillImage.DOFillAmount(1f, duration).SetEase(loadCurve).OnComplete(() =>
        {
            StartCoroutine(IECompleteLoading());
        });

        // Load the game scene additively after 1.2 seconds
        asyncOperation = SceneManager.LoadSceneAsync((int)SceneType.Game, LoadSceneMode.Additive);
    }

    public IEnumerator IECompleteLoading()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("complete loading");
        yield return new WaitUntil(() => GameManager.Ins != null && GameManager.Ins.isInited);
        Debug.Log("GameManager inited");
        yield return new WaitUntil(() => asyncOperation.isDone);
        Debug.Log("load scene async isDone");
        WorldTimeAPI.Ins.isFetched = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneType.Game));
        SceneManagerrr.Ins.UnloadScene(SceneType.Loading);
        if (DataManager.Ins.playerData.isPassedTutLevel)
        {
            UIManager.Ins.OpenUI<Home>();
        }
        else
        {
            LevelManager.Ins.LoadTut(0);
            UIManager.Ins.OpenUI<GamePlay>();
            UIManager.Ins.GetUI<GamePlay>().OnLoadLevel();
        }
    }
}