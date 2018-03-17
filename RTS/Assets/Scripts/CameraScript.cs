using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{

    private readonly Vector2 empty = new Vector2(-10, -10);

    private Vector2 firstclick;

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

    private UnitsGui unitsGui;
    private BuildingGui buildingGui;

    // Use this for initialization
    void Start()
    {

        camerastate = CameraStates.None;
        firstclick = empty;
        secondclick = empty;
        unitsGui = ScriptableObject.CreateInstance<UnitsGui>();

        buildingGui = ScriptableObject.CreateInstance<BuildingGui>();


        #region point to move
        pointtomove = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointtomove.transform.position = Camera.main.transform.position;
        pointtomove.transform.localScale = new Vector3(1, 1, 1);
        pointtomove.GetComponent<Renderer>().enabled = false;
        pointtomove.name = "POINTTOMOVE";
        #endregion

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Camera state " + camerastate.ToString() + " " + DateTime.Now.ToString("yyMMddHHmmss"));

        //Debug.Log("unitsGui HasOptionSelected " + unitsGui.HasOptionSelected() + " " + DateTime.Now.ToString("yyMMddHHmmss"));
        userClick();

        switch (camerastate)
        {
            case CameraStates.None:

                break;
            case CameraStates.UnitsSelection:

                if (!unitsGui.HasOptionSelected())
                    ClickActionsUnits();
                else
                    unitsGui.Update();
                break;
            case CameraStates.BuildingsSelection:
                break;
        }



    }

    private void userClick()
    {

       if (camerastate != CameraStates.None)
       {
           //if the gui is showing avoid picking object within user gui
           Vector3 _mouseposition = Input.mousePosition;
           _mouseposition.y = Screen.height - Input.mousePosition.y;
           //TODO CREATE CONSTANT
           if (_mouseposition.y >= Screen.height - 100)
           {
               return;
           }

       }

        if (Input.GetMouseButtonDown(0) &&
            //todo temporal solution
            !unitsGui.HasOptionSelected() &&
            firstclick == empty)
        {
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
                    var sphere = hit.transform.gameObject.GetComponent<SphereCollider>();
                    var capsule = hit.transform.gameObject.GetComponent<CapsuleCollider>();

                    var _name = hit.transform.gameObject.name;
                    //TRANSATITION TO UnitsSelection OR BuildingsSelection
                    var hitselected = hit.transform.gameObject;

                    SetSelectedGameObject(hitselected);
                   

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

    private void SetSelectedGameObject(GameObject hitselected)
    {
  

        if (hitselected != null)
        {
            var auxst = hitselected.GetComponent<ISelectable>();
            if (auxst != null)
                auxst.IsSelected = true;

            if (currentSelected != null)
            {
                auxst = currentSelected.GetComponent<ISelectable>();
                if (auxst != null)
                    auxst.IsSelected = false;
            }
            currentSelected = hitselected;
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
        if (currentSelected == null)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            //when a citizen is selected
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                string rightclickedObj = hit.transform.gameObject.name;
                switch (rightclickedObj)
                {
                    case "Land":
                        var citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        var soldierTemp = currentSelected.gameObject.GetComponent<NavAgentSoldierScript>();
                        var archerTemp = currentSelected.gameObject.GetComponent<NavAgentArcherScript>();
                        if (citizenTemp != null)
                        {
                            pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            citizenTemp.SetPointToMove(pointtomove.transform.position);
                            citizenTemp.SetState(CitizenStates.Walking);
                        }
                        if (soldierTemp != null)
                        {
                            pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            soldierTemp.SetPointToMove(pointtomove.transform.position);
                            soldierTemp.SetState(SoldierStates.Walking);
                        }
                        if (archerTemp != null)
                        {
                            pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                            archerTemp.SetPointToMove(pointtomove.transform.position);
                            archerTemp.SetState(SoldierStates.Walking);
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

    public bool HasScript<T>()
    {

        return currentSelected != null && currentSelected.GetComponent<T>() != null;
    }


    private void OnGUI()
    {

        switch (camerastate)
        {
            case CameraStates.None:
                if (firstclick != empty)
                {
                    //if (Input.GetMouseButtonDown(0))
                    {
                        secondclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //; new Vector2(firstclick.x+100, firstclick.y+100);// Input.mousePosition;

                        //Debug.Log("Second click " + secondclick.x + " " + secondclick.y);
                        //Default color to render the selection square
                        GUI.contentColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        GUI.Box(new Rect(firstclick.x, Screen.height - firstclick.y, secondclick.x - firstclick.x, (Screen.height - secondclick.y) - (Screen.height - firstclick.y)), ""); // -
                    }

                }

                break;
            case CameraStates.UnitsSelection:
                unitsGui.ShowGUI();

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