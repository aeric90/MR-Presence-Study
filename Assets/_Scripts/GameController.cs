using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private bool trialRunning = false;
    
    private int trialCount;  // Number of trials completed. 0 to 4


    private float trialTimer = 0.0f;
    private int goalCount = 0;  // Number of goals scored. 0 to 4



    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (trialRunning)
        {
            trialTimer += Time.deltaTime;
            if (goalCount >= 4)
            {
                trialRunning = false;
            } 
        }
    }

    public void ResetTrial()
    {
        trialTimer = 0.0f;
        goalCount = 0;
    }

    // Code that turns MR environment ON/OFF.

    // Code that turn environment from high detail to low detail.

    public void IncreaseScore()
    {
        goalCount++;
    }

}
