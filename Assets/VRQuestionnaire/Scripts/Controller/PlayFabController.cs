using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;
    //public int numberOfLogins;
    private string myID;
    public string DEMO;
    public string JSON1;
    public string JSON2;
    public string JSON3;
    public string JSON4;
    public int latestIndex;
    [HideInInspector]
    public List<GoogleSurveyData> googleSurveyData;
    private Dictionary<string,string> resultsData;
    private string playFabName;
    private int numberOfSurveys = 5;

    //Creates a singlton of this class, an empty googlesurveydata, and resultsdata
    private void OnEnable()
    {
        if (PlayFabController.PFC == null)
        {
            PlayFabController.PFC = this;
        }
        else
        {
            if (PlayFabController.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);

        googleSurveyData = new List<GoogleSurveyData>(new GoogleSurveyData[numberOfSurveys]);
        resultsData = new Dictionary<string, string>();
    }

    void OnDisable()
    {
        if(resultsData != null)
        SetUserData(resultsData);
    }

    //Sends resultsdata to the Database
    public void SubmitResults()
    {
        #region SURVEY_RESULTS
        Dictionary<string, string> tempResults = new Dictionary<string, string>();


        foreach (var question in FindObjectsOfType<Question>(true))
        {
            if (question._title != null && question._answer != "")
            {
                Debug.Log("Question " + question._index + " : " + question._title.text + " " + question._answer);
                tempResults.Add("Question " + question._index + " : " + question._title.text, question._answer);
            }
        }

        int i = 0;

        foreach (float time in GameController.instance.TrialTimes)
        {
            tempResults.Add("Time For " +i, time.ToString());
            i++;
        }
       
        string JSONout1 = JsonConvert.SerializeObject(tempResults);
        if (JSONout1 != null)
        {
            if (resultsData.ContainsKey("SURVEYRESULTS"))
            {
                resultsData["SURVEYRESULTS"] = JSONout1;
            }
            else
            {
                resultsData.Add("SURVEYRESULTS", JSONout1);
            }
            Debug.Log("You have successfully submitted your questionnaire results");
        }

        GameController.instance.EndSurvey();
        latestIndex++;
        #endregion SURVEY_RESULTS
        #region EXPERIMENT_DATA_RESULTS
        #endregion EXPERIMENT_DATA_RESULTS
    }

    //Registers this script to a playfab titleid, and then registers the android device to that title ID.
    public void Start()
    {
    if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
    {
        PlayFabSettings.TitleId = "AC209"; 
    }
 
    #if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(),CreateAccount = true};
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
    #endif
    }
    //On registration success, the success function below will get request the JSON file we need for the questionnaire from the Database, set the user ID.
#region -/////////////////////////////////////////////Login//////////////////////////////////////////////////- 
    private void OnLoginAndroidSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged into the Android Device!");
        myID = result.PlayFabId;
        //numberOfLogins++; STATS FUTURE
        GetAccountInfo();
        GetUserData();
    }
    private void OnLoginAndroidFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong logging in!");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    public static string ReturnAndroidID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }
    #endregion -/////////////////////////////////////////////Login//////////////////////////////////////////////////- 
    // 2 main function one to send data to the database, the other to request data from the database
#region -/////////////////////////////////////////////PlayerData(PlayFab)//////////////////////////////////////////////////-
    
    
    
    //Send data to the cloud!
    public void SetUserData(Dictionary<string,string> resultsData)
    {
        if (resultsData != null)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>(resultsData)
            }, SetDataSuccess, OnErrorDataSet);
        }
    }

    public void ExitnSave()
    {
        SetUserData(resultsData);
    }
    void SetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Results has been sent as version number : " + result.DataVersion + resultsData);
    }
    //Get data from the cloud!
    void OnErrorDataGet(PlayFabError error)
    {
      Debug.LogError("Could not retreive data" + error.GenerateErrorReport());
    }

    void OnErrorDataSet(PlayFabError error)
    {
        Debug.LogError("Could not send data" + error.GenerateErrorReport());
    }
    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myID,
            Keys = null
        }, UserDataSuccess, OnErrorDataGet);
    }

    private void UserDataSuccess(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("JSON1") || !result.Data.ContainsKey("JSON2"))
        {
            Debug.Log("There is no key call JSON in database or the data in the database is empty");
        }
        else
        {
            DEMO = result.Data["DEMO"].Value;
            JSON1 = result.Data["JSON1"].Value;
            JSON2 = result.Data["JSON2"].Value;
            JSON3 = result.Data["JSON3"].Value;
            JSON4 = result.Data["JSON4"].Value;
 
            googleSurveyData[0] = JsonConvert.DeserializeObject<GoogleSurveyData>(PlayFabController.PFC.DEMO);
            googleSurveyData[1] = JsonConvert.DeserializeObject<GoogleSurveyData>(PlayFabController.PFC.JSON1);
            googleSurveyData[2] = JsonConvert.DeserializeObject<GoogleSurveyData>(PlayFabController.PFC.JSON2);
            googleSurveyData[3] = JsonConvert.DeserializeObject<GoogleSurveyData>(PlayFabController.PFC.JSON3);
            googleSurveyData[4] = JsonConvert.DeserializeObject<GoogleSurveyData>(PlayFabController.PFC.JSON4);
            latestIndex = 0;
        }
    }

    public void GetAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest()
        {

        }, UserAccountSuccess, OnErrorDataGet);
    }

    private void UserAccountSuccess(GetAccountInfoResult result)
    {
        playFabName = result.AccountInfo.TitleInfo.DisplayName;
        Debug.Log("Device logged in name: " + playFabName);
    }
   
    #endregion -/////////////////////////////////////////////PlayerData(PlayFab)//////////////////////////////////////////////////-
}
























