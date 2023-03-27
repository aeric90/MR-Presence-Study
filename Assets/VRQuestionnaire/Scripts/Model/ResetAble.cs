using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAble : MonoBehaviour
{
   private Transform Itransform;
    private Vector3 IPosition;
        private Vector3 IEular;
    // Start is called before the first frame update
    void Start()
    {
        IPosition = this.transform.position;
        IEular = this.transform.eulerAngles;
    }

    public void Reset()
    {
        this.transform.position = new Vector3(IPosition.x, IPosition.y, IPosition.z);
        this.transform.eulerAngles = new Vector3(IEular.x, IEular.y, IEular.z);
    }
}