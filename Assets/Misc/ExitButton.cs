using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    public GameObject exitButton;

    public void onClicked()
    {
        Application.Quit();
    }
}
