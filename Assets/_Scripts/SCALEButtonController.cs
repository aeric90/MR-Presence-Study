using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCALEButtonController : MonoBehaviour
{
    public TMPro.TextMeshPro labelText;
    public TMPro.TextMeshPro bottomText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLableText(string inputText)
    {
        labelText.text = inputText;
    }

    public void UpdateBottomText(string inputText)
    {
        bottomText.text = inputText;
    }
}
