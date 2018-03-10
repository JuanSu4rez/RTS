using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavAgentCitizenScript : MonoBehaviour, IAliveBeing, IFigther, IWorker, IStatus {
    private IGameFacade gameFacade;

    private Collider citizenCollider;
    private CitizenStates citizenState;
    private CitizenStates citizenLabor;
    private BuildingBehaviour building;

    private Vector3 pointToMove;
    private Vector3 pointResource;
    private float speedWalk;
    private ResourceScript resourceTemp = null;    
    public float AttackPower { get; set; }
    public float ResourceCapacity { get; set; }
    public float DefensePower { get; set; }
    public float BuildingSpeed { get; set; }
    public float GatheringSpeed { get; set; }
    public Resources CurrentResource { get; set; }
    private int Health;
    private int CurrentHealth;
    public float CurrentAmountResouce { get; set; }

    public NavMeshAgent navMeshAgent;
    private Animator animator;   

    // Use this for initialization
    void Start()
    {
        gameFacade = GameScript.GetFacade();        

        citizenCollider = this.gameObject.GetComponent<Collider>();
        citizenState = CitizenStates.Idle;
        CurrentResource = Resources.None;
        speedWalk = 0.255F;
        GatheringSpeed = 0.2F;
        ResourceCapacity = 50;
        AttackPower = 0.1F;
        DefensePower = 0.1F;
        BuildingSpeed = 0.1F;
        citizenLabor = CitizenStates.None;
        Health = 9999;
        CurrentHealth = Health;

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        //Debug.Log(this.GetType().FullName+" FACDE "+ gameFacade != null +" "+gameFacade.ToString());
        Debug.Log("0 ");
        Debug.Log("1 " + this.GetType().FullName + " FACDE " + gameFacade != null );

        // if (gameFacade.AssetType == AssetTypes.THREED )lll
        // {
        //     Debug.Log(this.gameObject.transform.childCount);
        //     Debug.Log(this.transform.GetChild(2).name);
        //     this.transform.GetChild(2).gameObject.SetActive(false);           
        // }

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colision -----------------------------------" + pointResource);
        var name = collision.gameObject.name;
        var tag = collision.gameObject.tag;
        switch (citizenState)
        {
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
                //Debug.log("Collision enter");

                //TODO THE VALIDATION MUST BE AGAINST THE MATERIAL TO CRAFT
                if (name.Equals("GoldMine") || name.Equals("Forest")){
                    navMeshAgent.enabled = false;
                    if (citizenLabor ==  CitizenStates.Gathering){
                        citizenState = citizenLabor;
                        resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    }
                }
                else if (name.Equals("UrbanCenter")) {
                    //Debug.log("Collision enter UrbanCenter");                   
                    if (citizenLabor != CitizenStates.None){
                        if (CurrentAmountResouce > 0){
                            GameScript.GetFacade().AddResources(CurrentResource, CurrentAmountResouce);
                            CurrentAmountResouce = 0;
                        }
                        // pointToMove = pointResource;
                        SetPointToMove(pointResource);
                    }
                }

                //TODO THE VALIDATION MUST BE AGAINST THE BUILDING TO BUILD
                if (collision.gameObject.CompareTag("Building")) {
                    if (citizenLabor == CitizenStates.Building){
                        building = collision.gameObject.GetComponent<BuildingBehaviour>();
                        //THIS SHOULD BE DONE BY TASK? BY THE FACADE?
                        if(building!= null)
                        building.InitBuilding();

                        navMeshAgent.enabled = false;
                        citizenState = citizenLabor;
                    }
                }

                break;
            default:
                break;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.log("Collisionstay  " + pointResource);
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                if (collision.gameObject.name.Equals("Building"))
                {

           
                   
                }

                break;
            case CitizenStates.Died:
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:
                if (resourceTemp == null)
                {
                    resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    //pointToMove = pointResource;
                    SetPointToMove(pointResource);

                }
                break;

            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                if (collision.gameObject.name.Equals("UrbanCenter"))
                {
                    //Debug.log("Collisionstay  UrbanCenter");
                    if (citizenLabor != CitizenStates.None)
                    {
                        //TODO  ahumentar recursos al jugador
                        if (CurrentAmountResouce > 0){
                            CurrentAmountResouce = 0;
                        }
                        //pointToMove = pointResource;
                        SetPointToMove(pointResource);
                    }

                }
                else if (collision.gameObject.name.Equals("POINTTOMOVE"))
                {
                    collision.gameObject.transform.position = Camera.main.transform.position;
                    SetState(CitizenStates.Idle);
                    navMeshAgent.enabled = false;
                }
                break;
            default:
                break;
        }        
    }

    // Update is called once per frame
    void Update()
    {       
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                BuildProgress();
                break;
            case CitizenStates.Died:                
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:


                if (resourceTemp != null)
                {
                    if (resourceTemp.HasResource() && CurrentAmountResouce < ResourceCapacity)
                    {

                        //TODO Descontar del recurso con el que se colisiona
                        CurrentAmountResouce += resourceTemp.DiscountAmount(CalculateGatheringSpeed());

                    }
                    else if (!resourceTemp.HasResource())
                    {
                        SetState(CitizenStates.Idle);
                        //TODO buscar recurso CERCANO del mapa explorado y construido
                    }
                    else
                    {
                        citizenState = CitizenStates.Walking;
                        citizenLabor = CitizenStates.Gathering;
                        //TODO CALCULAR EL PUNTO MAS CERCANO A DEPOSITAR
                        pointToMove = new Vector3(0, 1, 0);
                        SetPointToMove(pointToMove);
                    }
                }
                break;
            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                //Walking();
                break;
            default:
                break;
        }
        setAnimation();
        AddDamage(1);
    }

    public void setAnimation() {
        if (animator == null )
            return;
        this.transform.GetChild(2).gameObject.SetActive(false);
        //Attacking,    0
        //Building,     1
        //Died,         2
        //Escaping,     3
        //Gathering,    4    
        //Idle,         5
        //None,         6    
        //Walking       7
        //Gold          8
        //Wood          9
        //Carrying      10

        int animationState = (int)citizenState;
        if (citizenState == CitizenStates.Walking && citizenLabor == CitizenStates.Gathering && CurrentAmountResouce > 0){
            animationState = 10;
        }

        if (citizenState == CitizenStates.Gathering && citizenLabor == CitizenStates.Gathering && CurrentResource == Resources.Gold){
            if (gameFacade.AssetType == AssetTypes.THREED)            
                this.transform.GetChild(2).gameObject.SetActive(true);
            animationState = 8;            
        }

        if (citizenState == CitizenStates.Gathering && citizenLabor == CitizenStates.Gathering && CurrentResource == Resources.Wood){
            animationState = 9;
        }

        animator.SetInteger("state", animationState);
    }

    public string GetStatus()
    {
        return "GetCurrentHealth [reason =  " + GetHealthReason() +"] " + citizenState.ToString() + " " + citizenLabor.ToString() + " " + pointToMove;
    }

    public void SetState(CitizenStates _citizenStates)
    {

        if (_citizenStates == CitizenStates.Attacking || _citizenStates == CitizenStates.Building || _citizenStates == CitizenStates.Gathering)
        {
            this.citizenState = CitizenStates.Walking;
            this.citizenLabor = _citizenStates;
        }
        else
        {
            this.citizenState = _citizenStates;
            this.citizenLabor = CitizenStates.None;
        }
    }

    public void SetPointToMove(Vector3 newPointToMove)
    {
        navMeshAgent.enabled = true;
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
        if (building != null)
        {
            //Debug.log("CurrentBuiltAmount  " + building.CurrentBuiltAmount);
            if (building.IsBulding())
                building.AddCurrentBuiltAmount(this.BuildingSpeed);
            else if (building.CheckState(BuildingStates.Built))
            {
                //TODO check if there is other Building to build
                SetState(CitizenStates.Idle);
            }
        }
    }

    public int GetCurrentHealth(){
        return CurrentHealth;        
    }

    public int GetHealth(){
        return Health;
    }

    public void AddDamage(float damage){
        if (IsAlive())
        {
            if (damage > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
                CurrentHealth = (int)(CurrentHealth-damage);
        }
        if (CurrentHealth <= 0){
            citizenState = CitizenStates.Died;
            Destroy(gameObject, 1);
        }
    }

    public bool IsAlive()    {
        return CurrentHealth > 0;
    }

    public float GetHealthReason()
    {
        return GetCurrentHealth() / GetHealth();
    }
}
