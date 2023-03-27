using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTankController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "cube")
        {
            Destroy(other.gameObject);
            GameController.instance.IncreaseScore();
        }
    }

}
