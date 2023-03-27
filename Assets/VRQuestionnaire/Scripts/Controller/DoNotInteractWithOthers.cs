using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotInteractWithOthers   : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "RightHand Controller")
        {
        Physics.IgnoreCollision(other,this.GetComponent<Collider>());
        }
    }
}
