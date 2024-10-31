using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using Sirenix.OdinInspector;
using DG.Tweening;

public class WorldTimeAPI : Singleton<WorldTimeAPI>
{
    //json container
    struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //..
    }

    const string API_URL = "https://worldtimeapi.org/api/ip";
    //const string API_URL = "https://timeapi.io/api/Time/current/zone?timeZone=UTC";

    [HideInInspector] public bool IsTimeLoaded = false;

    private DateTime _GetTimeInternetDateTime = DateTime.Now;

    public bool isFetched;
    public float timeOut;
    public float startGameToFinishGetDuration;

    public float loopCapping => timeOut + 2f; // set thời gian lặp lại lớn hơn timeout
    public float loopCappingTimer;

    private void Start()
    {
        _GetTimeInternetDateTime = DateTime.Now;
        isFetched = false;
        loopCappingTimer = loopCapping;
    }

    private void Update()
    {
        if (IsTimeLoaded) return;
        if (loopCappingTimer > 0)
        {
            loopCappingTimer -= Time.deltaTime;
        }
        else
        {
            loopCappingTimer = loopCapping;
            StartGetCurrentDateTime();
        }
    }

    public void OnInit()
    {
        StartGetCurrentDateTime();
    }

    public void StartGetCurrentDateTime()
    {
        StartCoroutine(IEGetRealDateTimeFromAPI());
    }

    public DateTime GetCurrentDateTime()
    {
        //here we don't need to get the datetime from the server again
        // just add elapsed time since the game start to _currentDateTime
        if (CheatInput.Ins.isCheatDateTime)
        {
            return DateTime.Now;
        }
        return _GetTimeInternetDateTime.AddSeconds(Time.realtimeSinceStartup - startGameToFinishGetDuration);
    }

    IEnumerator IEGetRealDateTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        Debug.Log("getting real datetime...");
        DOVirtual.DelayedCall(timeOut, () =>
        {
            isFetched = true;
        });
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            //error
            Debug.Log("Error: " + webRequest.error);

        }
        else
        {
            //success
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
            //timeData.datetime value is : 2020-08-14T15:54:04+01:00

            _GetTimeInternetDateTime = ParseDateTime(timeData.datetime);
            Debug.Log("Time from internet now is: " + _GetTimeInternetDateTime.ToString());
            IsTimeLoaded = true;
            startGameToFinishGetDuration = Time.realtimeSinceStartup;
        }

        isFetched = true;
    }
    //datetime format => 2020-08-14T15:54:04+01:00
    DateTime ParseDateTime(string datetime)
    {
        try
        {
            //match 0000-00-00
            string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;

            //match 00:00:00
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

            return DateTime.Parse(string.Format("{0} {1}", date, time));
        }
        catch (FormatException ex)
        {
            Debug.LogError("Error parsing datetime: " + ex.Message);
            return DateTime.Now; // Trả về giá trị mặc định nếu lỗi
        }
    }



    [Button]
    public void DebugDateTime()
    {
        Debug.Log(GetCurrentDateTime().ToString());
    }

    public int GetDurationFromFirstOpen()
    {
        return (int)GetCurrentDateTime().Subtract(DateTime.Parse(DataManager.Ins.playerData.firstOpenDateTimeStr)).TotalSeconds;
    }

    public DayOfWeek GetDayOfWeek()
    {
        return GetCurrentDateTime().DayOfWeek;
    }
}


/* API (json)
{
	"abbreviation" : "+01",
	"client_ip"    : "190.107.125.48",
	"datetime"     : "2020-08-14T15:544:04+01:00",
	"dst"          : false,
	"dst_from"     : null,
	"dst_offset"   : 0,
	"dst_until"    : null,
	"raw_offset"   : 3600,
	"timezone"     : "Asia/Brunei",
	"unixtime"     : 1595601262,
	"utc_datetime" : "2020-08-14T15:54:04+00:00",
	"utc_offset"   : "+01:00"
}

We only need "datetime" property.
*/