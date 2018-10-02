using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{

    public ArrayList objectsInScene;

    //BoxSelection
    private Texture2D selectionTexture = null;
    private Texture2D selectionBorder = null;
    private static Rect selection = new Rect(0, 0, 0, 0);

    private readonly Vector2 empty = new Vector2(-10, -10);

    private Vector2 firstclick;

    private Vector2 secondclick;

    private CameraSelectionTypes cameraSelectionType = CameraSelectionTypes._None;


    private ArrayList currentSelecteds;

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

        objectsInScene = new ArrayList();
        currentSelecteds = new ArrayList();
        GetObjectsinScene();

    }

    public void GetObjectsinScene()
    {

        GameObject[] allSelectableObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        for (int i = 0; i < allSelectableObjects.Length; i++)
        {
            ISelectable tempSel = allSelectableObjects[i].GetComponent<ISelectable>();
            if (tempSel != null)
            {
                objectsInScene.Add(allSelectableObjects[i]);
            }
        }
    }

    public static Vector2 get2sCoordinates(GameObject selectable)
    {
        if (selectable == null)
            return Vector2.zero;

        Transform transform = selectable.GetComponent<Transform>();
        Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
        return new Vector2(v.x, Screen.height - v.y);
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is not putting a building or something
        if (currentGui == null || !currentGui.HasOptionSelected())
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
            //check wether the clikc is inside of the button area
            Vector3 _mouseposition = Input.mousePosition;
            _mouseposition.y = Screen.height - Input.mousePosition.y;
            //TODO CREATE CONSTANTS

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
            Vector3 initialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (firstclick != empty)
            {
                if (currentSelecteds.Count > 0)
                {
                    //Debug.Log("Camera state " + camerastate);
                    for (int i = 0; i < currentSelecteds.Count; i++)
                    {
                        ((GameObject)currentSelecteds[i]).gameObject.GetComponent<ISelectable>().IsSelected = true;
                    }
                    camerastate = CameraStates.UnitsSelection;
                    cameraSelectionType = CameraSelectionTypes.Military;
                }
                else
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
                }

                firstclick = empty;
            }
        }

        if (Input.GetMouseButton(0))
        {
 
            secondclick = Input.mousePosition;

            Vector2 selectionStart = new Vector2();
            Vector2 selectionEnd = selectionStart;

            selectionStart.x = firstclick.x <= secondclick.x ? firstclick.x : secondclick.x;
            selectionStart.y = firstclick.y >= secondclick.y ? Screen.height - firstclick.y : Screen.height - secondclick.y;
            selectionEnd.x = firstclick.x <= secondclick.x ? secondclick.x : firstclick.x;
            selectionEnd.y = firstclick.y <= secondclick.y ? Screen.height - firstclick.y : Screen.height - secondclick.y;

            float width = selectionEnd.x - selectionStart.x;
            float heigth = selectionEnd.y - selectionStart.y;

            //selection = new Rect(firstclick.x, Screen.height - firstclick.y, secondclick.x - firstclick.x, (Screen.height - secondclick.y) - (Screen.height - firstclick.y));

            selection = new Rect(selectionStart.x, selectionStart.y, width, heigth);

            //TODO SELECT ALL THE GAMEOBJECTS ON THE AREA
            if (selection.width != 0 && selection.height != 0) {
                currentSelecteds.RemoveRange(0, currentSelecteds.Count);
                for (int index = 0; index < objectsInScene.Count; index++) {
                    GameObject obj = ((GameObject)objectsInScene[index]);
                    if (selection.Contains(get2sCoordinates(obj)))
                    {
                        currentSelecteds.Add(obj);
                        obj.gameObject.GetComponent<ISelectable>().IsSelected = true;
                    }
                    else {
                        currentSelecteds.Remove(obj);
                        obj.gameObject.GetComponent<ISelectable>().IsSelected = false;
                    }
                }
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
                CitizenTask citizenTask = CitizenTask.Empty;

                string rightclickedObj = hit.transform.gameObject.tag;
                switch (rightclickedObj)
                {
                    case "Land":
                        pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                        citizenTemp.SetPointToMove(pointtomove.transform.position);
                        citizenTemp.SetState(CitizenStates.Walking);
                        break;
                    case "Gold":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Gold;

                        citizenTask = new CitizenTask(hit.transform.position, hit.transform.gameObject, CitizenStates.Gathering, Resources.Gold);
                        break;
                    case "Forest":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Wood;

                        citizenTask = new CitizenTask(hit.transform.position, hit.transform.gameObject, CitizenStates.Gathering, Resources.Wood);

                        break;

                    case "Rock":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetPointResource(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Gathering);
                        citizenTemp.CurrentResource = Resources.Rock;

                        citizenTask = new CitizenTask(hit.transform.position, hit.transform.gameObject, CitizenStates.Gathering, Resources.Rock);

                        break;

                    case "Building":

                        //TODO the player has selected a kind of bulding
                        //TODO check if the player can afford to build the bulding
                        //TODO create the gameobject depeding on the kind of bulding

                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();
                        citizenTemp.SetPointToMove(hit.transform.position);
                        citizenTemp.SetState(CitizenStates.Building);

                        citizenTask = CitizenTask.CitizenTaskBulding(hit.transform.position, hit.transform.gameObject);

                        break;

                    default:
                        break;
                }

                if(citizenTask != CitizenTask.Empty)
                citizenTemp.SetCitizenTask(citizenTask);


            }
        }
    }


    private void OnGUI()
    {
      
        switch (camerastate)
        {
            case CameraStates.None:
                //SOFAR , IT DOES  PAINT SELECTION RECTANGLE ONLY IN STATE NONE
                //TODO CHECK AGAIN CAMERA STATES
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
