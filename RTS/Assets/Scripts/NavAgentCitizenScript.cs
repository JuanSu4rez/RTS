using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/**
REMOVER LA RESPONSABILIDADES COMPLEJAS EN LOS ONCOLISION

TENER METODOS FIJOS CON FUNCIONALIDADES RESPONSABILIDADES BIEN DEFINIDAS
NO TENER QUE COPIAR TRES LINEAS DE COLIDO PARA PODER REPLICAR UNA FUNCIONALIDADES

NO PONSERSE EMOS

NO RECORDARSE FOTO MONTAJES EXCEPTO SIN SON CON CAMISETAS DE SANTA FE

VERIFICAR TYPING 
 - OPCIONAL BUSCAR DONDE EL JEFE DE TOCA PLATZI DIGITA 

**/

public class NavAgentCitizenScript : MonoBehaviour, IAliveBeing, IControlable<CitizenStates>, IFigther, IWorker, IStatus, ISelectable, ITeamable, IDamagable
{
    private IGameFacade gameFacade;

    private CitizenTask citizenTask = CitizenTask.Empty;

    private Collider citizenCollider;
    private CitizenStates citizenState;


    public Resources CurrentResource { get; set; }

    [SerializeField]
    private Team team;

    public Team Team
    {
        get
        {
            return team;
        }
        set
        {
            team = value;
          
        }
    }




    public bool IsSelected { get; set; }

    private Vector3 pointToMove;
    private Vector3 pointResource;
    private float speedWalk;


    public float AttackPower { get; set; }
    public float ResourceCapacity { get; set; }
    public float DefensePower { get; set; }
    public float BuildingSpeed { get; set; }
    public float GatheringSpeed { get; set; }

    public float CurrentAmountResouce { get; set; }

    public float AttackRange { get; set; }

    private float Health;
    private float CurrentHealth;
    private float distanceTolerance;


    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    private Animator animator;   
    void Awake()
    {

    }

    // Use this for initialization
    void Start(){
        citizenCollider = this.gameObject.GetComponent<Collider>();
        citizenState = CitizenStates.Idle;
        CurrentResource = Resources.None;
        speedWalk = 0.255F;
        GatheringSpeed = 0.2F;
        ResourceCapacity = 50;
        AttackPower = 0.1F;
        DefensePower = 0.1F;
        BuildingSpeed = 0.1F;

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        navMeshObstacle = this.gameObject.GetComponent<NavMeshObstacle>();

        Health = 9999;
        CurrentHealth = Health;
        distanceTolerance = 2;

        animator = this.gameObject.GetComponent<Animator>();      

        changeColor();
        gameFacade = GameScript.GetFacade(team);
        gameFacade.AddUnit(this.gameObject, Units.Citizen);
		
	    InitChildrentTool(CitizenTransformChilden.Pick);
        InitChildrentTool(CitizenTransformChilden.Axe);		
	}
		
    private void InitChildrentTool(CitizenTransformChilden children){
        EnableChildrentTool(children, false);
    }

    private void EnableChildrentTool(CitizenTransformChilden children, bool enable){
        var id = (int)children;
        if (this.transform.childCount > id){
            this.transform.GetChild(id).gameObject.SetActive(enable);
        }
    }
	
  	public virtual void changeColor(){
        var meshColor = gameObject.GetComponent<MeshRenderer>();
        if(meshColor != null)
        Utils.ChangeColor(meshColor, team);
    }


    void OnCollisionEnter(Collision collision){
        if (collision.collider.isTrigger)
            return;

        var tag = collision.gameObject.tag;
        var name = collision.gameObject.name;
        switch (citizenState) { 
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:

                break;
            case CitizenStates.Died:
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:
                break;
            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                ////Debug.log("Collision enter");

                if (CitizenTask.IsValidCitizenTask(citizenTask) && citizenTask.IsTaskOnPorgress())
                {
                    if (CheckState(CitizenTaskStates.OnTheWay))
                    {


                        if (citizenTask.Gameobject == collision.transform.gameObject)
                        {

                            //navMeshObstacle.enabled = true;
                            //navMeshObstacle.carving = true;
                            this.transform.LookAt(citizenTask.Gameobject.transform.position);
                            citizenState = citizenTask.CitizenLabor;
                            SetDoingState();
                        }
                    }
                    else if (CheckState(CitizenTaskStates.Carrying))
                    {
                        //TODO DEPENDS ON THE REOSURCE CHEK THE TYPE OF BULDING
                        //WOOD => LumberCamp
                        //GOLD, ROCK => MiningCamp
                        //EVERYTING CAN BE DEPOSIT ON URBANCENTER
                        if (name.Equals("UrbanCenter") && gameFacade.IsMemberOfMyTeam(collision.gameObject.GetComponent<ITeamable>()))
                        {
                            ////Debug.log("Collision enter UrbanCenter")
                            if (CurrentAmountResouce > 0)
                            {
                                gameFacade.AddResources(CurrentResource, CurrentAmountResouce);
                                CurrentAmountResouce = 0;
                            }


                            if (citizenTask.IsTaskOnPorgress())
                            {
                                SetOnTheWayState();
                                SetState(CitizenStates.Walking);
                               
                                var queuecontroller = citizenTask.Gameobject.GetComponent<QueueController>();

                                if (queuecontroller != null)
                                {

                                    bool flag = false;
                                    var position = queuecontroller.GetPosition(this.gameObject, out flag);

                                    if (flag)
                                    {
                                        SetPointToMove(position);
                                      
                                    }
                                    else
                                    {
                                        Debug.Log("Recurso no recibe mas trabajadores");
                                    }

                                }
                                else
                                {

                                   
                                    SetPointToMove(citizenTask.Position);

                                }


                             
                            }
                            else
                            {
                                // todo 
                                SetState(CitizenStates.Idle);
                                ReleaseTask();
                            }



                        }
                    }


                }



                //TODO THE VALIDATION MUST BE AGAINST THE BUILDING TO BUILD
                //if (collision.gameObject.CompareTag("Building"))
                //{
                //    if (citizenLabor == CitizenStates.Building)
                //    {
                //        building = collision.gameObject.GetComponent<BuildingBehaviour>();
                //        //THIS SHOULD BE DONE BY TASK? BY THE FACADE?
                //        if(building!= null)
                //        building.InitBuilding();
                //
                //        navMeshAgent.enabled = false;
                //        citizenState = citizenLabor;
                //        
                //    }
                //}

                break;
            default:
                break;
        }
    }

