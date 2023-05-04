using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorContoller : MonoBehaviour
{
    private Animator doorAnimator = null;
    public AudioClip doorSound;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        Debug.Log("DOOR OPENING");
        doorAnimator.SetTrigger("open");
        GetComponent<AudioSource>().PlayOneShot(doorSound);
    }

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("close");
        GetComponent<AudioSource>().PlayOneShot(doorSound);
    }
}
