using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public void resetButton()
    {
        foreach (var material in FindObjectsOfType<ResetAble>(true))
        {
            material.Reset();
        }
    }
}
