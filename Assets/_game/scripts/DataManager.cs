﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[Serializable]
public class DataManager : Singleton<DataManager>
{
    public bool isLoaded = false;
    public PlayerData playerData;
    public const string PLAYER_DATA = "PLAYER_DATA";


    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }



    [Button]
    public void LoadData(bool isShowAOA = false)
    {
        Debug.Log("START LOAD DATA");
        string d = PlayerPrefs.GetString(PLAYER_DATA, "");
        if (d != "")
        {
            playerData = JsonUtility.FromJson<PlayerData>(d);
        }
        else
        {
            playerData = new PlayerData();
        }

        if (DataManager.Ins.playerData.firstOpenDateTimeStr == null || DataManager.Ins.playerData.firstOpenDateTimeStr == "")
        {
            DataManager.Ins.playerData.firstOpenDateTimeStr = WorldTimeAPI.Ins.GetCurrentDateTime().ToString();
        }

        isLoaded = true;
    }

    public void SaveData()
    {
        if (!isLoaded) return;
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
        PlayerPrefs.Save();
        Debug.Log("SAVE DATA");
    }

    public IEnumerator CheckSession()
    {
        yield return new WaitForSecondsRealtime(10f);
        TimeSpan timeSpan = WorldTimeAPI.Ins.GetCurrentDateTime() - DateTime.Parse(playerData.lastExitTime);
        if (timeSpan.TotalMinutes > 30 && playerData.currentSession < 5)
        {
            // session += 1
            playerData.currentSession += 1;
            //firebase
            //FirebaseManager.Ins.SendEvent("session_start_" + playerData.currentSession);
            //af
            //AFSendEvent.SendEvent("session_start_" + playerData.currentSession.ToString());
        }
    }
}

[Serializable]
public class BuyedItemId
{
    public List<int> list;
    public BuyedItemId()
    {
        list = new List<int>();
    }
}

[System.Serializable]
public class PlayerData
{
    [Header("------Chỉ số Game--------")]
    public double timeLastOpen;//days
    public double timeLastOpenHour;
    public int daysPlayed;
    public string firstOpenDateTimeStr;

    [Header("--------- Game Params ---------")]
    [Header("resource:")]
    public int gold;
    [Header("level index:")]
    public int currentTutLevelIndex;
    public int currentLevelIndex;
    public List<string> passedLevelNames = new();
    public List<ThemeLevelsData> themeLevelsDatas = new();   
    public bool isPassedTutLevel;
    [Header("booster:")]
    public int boosterHintAmount;
    public int boosterBombAmount;
    public int boosterMagnetAmount;
    public List<BoosterType> unlockedBoosterTypes;
    [Header("spin:")]
    public bool isCanSpin;
    public int levelPassToSpin;
    public bool isFirstSpined;
    public bool isIgnoreSpin;
    [Header("lives:")]
    public List<string> liveTimes;
    public int currentLiveCount;
    public string infinityLiveExpire;
    [Header("IAP:")]
    public bool isPurchasedRemoveAds;
    public bool isPurchasedStarterPack;
    public List<string> purchasedIDs;
    [Header("AnhTungOrder:")]
    public int firstTryWinCount;
    [Header("aflyer:")]
    public int interAdCount;
    public int rewardAdCount;
    [Header("new daily reward:")]
    public int currentRewardFrame;
    public int freeClaimCount;
    public string freeClaimExpire;
    public string lastClaimDailyReward2;
    [Header("starter pack : ")]
    public string cappingStarterPackExpire;
    [SerializeField]
    public bool isMusicOn;
    public bool isSoundOn;
    public bool isVibrateOn;

    #region firebase
    public bool isFirstOpen = false;
    public string lastExitTime;
    public int currentSession;

    public int maxCheckPointStartIndex;
    public int maxCheckPointEndIndex;
    #endregion


    public PlayerData()
    {
        timeLastOpen = 9999;
        timeLastOpenHour = 9999;
        daysPlayed = 0;

        gold = 1000;
        currentTutLevelIndex = 0;
        currentLevelIndex = 0;
        passedLevelNames = new();
        isCanSpin = false;
        isFirstSpined = false;
        isIgnoreSpin = false;
        liveTimes = new List<string>();
        currentLiveCount = 5;
        purchasedIDs = new List<string>();
        firstTryWinCount = 0;
        lastClaimDailyReward2 = new DateTime(1999, 1, 1).ToString();
        cappingStarterPackExpire = "";
        unlockedBoosterTypes = new();
        themeLevelsDatas = new();    

        isMusicOn = true;
        isSoundOn = true;
        isVibrateOn = true;

        //fire base
        isFirstOpen = true;
        lastExitTime = new DateTime(1970, 1, 1).ToString();
        currentSession = 0;
        maxCheckPointStartIndex = -1;
        maxCheckPointEndIndex = -1;
    }



}

[Serializable]
public class ThemeLevelsData
{
    public ThemeType themeType;
    public List<int> passedIndexs = new();  

    public ThemeLevelsData(ThemeType themeType)
    {
        this.themeType = themeType;
        passedIndexs = new();
    }
}