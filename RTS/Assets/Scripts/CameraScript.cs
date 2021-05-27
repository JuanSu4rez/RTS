using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour {
    private Vector3 _mouseposition;
    //BoxSelection
    private Texture2D selectionTexture = null;

    private Texture2D selectionBorder = null;

    public static Rect selection = new Rect(0, 0, 0, 0);

    private readonly Vector2 empty = new Vector2(-10, -10);

    public static Vector2 firstclick;

    public static Vector2 secondclick;

    private CameraSelectionTypes cameraSelectionType = CameraSelectionTypes._None;

    private ArrayList currentSelecteds;

    public ArrayList objectsInScene;

    private GameObject currentSelected;


    private CameraStates camerastate;

    public GameObject CurrentSelected {
        get {
            return currentSelected;
        }
    }

    [SerializeField]
    private Plane plane;

    //private GameObject pointtomove;



    private NonGUI nonGUI;
    private BuildingGui buildingGui;
    private MilitaryGui militaryGui;
    private UnitsGui unitsGui;

    private IGui currentGui = null;

    [SerializeField]
    private GameObject buildingValidator;

    public static GameAreasManager gameareas;

    public bool DebugAreas = false;



    // Use this for initialization
    void Start() {
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


        nonGUI = ScriptableObject.CreateInstance<NonGUI>();
        currentGui = nonGUI;

        unitsGui = ScriptableObject.CreateInstance<UnitsGui>();
        unitsGui.SetBuildingValidator(buildingValidator);

        buildingGui = ScriptableObject.CreateInstance<BuildingGui>();
        militaryGui = ScriptableObject.CreateInstance<MilitaryGui>();

        gameareas = ScriptableObject.CreateInstance<GameAreasManager>();
        gameareas.Init();

        #region point to move
        /*
        pointtomove = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointtomove.transform.position = Camera.main.transform.position;
        pointtomove.transform.localScale = new Vector3(1, 1, 1);
        pointtomove.GetComponent<Renderer>().enabled = false;
        pointtomove.name = "POINTTOMOVE";
        pointtomove.tag = "Land";*/
        #endregion

        objectsInScene = new ArrayList(20);
        currentSelecteds = new ArrayList(20);
        GetObjectsinScene();

    }



    public void GetObjectsinScene() {

        //TODO FILTER BY TEAM
        GameObject[] allSelectableObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        for (int i = 0; i < allSelectableObjects.Length; i++) {
            ISelectable tempSel = allSelectableObjects[i].GetComponent<ISelectable>();
            if (tempSel != null) {
                objectsInScene.Add(allSelectableObjects[i]);
            }
        }
    }

    public static Vector2 get2sCoordinates(GameObject selectable) {
        if (selectable == null)
            return Vector2.zero;

        Transform transform = selectable.GetComponent<Transform>();
        Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
        return new Vector2(v.x, Screen.height - v.y);
    }

    // Update is called once per frame
    void Update() {
        gameareas.RecalculateAreas(camerastate);
        //if the player is not putting a building or something
        if (!currentGui.HasOptionSelected())
            userClick();


        switch (camerastate) {
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

    private void userClick() {
        Vector3 _onscreenmouseposition = Input.mousePosition;
        _onscreenmouseposition.y = Screen.height - Input.mousePosition.y;

        _mouseposition = Input.mousePosition;

        if (camerastate != CameraStates.None) {
            if (_onscreenmouseposition.y >= gameareas.GameArea.height) {
                if (firstclick == empty)
                    return;
                else
                    _mouseposition.y = gameareas.GuiArea.height;
            }
        }

        if (Input.GetMouseButtonDown(0) &&
            //todo temporal solution
            !unitsGui.HasOptionSelected() && firstclick == empty) {
            secondclick = empty;
            firstclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 initialPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return;
        }
        else if (Input.GetMouseButtonUp(0)) {
            /*
            if (firstclick != empty) {
                if (currentSelecteds.Count > 0) {
                    ////Debug.Log("Camera state " + camerastate);
                    //TODO VALIDATE WITH SUAREZ
                    //temporal counter
                    int citizens = 0, militar = 0, buildings = 0;
                    for (int i = 0; i < currentSelecteds.Count; i++) {
                        var obj = ((GameObject)currentSelecteds[i]);

                        //obj.gameObject.GetComponent<ISelectable>().IsSelected = true;
                        //todo temporal
                        if (obj.GetComponent<NavAgentCitizenScript>() != null) {
                            citizens++;
                        }
                        else if (obj.GetComponent<BuildingBehaviour>() != null) {
                            buildings++;
                        }
                        else {
                            militar++;
                        }

                    }

                    //todo temporal
                    camerastate = CameraStates.UnitsSelection;

                    if (citizens > 0 && militar == 0 && buildings == 0) {
                        SetSelectedGameObject((GameObject)currentSelecteds[0]);
                        cameraSelectionType = CameraSelectionTypes.Citizen;
                        currentGui = this.unitsGui;
                    }
                    else if (militar > 0 && citizens >= 0 && buildings == 0) {
                        SetSelectedGameObject((GameObject)currentSelecteds[0]);
                        cameraSelectionType = CameraSelectionTypes.Military;
                        currentGui = this.militaryGui;
                    }
                    else if (buildings > 0) {
                        camerastate = CameraStates.None;
                        cameraSelectionType = CameraSelectionTypes._None;
                        currentGui = this.nonGUI;
                    }

                }
                else {

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(firstclick);

                    if (Physics.Raycast(ray, out hit)) {
                        var _name = hit.transform.gameObject.name;
                        //TRANSATITION TO UnitsSelection OR BuildingsSelection
                        var hitselected = hit.transform.gameObject;
                        SetSelectedGameObject(hitselected);
                        if (currentSelected != null) {
                            var _tag = currentSelected.tag;
                            switch (_tag) {

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
                                    currentGui = nonGUI;
                                    break;

                            }

                        }
                    }
                }

                firstclick = empty;
                secondclick = empty;
            }
            */
            firstclick = empty;
            secondclick = empty;
        }

        if (Input.GetMouseButton(0) && firstclick != empty) {

            secondclick = _mouseposition;// Input.mousePosition;

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
            /*
            //TODO SELECT ALL THE GAMEOBJECTS THAT BELONG TO YOUR TEAM ON THE AREA
            //TODO FILTER BY TEAM
            if (selection.width != 0 && selection.height != 0) {
                currentSelecteds.RemoveRange(0, currentSelecteds.Count);
                for (int index = 0; index < objectsInScene.Count; index++) {
                    GameObject obj = ((GameObject)objectsInScene[index]);

                    if (obj == null)
                        continue;

                    var iselectable = obj.gameObject.GetComponent<ISelectable>();

                    if (iselectable == null)
                        continue;

                    if (selection.Contains(get2sCoordinates(obj))) {
                        currentSelecteds.Add(obj);
                       // iselectable.IsSelected = true;
                    }
                    else {

                        currentSelecteds.Remove(obj);
                        //iselectable.IsSelected = false;
                    }
                }
                */
            }
            else if (currentSelecteds.Count > 0) {

               // ClearSelection();
            }
        //}
    }

    private void ClearSelection() {
        for (int index = 0; index < currentSelecteds.Count; index++) {
            GameObject obj = ((GameObject)currentSelecteds[index]);
            obj.gameObject.GetComponent<ISelectable>().IsSelected = false;

        }
        currentSelecteds.RemoveRange(0, currentSelecteds.Count);
    }


    private void SetSelectedGameObject(GameObject hitselected) {
        if (hitselected != null) {
            var auxst = hitselected.GetComponent<ISelectable>();
            if (auxst != null)
                auxst.IsSelected = true;

            if (currentSelected != null) {
                auxst = currentSelected.GetComponent<ISelectable>();
                if (auxst != null)
                    auxst.IsSelected = false;
            }
            currentSelected = hitselected;
        }
    }

    private void ClickActionsUnits() {
        //TODO TEMPORAL SOLUTION
        //TO AVOID CLICKING ON GUI AREA WHEN YOU HAVE A SELECTION
        Vector3 _onscreenmouseposition = Input.mousePosition;
        _onscreenmouseposition.y = Screen.height - Input.mousePosition.y;
        if (_onscreenmouseposition.y >= gameareas.GameArea.height) {
            return;
        }

        if (currentSelected == null)
            return;

        switch (cameraSelectionType) {
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

    private void PeopleAction() {

    }

    private void BuldingAction() {

    }

    private void MilitaryAction() {
        var soldierTemp = currentSelected.gameObject.GetComponent<IControlable_v1<SoldierStates>>();

        if (soldierTemp == null) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            //when a citizen is selected
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                string rightclickedObj = hit.transform.gameObject.name;
                switch (rightclickedObj) {
                    case "Land":
                      
                        //pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                        soldierTemp.SetState(SoldierStates.Walking);
                        soldierTemp.SetPointToMove(new Vector3(hit.point.x, 1, hit.point.z));
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

    private void CitizenAction() {
        #region implementatioforunicontroller
        var unitController = currentSelected.gameObject.GetComponent<UnitController>();

        if (unitController != null) {



            if (Input.GetMouseButtonDown(1)) {
                //when a citizen is selected
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {


                    string rightclickedObj = hit.transform.gameObject.tag;

                    var iselectable = currentSelected.GetComponent<ISelectable>();

                    if (iselectable != null)
                        iselectable.IsSelected = true;

                    //se le asigna un pinto con una altura y+1
                    var point = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);

                    switch (rightclickedObj) {
                        case "Land":
                            //esto no funciona por que cuando camina de la cola a la mina la tarea es 
                            //moverse y el action esta pendiente
                            unitController.ReleaseTask();
                            unitController.Move(point,null);


                            break;
                        case "Gold":


                            //si mi task actual es recojer oro
                            Task currenttask = unitController.GetTask<Task>();

                            CompositeTask cmp = (currenttask as CompositeTask);
                            
                            GatheringTask gthtask = (currenttask as GatheringTask);

                            if (gthtask != null && gthtask.resourceType == Resources.Gold) {

                                ////Debug.Log("Citizen is already Gathering gold");
                                return;

                            }

                            if (cmp != null) {
                                if (cmp.task is GatheringTask) {
                                    gthtask = cmp.task as GatheringTask;
                                }
                            }

                            if (gthtask != null && gthtask.resourceType == Resources.Gold) {

                                ////Debug.Log("Citizen is already Gathering gold");
                                return;

                            }







                            var queuecontroller = hit.transform.gameObject.GetComponent<QueueController>();

                            if (queuecontroller != null) {

                                int flag = -1;

                                GameObject obj = unitController.gameObject;

                                var buildinggameobject = GameScript.FindResoruceBuidingToDeposit(0, Resources.Gold, unitController.transform.position);

                                if (buildinggameobject == null) {
                                    ////Debug.Log("!No hay edificio para depositar no se asigna la tarea");
                                    return;
                                }

                                var position = queuecontroller.GetPosition(ref obj, out flag);

                                var resource = hit.transform.gameObject.GetComponent<ResourceScript>();

                                if (flag >= 0) {



                                    GatheringTask gatheringtask = new GatheringTask();

                                    gatheringtask.onwait = flag == 0;
                                    gatheringtask.resourceType = Resources.Gold;
                                    gatheringtask.positionBuldingtodeposit = Utils.PositionSubHalfBounsdssizeXZ(buildinggameobject);
                                    gatheringtask.position = position;
                                    gatheringtask.Gatheringspeed = 0.1f;
                                    gatheringtask.MaxCapacity = 50;
                                    gatheringtask.CurrentAmountResouce = 0;
                                    gatheringtask.resourcescript = resource;

                                    //antes de mover se hace un realease del task actual
                                    unitController.ReleaseTask();
                                    unitController.Move(position, gatheringtask,
                                    //accion una ves llega al puesto    
                                        () => {
                                            //todo que pasa si es null
                                            unitController.SetTask(gatheringtask);
                                        }
                                    );

                                }
                                else {
                                    ////Debug.Log("Recurso no recibe mas trabajadores");
                                }

                            }
                            else {
                                ////Debug.Log("No existe queue controller");
                            }


                            break;
                        case "Forest":


                            break;

                        case "Rock":


                            break;

                        case "Building":


                            break;

                        default:
                            break;
                    }





                }
            }




        }
        #endregion

        var citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();

        if (citizenTemp == null) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            //when a citizen is selected
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                CitizenTask citizenTask = CitizenTask.Empty;

                string rightclickedObj = hit.transform.gameObject.tag;
                switch (rightclickedObj) {
                    case "Land":
                       // pointtomove.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
                        citizenTemp.SetPointToMove(new Vector3(hit.point.x, 1, hit.point.z));
                        citizenTemp.SetState(CitizenStates.Walking);
                        break;
                    case "Gold":
                        citizenTemp = currentSelected.gameObject.GetComponent<NavAgentCitizenScript>();


                        var queuecontroller = hit.transform.gameObject.GetComponent<QueueController>();

                        if (queuecontroller != null) {

                            int flag = -1;
                            var go = citizenTemp.gameObject;
                            var position = queuecontroller.GetPosition(ref go, out flag);

                            if (flag >= 0) {
                                citizenTemp.SetPointToMove(position);
                                citizenTemp.SetPointResource(hit.transform.position);
                            }
                            else {
                                ////Debug.Log("Recurso no recibe mas trabajadores");
                            }

                        }
                        else {

                            citizenTemp.SetPointToMove(hit.transform.position);
                            citizenTemp.SetPointResource(hit.transform.position);

                        }

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

                if (citizenTask != CitizenTask.Empty)
                    citizenTemp.SetCitizenTask(citizenTask);


            }
        }
    }


    void OnGUI() {
        var originalcolor = GUI.color;

        var originalcolorback = GUI.backgroundColor;

        if (DebugAreas) {
            /*
            Color coloraux = Color.red;
            GUI.color = coloraux;
            coloraux.a = 0.3f;
            GUI.backgroundColor = coloraux;
            GUI.Box(gameareas.GameArea, "");*/


            GUI.color = Color.blue;

            GUI.backgroundColor = Color.blue;
            GUI.Box(gameareas.GuiArea, "");
        }

        GUI.color = originalcolor;
        GUI.backgroundColor = originalcolorback;



        switch (camerastate) {
            case CameraStates.None:

                if (Input.GetMouseButton(0) && firstclick != empty && secondclick != empty) {
                    GUI.DrawTexture(selection, selectionTexture);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, selection.width, 1), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x + selection.width, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y + selection.height, selection.width, 1), selectionBorder);
                }



                break;
            default:



                if (Input.GetMouseButton(0) && firstclick != empty && secondclick != empty) {
                    GUI.DrawTexture(selection, selectionTexture);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, selection.width, 1), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x + selection.width, selection.y, 1, selection.height), selectionBorder);
                    GUI.DrawTexture(new Rect(selection.x, selection.y + selection.height, selection.width, 1), selectionBorder);
                }

                if (currentGui != null)
                    currentGui.ShowGUI(currentSelected);

              

                break;
        }

 
    }


}