// This is for future use if we require users to log in instead of using automatic login on android ID. Use playerStats, and leaderboard!
#region EmailLoginCommented
//private string userEmail;
//private string userPassword;
//private string userName;
//public GameObject loginPanel;


//public GameObject addLoginPanel;
//public GameObject recoverButton;

//if (PlayerPrefs.HasKey("EMAIL"))
//{
//    userEmail = PlayerPrefs.GetString("EMAIL");
//    userPassword = PlayerPrefs.GetString("PASSWORD");
//    var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
//    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
//}

//public void OnClickLogin()
//{
//    var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
//    PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
//}

//public void OpenAddLogin()
//{
//    addLoginPanel.SetActive(true);
//}

//public void OnClickAddLogin()
//{
//    var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = userName };
//    //PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnLoginAndroidSuccess, OnLoginAndroidFailure);
//}

//private void OnAddLoginSuccess(AddUsernamePasswordResult result)
//{
//    Debug.Log("Congratulations, you made your first successful API call!");
//    PlayerPrefs.SetString("EMAIL", userEmail);
//    PlayerPrefs.SetString("PASSWORD", userPassword);
//    addLoginPanel.SetActive(false);
//}

//private void OnLoginSuccess(LoginResult result)
//{
//    Debug.Log("Congratulations, you made your first successful API call!");
//    PlayerPrefs.SetString("EMAIL", userEmail);
//    PlayerPrefs.SetString("PASSWORD", userPassword);
//    loginPanel.SetActive(false);
//}

//private void OnRegisterSuccess(RegisterPlayFabUserResult result)
//{
//    Debug.Log("Congratulations, you made your first successful API call!");
//    PlayerPrefs.SetString("EMAIL", userEmail);
//    PlayerPrefs.SetString("PASSWORD", userPassword);
//    loginPanel.SetActive(false);
//}


//private void OnLoginFailure(PlayFabError error)
//{
//    var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
//    PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
//}

//private void OnRegisterFailure(PlayFabError error)
//{
//    Debug.LogError(error.GenerateErrorReport());
//}



//public void GetUserEmail(string emailIn)
//{
//    userEmail = emailIn;
//}
//
//public void GetUserPassword(string passwordIn)
//{
//    userPassword = passwordIn;
//}
//
//public void GetUsername(string usernameIn)
//{
//    userName = usernameIn;
//}

#endregion EmailLoginCommented
#region PlayerStats

//public void UpdatePlayerStats()
//{
//    PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
//    {
//        // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
//        Statistics = new List<StatisticUpdate> {
//                new StatisticUpdate { StatisticName = "numberOfLogins", Value = numberOfLogins },
//        }
//    },
//    result => { Debug.Log("User statistics updated"); },
//    error => { Debug.LogError(error.GenerateErrorReport()); });
//}
//
//public void GetPlayerStats()
//{
//    PlayFabClientAPI.GetPlayerStatistics(
//         new GetPlayerStatisticsRequest(),
//         OnGetStats,
//         error => Debug.LogError(error.GenerateErrorReport())
//     );
//}
//
//void OnGetStats(GetPlayerStatisticsResult result)
//{
//    Debug.Log("Received the following Statistics:");
//    foreach (var eachStat in result.Statistics)
//    {
//        Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
//        switch (eachStat.StatisticName)
//        {
//            case "numberOfLogins":
//                numberOfLogins = eachStat.Value;
//                break;
//        }
//    }
//}
//
//
//
#endregion PlayerStats
#region Leaderboard
//public void GetLeaderboard()
//{
//    var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "numberOfLogins", MaxResultsCount = 20 };
//    PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetLeaderboard, OnErrorLeaderboard);
//}
//
//void OnGetLeaderboard(GetLeaderboardResult result)
//{
//    Debug.Log(result.Leaderboard[0].StatValue);
//    foreach (PlayerLeaderboardEntry player in result.Leaderboard)
//    {
//        Debug.Log(player.DisplayName + ":" + player.StatValue); //You can add in the display name if you already have that setup. You can either update the display name in the code or on the website.
//    }
//}
//
//void OnErrorLeaderboard(PlayFabError error)
//{
//    Debug.LogError(error.GenerateErrorReport());
//}
#endregion Leaderboard