using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class SurveyButtonController : MonoBehaviour
{
    public static SurveyButtonController instance = null;

    public PokeInteractable buttonInteractable;

    public Renderer buttonBody;
    public TMPro.TextMeshPro buttonText;

    public Material buttonStart;
    public Material buttonNewTrial;
    public Material buttonInactive;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetButtonStart()
    {
        buttonInteractable.enabled = true;
        buttonBody.material = buttonStart;
        buttonText.text = "Start Trial";
    }

    public void SetButtonNewTrial()
    {
        buttonInteractable.enabled = true;
        buttonBody.material = buttonNewTrial;
        buttonText.text = "Next Trial";
    }

    public void SetButtonInactive()
    {
        buttonInteractable.enabled = false;
        buttonBody.material = buttonInactive;
        buttonText.text = "";
    }
}
