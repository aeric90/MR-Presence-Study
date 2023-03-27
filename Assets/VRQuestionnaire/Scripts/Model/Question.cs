using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public abstract class Question : MonoBehaviour
{
    public abstract string UnityType { get; }
    public TextMeshPro _title;
    public string _answer = "No Answer";
    [HideInInspector]
    public string _index;
    public bool _isRequired;
    public List<GameObject> Buttons;
    private GameObject buttonActive;

    protected abstract void InstantiateButtons(GoogleQuestionData questionData);
    protected abstract void ClearContent();
    //private void OnDisable()
    //{
    //    Buttons.ForEach(delegate (GameObject button)
    //    {
    //        if (button.GetComponentInChildren<TextMeshPro>() != null)
    //            button.GetComponentInChildren<TextMeshPro>().text = "";
    //        if (button.GetComponentInChildren<TextMeshProUGUI>() != null)
    //            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
    //
    //        button.SetActive(false);
    //    });
    //}
    public virtual void Instantiate(GoogleQuestionData questionData)
    {
        if (UnityType == questionData.type)
        {
            _title.text = questionData.title;
            _isRequired = questionData.isRequired;
            _index = (questionData.index + 1).ToString();
            //If a question is required it will concatenate a red asterik to the end
            if (_isRequired)
            {
                _title.text += " <color=red>*</color>"; 
            }
        }
        else
        {
            this._title.text = "The quetion unityType does not match the question googleType";
        }
        ClearContent();
        InstantiateButtons(questionData); 
    }

    public virtual void onButtonClick(GameObject buttonIn)
    {
        TransitionEffect buttonInScript = buttonIn.GetComponent<TransitionEffect>();
        //Reset all Buttons to idleMesh
        foreach (var button in Buttons)
        {
            if (button != null && buttonIn != buttonActive)
                button.GetComponentInChildren<TransitionEffect>(false).DeactivateMaterial();
        }

        if (buttonIn == buttonActive)
        {
            _answer = "No Answer";
            Debug.Log("Answer is : " + _answer);
            buttonInScript.DeactivateMaterial();
            buttonActive = null;
        }
        else if (buttonIn != buttonActive)
        {
            buttonInScript.ActivateMaterial();
            _answer = buttonIn.GetComponentInChildren<TextMeshPro>().text;
            buttonActive = buttonIn;
            Debug.Log("Answer is : " + _answer);
        }
    }
  
}
