using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LSQuestion : Question
{
    public override string UnityType => "SCALE";

    public override void Instantiate(GoogleQuestionData questionData)
    {
        base.Instantiate(questionData);
    }

    protected override void InstantiateButtons(GoogleQuestionData questionData)
    {
        int k = 0;
        if(questionData.lowerBound >=0 && questionData.lowerBound <=1)
            if(questionData.upperBound >= 2 && questionData.upperBound <=7)
            {
                Buttons[k].GetComponentInChildren<TextMeshProUGUI>().text = questionData.leftLabel;
                
                for (int i = questionData.lowerBound; i <= questionData.upperBound; i++)
                {  
                    Buttons[k].GetComponentInChildren<TextMeshPro>().text = i.ToString();
                    Buttons[k].SetActive(true);
                    k++;
                }
                    if(questionData.lowerBound == 0)
                    Buttons[questionData.upperBound].GetComponentInChildren<TextMeshProUGUI>().text = questionData.rightLabel;
                    if(questionData.lowerBound == 1)
                    Buttons[questionData.upperBound-1].GetComponentInChildren<TextMeshProUGUI>().text = questionData.rightLabel;
            }
            else
            {
                _title.text = "This app only supports likert scale options with a lowerbound of either 0 or 1 and an upperbound of 7";
                Buttons.ForEach(delegate (GameObject button)
                {
                    button.SetActive(false);
                });
            }
    }

    protected override void ClearContent()
    {
        //Loop buttons to reset all text to an empty string
        Buttons.ForEach(delegate (GameObject button) {
            button.GetComponentInChildren<TextMeshPro>().text = "";
            button.GetComponentInChildren<TextMeshProUGUI>().text = "";
            button.SetActive(false);
        });
    }
}
