using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);



[System.Serializable]
public class GoogleSurveyData
{
    public GoogleMetaData metadata { get; set; }
    public List<GoogleQuestionData> items { get; set; }
    public int count { get; set; }
}


