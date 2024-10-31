using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Config:")]
    public float timeScale = 1f;

    [Header("LayerMasks:")]
    public LayerMask arrowTileMask;

    /*[Header("Spin:")]
    public int levelPassToFirstSpin;
    public int levelPassToSpin;

    [Header("Revive:")]
    public int moveLeftBonus;

    [Header("Hard Level Offer Level Index:")]
    public int hardLevelOfferLevelIndex;

    [Header("FLow:")]
    public int homeIndex;*/

    public bool isInited = false;


    private void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(LoadAllData());
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public IEnumerator LoadAllData()
    {
        yield return new WaitUntil(() => (
            GameManager.Ins != null
            && DataManager.Ins != null
            && LevelManager.Ins != null
            && SceneManagerrr.Ins != null
            && UIManager.Ins != null
            && PoolManager.Ins != null
            && WorldTimeAPI.Ins != null
        ));
        yield return null;
        UIManager.Ins.InitWidthHeight();
        yield return null;
        /*AppsFlyerAdRevenue.start();
        UMPManager.Ins.CheckUMP();*/
        yield return null;
        WorldTimeAPI.Ins.OnInit();
        yield return null;
        DataManager.Ins.LoadData();
        yield return null;
        /*SoundManager.Ins?.OnInit();
        yield return null;
        SoundManager.Ins?.OnInit();
        yield return null;*/
        isInited = true;
        yield return null;
    }
}
