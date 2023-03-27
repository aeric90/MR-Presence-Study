using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MCQuestion : Question
{
    public override string UnityType => "MULTIPLE_CHOICE";

    public override void Instantiate(GoogleQuestionData questionData)
    {
        base.Instantiate(questionData);
    }

    protected override void InstantiateButtons(GoogleQuestionData questionData)
    {
        int questionDataCount = 0;
        int questionDifference = 0;

        if(questionData.choices != null)
        {
            if(questionData.hasOtherOption)
            {
                questionData.choices.Add("Other...");
            }
        }

        questionDataCount = questionData.choices.Count;
        questionDifference = Buttons.Count - questionDataCount;

        if(questionDifference < 0)
        {
            _title.text = "Too many question choices added in google. Please limit MC Question choices to 4 maximum!";
            Buttons.ForEach(delegate (GameObject button)
            {
                button.SetActive(false);
            });
        }
        else
        {
            for (int i = 0; i < Buttons.Count - questionDifference; i++)
            {
                Buttons[i].SetActive(true);
                Buttons[i].GetComponentInChildren<TextMeshPro>(true).text = questionData.choices[i];
            }
        }
    }

    protected override void ClearContent()
    {
        //Loop buttons to reset all text to an empty string
        Buttons.ForEach(delegate (GameObject button) {
            button.GetComponentInChildren<TextMeshPro>().text = "";
            button.SetActive(false);
        });
    }
}