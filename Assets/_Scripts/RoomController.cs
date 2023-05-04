using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject mainDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        Debug.Log("ROOM REQUESTING DOOR OPEN");
        mainDoor.GetComponent<DoorContoller>().OpenDoor();
    }
    public void CloseDoor()
    {
        Debug.Log("ROOM REQUESTING DOOR CLOSE");
        mainDoor.GetComponent<DoorContoller>().CloseDoor();
    }
}
