using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    Quaternion initialRotation;

    // Use this for initialization
    void Awake()
    {
        initialRotation = this.gameObject.transform.rotation;
    }

    void Start()
    {
        Destroy(gameObject, 4f);    
    }

    // Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity) * initialRotation;
        //transform.LookAt(transform.forward);
    }
}
