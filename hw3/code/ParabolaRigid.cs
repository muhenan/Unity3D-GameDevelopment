using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRigid : MonoBehaviour
{

    private Vector3 speed;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime * 10;
        Debug.Log(this.transform.position);
    }
}
