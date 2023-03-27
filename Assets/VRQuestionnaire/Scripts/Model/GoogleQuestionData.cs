using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GoogleQuestionData
{
    public string type { get; set; }
    public string helpText { get; set; }
    public int index { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public int points { get; set; }
    public bool isRequired { get; set; }
    public bool hasOtherOption { get; set; }
    public List<string> choices { get; set; }
    public int upperBound { get; set; }
    public string rightLabel { get; set; }
    public string leftLabel { get; set; }
    public int lowerBound { get; set; }
}
