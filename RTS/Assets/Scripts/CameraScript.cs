using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour {

    private readonly Vector2 empty = new Vector2(-10, -10);

    private Vector2 firstclick ;

    private Vector2 secondclick;



    private GameObject currentSelected;


    private CameraStates camerastate;

    public GameObject CurrentSelected
    {
        get
        {
            return currentSelected;
        }
    }

    [SerializeField]
    private Plane plane;

    private GameObject pointtomove;

    private UnitsGui selectedGui;
    private BuildingGui buildingGui;

    // Use this for initialization
    void Start () {

        camerastate = CameraStates.None;
        firstclick = empty;
        secondclick = empty;
        selectedGui= ScriptableObject.CreateInstance<UnitsGui>();

        buildingGui = ScriptableObject.CreateInstance<BuildingGui>();


        #region point to move
        pointtomove =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointtomove.transform.position = Camera.main.transform.position;
        pointtomove.transform.localScale = new Vector3(1, 1, 1);
        pointtomove.GetComponent<Renderer>().enabled = false;
        pointtomove.name = "POINTTOMOVE";
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        userClick();

        switch (camerastate)
        {
            case CameraStates.None:                
                break;
            case CameraStates.UnitsSelection:

                if (!selectedGui.HasOptionSelected())
                    ClickActionsUnits();
                else
                    selectedGui.Update();
                break;
            case CameraStates.BuildingsSelection:
                break;
        }


      
    }

    private void userClick() {

        if (Input.GetMouseButtonDown(0) && firstclick == empty){
            firstclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (firstclick != empty)
            {
                //TODO SELECT ALL THE GAMEOBJECTS ON THE AREA

                //In the mean time keep the same logic
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(firstclick);

                if (Physics.Raycast(ray, out hit))
                {
                    var _name = hit.transform.gameObject.name;
                    //TRANSATITION TO UnitsSelection OR BuildingsSelection
                    currentSelected = hit.transform.gameObject;
                    //TODO CREATE SOMETHING LIKE INSTANCE OF BY CHECKING ITS COMPONENTS

                    switch (_name)
                    {
                        case "Citizen":
                            camerastate = CameraStates.UnitsSelection;
                            break;
                        case "Barracks":
                            camerastate = CameraStates.BuildingsSelection;
                            break;
                        case "UrbanCenter":
                            camerastate = CameraStates.BuildingsSelection;
                            break;
                        default:
                            camerastate = CameraStates.None;
                            break;
                    }

                    var _tag = hit.transform.gameObject.tag;

                    switch (_tag)
                    {
                        case "Citizen":
                            camerastate = CameraStates.UnitsSelection;
                            break;
                        case "Building":
                            camerastate = CameraStates.BuildingsSelection;
                            break;
                        default:
                            camerastate = CameraStates.None;
                            break;
                    }

                    printStatus(hit.transform);
                }
                firstclick = empty;
            }
        }
    }


    private void printStatus(Transform transform)
    {
        //Debug.log(transform.gameObject.name);
        //transform.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Specular");
        //transform.gameObject.GetComponent<Renderer>().material.SetColor("_SpecColor", Color.red);
    }

    private void ClickActionsUnits()
    {
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
                        var citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();


                        // if (hit.rigidbody != null)
                        {
                            pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            citizenTemp.SetPointToMove(pointtomove.transform.position);
                            citizenTemp.SetState(CitizenStates.Walking);
                        }

                        break;
                    case "GoldMine":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Gold;
                        break;
                    case "Forest":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Wood;
                        break;

                    case "Building":

                        //TODO the player has selected a kind of bulding
                        //TODO check if the player can afford to build the bulding
                        //TODO create the gameobject depeding on the kind of bulding

                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Building);

                        break;

                    default:
                        break;
                }

                string rightclickedtag = hit.transform.gameObject.tag;
                switch (rightclickedtag)
                {
                    case "Building":
                        var citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        //TODO the player has selected a kind of bulding
                        //TODO check if the player can afford to build the bulding
                        //TODO create the gameobject depeding on the kind of bulding

                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Building);
                        break;
                }

                  

                }


        }

    }

    //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //if (Physics.Raycast(ray, out hit))
    //{
    //    Transform objectHit = hit.transform;
    //}

    public bool HasScript<T>(){

        return currentSelected != null && currentSelected.GetComponent<T>()!= null;
    }


    private void OnGUI()
    {
        switch (camerastate)
        {
            case CameraStates.None:
                if(firstclick!= empty)
                {
                    //if (Input.GetMouseButtonDown(0))
                    {
                         secondclick  = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //; new Vector2(firstclick.x+100, firstclick.y+100);// Input.mousePosition;


                        GUI.Box(new Rect(firstclick.x, Screen.height - firstclick.y, secondclick.x - firstclick.x,     (Screen.height - secondclick.y)- (Screen.height - firstclick.y)),""); // -
                    }

                }

                break;
            case CameraStates.UnitsSelection:
                selectedGui.ShowGUI();

                break;
            case CameraStates.BuildingsSelection:
                buildingGui.ShowGUI(currentSelected);
                break;
        }
    }

    private void ButtonActionsUnits()
    {
       
    }
}