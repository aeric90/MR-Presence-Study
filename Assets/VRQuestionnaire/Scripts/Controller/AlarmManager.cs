using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using UnityEditor.Animations;

public class AlarmManager : MonoBehaviour
{
    public static AlarmManager watchTimer;
    [Header("Alarm Management")]
    public float userTimerInput;
    public float fadeOutTime;
    public bool surveyStatus;
    public bool timerStatus;
    public AudioSource audioInput2;
    public GameObject ResetButton;
    private bool startTimer;
    private float timerClock;
    private bool coroutinePlaying;
    private string timerText = "";
    private readonly WaitForSeconds delay = new WaitForSeconds(5f);
    private readonly WaitForSeconds delay2 = new WaitForSeconds(1.5f);

    [SerializeField]
    //XRBaseController controller;
    AudioSource audioInput;

    Animator anim;

    // public Material watchMaterial;

    private void Awake()
    {
        if (AlarmManager.watchTimer == null)
        {
            AlarmManager.watchTimer = this;
        }
        else
        {
            if (AlarmManager.watchTimer != this)
            {
                Destroy(this.gameObject);
            }
        }

        anim = GetComponent<Animator>();
        audioInput = GetComponent<AudioSource>();

        if (audioInput == null || anim == null || audioInput2 == null)
        {
            Debug.LogWarning("Reference to controller or audio or animator was not set in the inspector", this);
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            ResetButton.SetActive(false);
            if (surveyStatus == false) 
            {
                timerClock -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(timerClock / 60);
                float seconds = Mathf.FloorToInt(timerClock % 60);
                timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
                Debug.Log(timerText);
                if (timerClock < 0)
                {
                    audioInput2.Stop();
                    FireAlarm();
                    startTimer = false;
                }
            }
            else if (surveyStatus == true)
            {
                anim.SetBool("playLights", false);
                timerClock = userTimerInput;
                startTimer = false;
                audioInput2.Stop();
                StopAllCoroutines();
            }   
        }
    }

        public void StartTimer()
        {
        if (startTimer == false && coroutinePlaying == false && surveyStatus == false)
            {
            timerClock = userTimerInput;
            StartCoroutine(FadeOut(audioInput2, fadeOutTime));
            audioInput2.Play();
            startTimer = true;
            }
        else
            {
            Debug.LogWarning("Trial or timer could not start! Make sure the questionnaire is closed or the application is not requesting to fill the questionnaire.");
            }
        }


        #region ALARMEVENTS

        void FireAlarm()
        {
            coroutinePlaying = true;
            StartCoroutine(StartLights());
            StartCoroutine(StartPeriodicAudio());
            StartCoroutine(StartPeriodicHaptics());
        }

        void PlayAudio()
        {
            if (audioInput != null)
            {
                audioInput.Play(0);
                Debug.Log("Sending Audio!");
            }
        }
    /*
        void SendHaptics()
        {
            if (controller != null)
            {
                controller.SendHapticImpulse(0.7f, 0.1f);
                Debug.Log("Sending Haptic!");
            }
        }
    */

        IEnumerator StartLights()
        {
            yield return delay2;
            Debug.Log("Sending Lights!");
            anim.SetBool("playLights", true);
        }

        IEnumerator StartPeriodicHaptics()
        {
        coroutinePlaying = true;  
            while (surveyStatus == false)
            {
                //SendHaptics();
                yield return delay;
            }
        coroutinePlaying = false;          
    }
        IEnumerator StartPeriodicAudio()
        {
        coroutinePlaying = true;
            while (surveyStatus == false)
            {
                PlayAudio();
                yield return delay;
            }
        coroutinePlaying = false;
        anim.SetBool("playLights", false);
    }

        IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

    }


    #endregion ALARMEVENTS  
