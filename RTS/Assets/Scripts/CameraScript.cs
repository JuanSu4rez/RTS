using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject currentSelected;

	// Use this for initialization
	void Start () {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click izquierdo");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                currentSelected = hit.transform.gameObject;

                printStatus(hit.transform);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                string rightclickedObj = hit.transform.gameObject.name;
                switch (rightclickedObj)
                {
                    case "Land":
                     //   Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                      //  newPosition.y = 1;
                      //  currentSelected.transform.position = newPosition;
                        break;
                    case "GoldMine":
                        var citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Gold;                        
                        break;
                    case "Forest":
                        citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Wood;
                        break;
                    default:
                        break;
                }

                Debug.Log(hit.transform.gameObject.name);

               // currentSelected = hit.transform.gameObject;

                //printStatus(hit.transform);
            }
        }
    }

    private void printStatus(Transform transform)
    {
        Debug.Log(transform.gameObject.name);
        transform.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Specular");
        transform.gameObject.GetComponent<Renderer>().material.SetColor("_SpecColor", Color.red);
    }

    //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //if (Physics.Raycast(ray, out hit))
    //{
    //    Transform objectHit = hit.transform;
    //}
}
