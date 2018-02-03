using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject currentSelected;

    [SerializeField]
    private Plane plane;

    private GameObject pointtomove;
	// Use this for initialization
	void Start () {

      
        pointtomove =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointtomove.transform.position = Camera.main.transform.position;
        pointtomove.transform.localScale = new Vector3(1, 1, 1);
        pointtomove.GetComponent<Renderer>().enabled = false;
        pointtomove.name = "POINTTOMOVE";

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click izquierdo");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name.Equals("Citizen"))
            {
                currentSelected = hit.transform.gameObject;

                printStatus(hit.transform);
            }

            //cuando volver el currentselected null
           
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
                        var citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();

                       
                       // if (hit.rigidbody != null)
                        {
                            
                          
                            pointtomove.transform.position = new Vector3( hit.point.x,1,hit.point.z);
                            citizenTemp.SetPointToMove(pointtomove.transform.position);
                            citizenTemp.SetState(CitizenStates.Walking);

                           
                                }
                        break;
                    case "GoldMine":
                         citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();
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

                rightclickedObj = hit.transform.gameObject.tag;

                switch (rightclickedObj)
                {
                    case "Building":


                        //TODO the player has selected a kind of bulding
                        //TODO check if the player can afford to build the bulding
                        //TODO create the gameobject depeding on the kind of bulding
                     
                       var citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Building);
                        
                        break;
                }

                Debug.Log(hit.transform.gameObject.name);

                // currentSelected = hit.transform.gameObject;

                //printStatus(hit.transform);

                //float rayDistance = 0;
                //var _plane = plane.GetComponent<Plane>();
                //if (_plane.Raycast(ray, out rayDistance))
                //{
                //    var citizenTemp = currentSelected.gameObject.GetComponent<CitizenScript>();
                //    Debug.Log(hit.transform.position);
                //    pointtomove.transform.position = hit.transform.position;
                //    citizenTemp.SetPointToMove(hit.transform.position);
                //    citizenTemp.SetState(CitizenStates.Walking);
                //}
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
