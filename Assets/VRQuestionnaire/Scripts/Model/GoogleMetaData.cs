using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoogleMetaData
{
    public string title { get; set; }
    public string id { get; set; }
    public string description { get; set; }
    public string publishedUrl { get; set; }
    public List<string> editorEmails { get; set; }
    public int count { get; set; }
    public string confirmationMessage { get; set; }
    public string customClosedFormMessage { get; set; }
}