    /*
    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Collisionstay  ");
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                //Debug.Log("Collisionstay  Building ");


                break;
            case CitizenStates.Died:
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:

                break;

            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                if (collision.gameObject.name.Equals("UrbanCenter"))
                {

                    //Debug.Log("Collisionstay  UrbanCenter ");


                }
                else if (collision.gameObject.name.Equals("POINTTOMOVE"))
                {
					
					//evitar esto manejarlo por update manejando un rango de toleracia
                    collision.gameObject.transform.position = Camera.main.transform.position;
                    SetState(CitizenStates.Idle);

                }
                break;
            default:
                break;
        }

        //ResourceCapacity =
    }*/

    // Update is called once per frame
    void Update(){
        switch (citizenState){
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                BuildProgress();
                break;
            case CitizenStates.Died:

                gameFacade.RemoveUnity(this.gameObject, Units.Citizen);
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:

                if (CitizenTask.IsValidCitizenTask(citizenTask)){
                    if (citizenTask.IsTaskOnPorgress()){
                        if (CurrentAmountResouce < ResourceCapacity){
                            CurrentAmountResouce += citizenTask.DiscountResource(CalculateGatheringSpeed());
                        }
                        else{
                            var go = gameFacade.FindNearBuldingToDeposit(this.transform.position, this.CurrentResource);
                            if (go != null){

								//debe ir todo en un solo metodo
                                SetCarryingState();
                                SetState(CitizenStates.Walking);
                                SetPointToMove(go.transform.position);
                                //llamar al metodo de librar la cola
                              var queue =   this.citizenTask.Gameobject.GetComponent<QueueController>();

                                queue.RelasePostion(this.gameObject);

                            }
                            else{
                                SetState(CitizenStates.Idle);
                            }
                        }
                    }
                    else{
                        //TODO find near resource of the same type , if it does not exits
                        //Idle and reset citizentask
                        SetState(CitizenStates.Idle);
                        ReleaseTask();
                    }
                }
                else{
                    //TODO find near object to deposit
                    var go = gameFacade.FindNearBuldingToDeposit(this.transform.position, this.CurrentResource);
                    if (go != null){
                        SetState(CitizenStates.Walking);
                        SetPointToMove(go.transform.position);
                    }
                    else{
                        SetState(CitizenStates.Idle);
                    }
                }
                break;
            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                float distanceToDestiny = Vector3.Distance(navMeshAgent.destination, transform.position);
                if (distanceTolerance >= distanceToDestiny)
                    SetState(CitizenStates.Idle);
                break;
            default:
                break;
        }
        setAnimation();
    }

