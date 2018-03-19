using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{

    private readonly Vector2 empty = new Vector2(-10, -10);

    private Vector2 firstclick;

    private Vector2 secondclick;

    private CameraSelectionTypes cameraSelectionType = CameraSelectionTypes._None;


    private GameObject[] currentSelecteds;

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

    private IGui currentGui;


    private BuildingGui buildingGui;
    private MilitaryGui militaryGui;
    private UnitsGui unitsGui;
    // Use this for initialization
    void Start()
    {

        camerastate = CameraStates.None;
        firstclick = empty;
        secondclick = empty;

        unitsGui = ScriptableObject.CreateInstance<UnitsGui>();
        buildingGui = ScriptableObject.CreateInstance<BuildingGui>();
        militaryGui = ScriptableObject.CreateInstance<MilitaryGui>();

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

        if (currentGui == null)
            return;

        switch (camerastate)
        {
            case CameraStates.None:

                break;
            case CameraStates.UnitsSelection:

                if (!currentGui.HasOptionSelected())
                    ClickActionsUnits();
                else
                    currentGui.UpdateGui(currentSelected);


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
                //then count each type citizen and militaries or bulginds

                //In the mean time keep the same logic
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(firstclick);

                if (Physics.Raycast(ray, out hit))
                {

                    var _name = hit.transform.gameObject.name;
                    //TRANSATITION TO UnitsSelection OR BuildingsSelection
                    var hitselected = hit.transform.gameObject;

                    SetSelectedGameObject(hitselected);

                    if (currentSelected != null)
                    {


                        var _tag = currentSelected.tag;


                        switch (_tag)
                        {
                            case "Citizen":
                                camerastate = CameraStates.UnitsSelection;
                                cameraSelectionType = CameraSelectionTypes.Citizen;
                                currentGui = this.unitsGui;
                                break;
                            case "Military":
                                camerastate = CameraStates.UnitsSelection;
                                cameraSelectionType = CameraSelectionTypes.Military;
                                currentGui = this.militaryGui;
                                break;
                            case "Building":
                                camerastate = CameraStates.UnitsSelection;
                                cameraSelectionType = CameraSelectionTypes.Bulding;
                                currentGui = this.buildingGui;
                                break;
                            default:
                                camerastate = CameraStates.None;
                                currentGui = null;
                                break;
                        }
                    }

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

    private void ClickActionsUnits()
    {
        if (currentSelected == null)
            return;

        switch (cameraSelectionType)
        {
            case CameraSelectionTypes.Citizen:
                CitizenAction();
                break;
            case CameraSelectionTypes.People:
                PeopleAction();
                break;

            case CameraSelectionTypes.Military:
                MilitaryAction();
                break;
            case CameraSelectionTypes.Bulding:
                BuldingAction();
                break;
        }

    }

    private void PeopleAction()
    {

    }

    private void BuldingAction()
    {

    }

    private void MilitaryAction()
    {
        var soldierTemp = currentSelected.gameObject.GetComponent<IControlable<SoldierStates>>();

        if (soldierTemp == null)
        {
            return;
        }

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
                        pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                        soldierTemp.SetState(SoldierStates.Walking);
                        soldierTemp.SetPointToMove(pointtomove.transform.position);
                  
                        soldierTemp.ReleaseTask();
                        break;

                }

                string rightclickedtag = hit.transform.gameObject.tag;

                //TODO when tag is bulding and enemy assinga miltary task to attack
                //of is alive beign or bulding
                //switch (rightclickedtag)
                //{
                //    case "Building":
                //       
                //        break;
                //   
                //}

            }


        }
    }

    private void CitizenAction()
    {
        var citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();

        if (citizenTemp == null)
        {
            return;
        }


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



                        pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                        citizenTemp.SetPointToMove(pointtomove.transform.position);
                        citizenTemp.SetState(CitizenStates.Walking);


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




    private void OnGUI()
    {

        switch (camerastate)
        {
            case CameraStates.None:
                if (firstclick != empty)
                {

                    secondclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //; new Vector2(firstclick.x+100, firstclick.y+100);// Input.mousePosition;

                    //Debug.Log("Second click " + secondclick.x + " " + secondclick.y);
                    //Default color to render the selection square
                    GUI.contentColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    GUI.Box(new Rect(firstclick.x, Screen.height - firstclick.y, secondclick.x - firstclick.x, (Screen.height - secondclick.y) - (Screen.height - firstclick.y)), ""); // -


                }

                break;
            //case CameraStates.UnitsSelection:
            //
            //    switch (cameraSelectionType)
            //    {
            //        case CameraSelectionTypes.Citizen:
            //            unitsGui.ShowGUI(currentSelected);
            //            break;
            //    }
            //
            //
            //       
            //  
            //
            //    break;
            //case CameraStates.BuildingsSelection:
            //    buildingGui.ShowGUI(currentSelected);
            //    break;

            default:
                if (currentGui != null)
                    currentGui.ShowGUI(currentSelected);
                break;
        }
    }


}