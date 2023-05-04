using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    FLOORCHECK,
    DEMO,
    START,
    TRIAL,
    SURVEY,
    REST,
    END
}

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private GameState currentGameState = GameState.FLOORCHECK;

    public float trialTimer = 0.0f;
    private string trialChrono = "00:00:00";

    private int goalCount = 0;  // Number of goals scored. 0 to 4

    //public TMPro.TextMeshPro leftHandWatchText;
    //public TMPro.TextMeshPro rightHandWatchText;

    public Transform rightControllerLocation;
    public Transform cameraRigLocation;

    private GameObject CurrentTrialEnv = null;
    public int currentTrialID = 0;
    public List<GameObject> TrialEnvs = new List<GameObject>();
    public List<GameObject> TrialSurveys = new List<GameObject>();

    private List<int> CompletedTrials = new List<int>();
    public float[] TrialTimes = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    public GameObject sciFiCratePrefab;
    public GameObject sciFiBoxParent;
    public Transform sciFiCrateSpawn;

    private string trialScreenText;

    public List<AudioClip> trialStartSounds = new List<AudioClip>();
    public AudioClip trialEndSound;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = this;
        CurrentTrialEnv = TrialEnvs[0];
        SetFloorCheck();
        Debug.Log(currentTrialID);
        Debug.Log(TrialTimes);
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch(currentGameState)
        {
            case GameState.FLOORCHECK:
                if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Two)) FloorCheck();
                break;
            case GameState.DEMO:
            case GameState.TRIAL:
                trialTimer += Time.deltaTime;
                UpdateTrialChrono();
                trialScreenText = trialChrono + "<BR>SCORE - " + goalCount;
                if (goalCount >= 4) EndTrial();
                break;
            case GameState.SURVEY:
                break;
            case GameState.REST:
                if (CompletedTrials.Count >= 4) EndExperiment();
                break;
            case GameState.END:
                break;
            default:
                break;
        }
    }

    public void ResetTrial()
    {
        trialTimer = 0.0f;
        goalCount = 0;
    }

    public void IncreaseScore()
    {
        goalCount++;
        if(currentTrialID == 0) Instantiate(sciFiCratePrefab, sciFiCrateSpawn.position, sciFiCrateSpawn.rotation, sciFiBoxParent.transform);
    }

    private void UpdateTrialChrono()
    {
        string trialMinutes = Mathf.Floor(trialTimer / 60).ToString("00");
        string trialSeconds = (trialTimer % 60).ToString("00");
        string trialMilli = ((trialTimer * 100) % 100).ToString("00");
        trialChrono = trialMinutes + ":" + trialSeconds + ":" + trialMilli;
    }

    public string GetScreenText()
    {
        switch (currentGameState)
        {
            case GameState.FLOORCHECK:
            case GameState.DEMO:
            case GameState.TRIAL:
                return trialScreenText;
                break;
            case GameState.SURVEY:
                return "Survey Time";
                break;
            case GameState.REST:
                return "Press NEXT when ready";
                break;
            case GameState.END:
                return "Thank you!";
                break;
            default:
                return "Something's Wrong...";
                break;
        }

        return "";
    }

    private void GetNextTrial()
    {
        int nextTrial = 0;
        int randomTrial = 0;

        while(nextTrial == 0)
        {
            randomTrial = Random.Range(1, 5);
            if (!CompletedTrials.Contains(randomTrial)) nextTrial = randomTrial;
        }

        currentTrialID = nextTrial;
    }

    public void TrialButtonPushed()
    {
        switch (currentGameState)
        {
            case GameState.REST:
                StartCoroutine(SwitchRoomRoutine());
                break;
            case GameState.FLOORCHECK:
                Debug.Log("STARTING TRIAL");
                StartCoroutine(StartTrial());
                break;
            default:
                break;
        }
    }

    IEnumerator StartTrial()
    {
        SurveyButtonController.instance.SetButtonInactive();

        for (int i = 3; i >= 0; i--)
        {
            GetComponent<AudioSource>().PlayOneShot(trialStartSounds[Mathf.Abs(i - 3)]);
            // Play beep. If zero, play horn
            if (i > 0)
            {
                trialScreenText = i.ToString();
            } else
            {
                trialScreenText = "GO!";
            }

            yield return new WaitForSeconds(1.0f);
        }

        if (currentTrialID != 0)
        {
            CurrentTrialEnv.GetComponent<RoomController>().OpenDoor();
            currentGameState = GameState.TRIAL;
        }
        else
        {
            currentGameState = GameState.DEMO;
            Instantiate(sciFiCratePrefab, sciFiCrateSpawn.position, sciFiCrateSpawn.rotation, sciFiBoxParent.transform);
        }
    }

    IEnumerator SwitchRoomRoutine()
    {
        FadeController.instance.SetFade(true);
        yield return new WaitForSeconds(5);
        CurrentTrialEnv.SetActive(false);
        GetNextTrial();
        CurrentTrialEnv = TrialEnvs[currentTrialID];
        CurrentTrialEnv.SetActive(true);
        ResetTrial();
        FadeController.instance.SetFade(false);
        SetFloorCheck();
    }

    private void EndTrial()
    {
        if (currentTrialID != 0)
        {
            CurrentTrialEnv.GetComponent<RoomController>().CloseDoor();
            CompletedTrials.Add(currentTrialID);
        }
        GetComponent<AudioSource>().PlayOneShot(trialEndSound);
        TrialTimes[currentTrialID] = trialTimer;
        ActivateSurvey();
        currentGameState = GameState.SURVEY;
    }

    private void EndExperiment()
    {
        PlayFabController.PFC.ExitnSave();
        // Say thank you on the screens!
        currentGameState = GameState.END;
    }

    public void EndSurvey()
    {
        TrialSurveys[currentTrialID].SetActive(false);
        SurveyButtonController.instance.SetButtonNewTrial();
        currentGameState = GameState.REST;
    }

    private void ActivateSurvey()
    {
        if(!TrialSurveys[currentTrialID].activeSelf) TrialSurveys[currentTrialID].SetActive(true);
    }

    private void FloorCheck()
    {
        cameraRigLocation.position += new Vector3(0.0f, -(rightControllerLocation.position.y + 0.02f) , 0.0f);
    }

    private void SetFloorCheck()
    {
        trialScreenText = "Floor Check";
        currentGameState = GameState.FLOORCHECK;
        SurveyButtonController.instance.SetButtonStart();
        // Set all screens to ask for floor check.
    }
}