    public virtual void setAnimation() {
        if (animator == null || this.transform.childCount <= (int)CitizenTransformChilden.Axe)
            return;
        
        //this.transform.GetChild((int)CitizenTransformChilden.Pick).gameObject.SetActive(false);
        EnableChildrentTool(CitizenTransformChilden.Pick, false);
        //this.transform.GetChild((int)CitizenTransformChilden.Axe).gameObject.SetActive(false)
        EnableChildrentTool(CitizenTransformChilden.Axe, false);
        EnableChildrentTool(CitizenTransformChilden.Hammer, false);
        EnableChildrentTool(CitizenTransformChilden.Gathered_Gold, false);
        EnableChildrentTool(CitizenTransformChilden.Gathered_Meat, false);
        EnableChildrentTool(CitizenTransformChilden.Gathered_Wood, false);
        /*
                Attacking,  //  0
                Building,   //  1
                Died,       //  2
                Escaping,   //  3
                Gathering,  //  4    
                Idle,       //  5
                None,       //  6    
                Walking ,    //  7
                Gold     ,   //  8
                Wood      ,  //  9
                CarryingGold, //  10
                CarryingWood,  //11
                CarryingMeat  //12
                Dying1,         //13
                Dying2          //14
         */
        int animationState = (int)citizenState;
        if (citizenState == CitizenStates.Walking && citizenTask.CitizenLabor == CitizenStates.Gathering && CurrentAmountResouce > 0){
            if (CurrentResource == Resources.Gold ){
                animationState = (int)CitizeAnimationStates.CarryingGold;                
                EnableChildrentTool(CitizenTransformChilden.Gathered_Gold, true);
            }
            else if (CurrentResource == Resources.Wood){
                animationState = (int)CitizeAnimationStates.CarryingWood;
                EnableChildrentTool(CitizenTransformChilden.Gathered_Wood, true);
            }
            //animationState = (int)CitizeAnimationStates.Carrying;
        }

        if (citizenState == CitizenStates.Gathering && citizenTask.CitizenLabor == CitizenStates.Gathering && CurrentResource == Resources.Gold){

           // if (gameFacade.AssetType == AssetTypes.THREED)
                EnableChildrentTool(CitizenTransformChilden.Pick, true);

            animationState = (int)CitizeAnimationStates.Gold;
        }

        if (citizenState == CitizenStates.Gathering && citizenTask.CitizenLabor == CitizenStates.Gathering && CurrentResource == Resources.Wood){

            //if (gameFacade.AssetType == AssetTypes.THREED)
                EnableChildrentTool(CitizenTransformChilden.Axe, true);

            animationState = (int)CitizeAnimationStates.Wood;
        }

        if (citizenState == CitizenStates.Building){
            EnableChildrentTool(CitizenTransformChilden.Hammer, true);
        }

        if (citizenState == CitizenStates.Died){
            System.Random random = new System.Random();           
            animationState = (int)CitizeAnimationStates.Dying1 + random.Next(1, 2);
        }

        animator.SetInteger("state", animationState);
    }
    public string GetStatus()
    {
        return citizenState.ToString() + " " + GetHealthReason() + " " + citizenTask.ToString() + " " + pointToMove;
    }

    public void SetState(CitizenStates _citizenStates)
    {

        if (_citizenStates == CitizenStates.Attacking || _citizenStates == CitizenStates.Building || _citizenStates == CitizenStates.Gathering)
        {
            this.citizenState = CitizenStates.Walking;

        }
        else
        {
            this.citizenState = _citizenStates;
        }

        if (this.citizenState == CitizenStates.Idle)
            navMeshAgent.ResetPath();

    }

    public void SetPointToMove(Vector3 newPointToMove)
    {
        //navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(newPointToMove);

    }

    public void SetPointResource(Vector3 newPointToMove)
    {
        pointResource = newPointToMove;
    }

    public void Walking()
    {
        Vector3 positionDif = pointToMove - gameObject.transform.position;
        var distance = positionDif.magnitude;
        var direction = positionDif / distance;
        gameObject.transform.position += direction * speedWalk;
    }

    private float CalculateGatheringSpeed()
    {
        var result = GatheringSpeed;
        var leftquantity = ResourceCapacity - CurrentAmountResouce;
        if (GatheringSpeed > leftquantity)
            result = leftquantity;

        return result;
    }

    private void BuildProgress()
    {

        if (CitizenTask.IsValidCitizenTask(citizenTask))
        {
            ////Debug.log("CurrentBuiltAmount  " + building.CurrentBuiltAmount);
            if (citizenTask.IsTaskOnPorgress())
                citizenTask.AddCurrentBuiltAmount(this.BuildingSpeed);
            else if (citizenTask.BuildingBehaviour.CheckState(BuildingStates.Built))
            {
                //TODO check if there is other Building to build
                //set task ontheway if exits otherwise set idle
                SetState(CitizenStates.Idle);
            }
        }
    }



    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public float GetHealth()
    {
        return Health;
    }

    public float GetHealthReason()
    {
        return CurrentHealth / Health;
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void AddDamage(float damage)
    {
        if (IsAlive())
        {
            if (damage > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
                CurrentHealth = (int)(CurrentHealth - damage);
        }
        if (CurrentHealth <= 0)
        {
            citizenState = CitizenStates.Died;
            Destroy(gameObject, 3);
        }

    }

    public void ReleaseTask()
    {
        if (this.citizenTask != CitizenTask.Empty)
            this.citizenTask = CitizenTask.Empty;
    }

    public void SetCitizenTask(CitizenTask _citizenTask)
    {
        if (_citizenTask != CitizenTask.Empty)
        {
            this.citizenTask = _citizenTask;
            SetOnTheWayState();
        }
    }


    private CitizenTaskStates CitizenTaskState = CitizenTaskStates._None;

    public void SetOnTheWayState()
    {
        CitizenTaskState = CitizenTaskStates.OnTheWay;
    }

    public void SetDoingState()
    {
        CitizenTaskState = CitizenTaskStates.Doing;
        navMeshAgent.ResetPath();
    }

    public void SetCarryingState()
    {
        CitizenTaskState = CitizenTaskStates.Carrying;
    }

    public bool CheckState(CitizenTaskStates state)
    {
        return CitizenTaskState == state;
    }
}
