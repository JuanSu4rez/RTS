using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{

    //BoxSelection
    public Texture2D selectionTexture = null;
    public Texture2D selectionBorder = null;
    public static Rect selection = new Rect(0, 0, 0, 0);

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

    [SerializeField]
    private GameObject buildingValidator;

    // Use this for initialization
    void Start()
    {
        selectionTexture = new Texture2D(1, 1);
        selectionTexture.SetPixels32(new Color32[] { new Color32(10, 195, 28, 30) });
        selectionTexture.alphaIsTransparency = true;
        selectionTexture.Apply();

        selectionBorder = new Texture2D(1, 1);
        selectionBorder.SetPixels32(new Color32[] { new Color32(255, 255, 255, 85) });
        selectionBorder.Apply();

        camerastate = CameraStates.None;
        firstclick = empty;
        secondclick = empty;

        unitsGui = ScriptableObject.CreateInstance<UnitsGui>();
        unitsGui.SetBuildingValidator(buildingValidator);

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
            
            if (_mouseposition.y >= Screen.height - (Screen.height - Screen.width / 3.0f))
            {
                return;
            }
        }

        if (Input.GetMouseButtonDown(0) &&
            //todo temporal solution
            !unitsGui.HasOptionSelected() && firstclick == empty)
        {
            firstclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
             first3d = Input.mousePosition;
          

            Vector3 initialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (firstclick != empty)
            {
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

        if (Input.GetMouseButton(0))
        {
            second3d = Input.mousePosition;
            secondclick = Input.mousePosition;
            selection = new Rect(firstclick.x, Screen.height - firstclick.y, secondclick.x - firstclick.x, (Screen.height - secondclick.y) - (Screen.height - firstclick.y));

           // RaycastHit[] raycastHits = Physics.BoxCastAll(Camera.main.ScreenToWorldPoint(firstclick - secondclick), Camera.main.ScreenToWorldPoint(firstclick / 2 - secondclick / 2), Camera.main.transform.forward);
            //Debug.Log(raycastHits.Length);
            //Debug.Log(raycastHits[0]);

            //Camera.main.ScreenToWorldPoint(Input.mousePosition);


            //TODO SELECT ALL THE GAMEOBJECTS ON THE AREA
        }
    }

    Vector3 first3d = Vector3.zero;
    Vector3 second3d = Vector3.zero;

    Vector3 p1 = Vector3.zero;
    Vector3 p2 = Vector3.zero;
    Vector3 center = Vector3.zero;

   

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

                if (Input.GetMouseButton(0))
                {
                    GUI.DrawTexture(selection, selectionTexture);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, selection.width, 1), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x + selection.width, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y + selection.height, selection.width, 1), selectionBorder);
                }


                break;
          
            default:
                if (currentGui != null)
                    currentGui.ShowGUI(currentSelected);
                break;
        }
    }


}
