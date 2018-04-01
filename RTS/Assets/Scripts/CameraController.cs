using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    Vector3 cameraPosition;


	// Use this for initialization
	void Start () {
        cameraPosition = gameObject.transform.position;
        Vector3 initialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log("Total de la pantalla " + Screen.width);
        Debug.Log("Tercio de la pantalla " + Screen.width / 3);
    }
	
	// Update is called once per frame
	void Update () {
        
        //cameraPosition = gameObject.transform.position;
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePosition = Input.mousePosition;

        Debug.Log("mousePosition.x " + mousePosition.x);

        if (mousePosition.x < Screen.width/8){            
            gameObject.transform.position =  new Vector3(--cameraPosition.x, cameraPosition.y, ++cameraPosition.z);
        }
        if (mousePosition.x > Screen.width - Screen.width/8){
            gameObject.transform.position = new Vector3(++cameraPosition.x, cameraPosition.y, --cameraPosition.z);
        }
        if (mousePosition.y < Screen.height/8)
        {
            gameObject.transform.position = new Vector3(--cameraPosition.x, cameraPosition.y, --cameraPosition.z);
        }
        if (mousePosition.y > Screen.height - Screen.height/8)
        {
            gameObject.transform.position = new Vector3(++cameraPosition.x, cameraPosition.y, ++cameraPosition.z);
        }


    }
}
