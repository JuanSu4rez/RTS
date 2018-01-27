using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenScript : MonoBehaviour {

    private Collider citizenCollider;

	// Use this for initialization
	void Start () {
        citizenCollider = transform.GetComponent<Collider>();
        Debug.Log("citizenCollider " + citizenCollider != null);

	}

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Colision en el puto aldeano");

        Debug.Log(collision.gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {      
		
	}
}
