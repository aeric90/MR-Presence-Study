using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTankController : MonoBehaviour
{
    public Material tankNormalMaterial;
    public Material tankProcessingMaterial;
    public Renderer tankRenderer;
    public AudioClip cubeScoreSound;

    private bool processing = false;

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
        if(other.tag == "cube" && !processing)
        {
            Destroy(other.gameObject);
            StartCoroutine(TankProcessing());
        }
    }

    IEnumerator TankProcessing()
    {
        processing = true;
        GetComponent<AudioSource>().PlayOneShot(cubeScoreSound);
        GameController.instance.IncreaseScore();
        yield return new WaitForSeconds(1.0f);
        // PLAY SOUND
        processing = false;
    }

}
