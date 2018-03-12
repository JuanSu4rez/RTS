using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavAgentCitizenScript : MonoBehaviour, IAliveBeing, IFigther, IWorker, IStatus
{
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
    public float CurrentAmountResouce { get; set; }


    private float Health;
    private float CurrentHealth;


    public NavMeshAgent navMeshAgent;

    void Awake()
    {

    }

        // Use this for initialization
    void Start()
    {
        //Debug.log("citizenCollider " + citizenCollider != null);
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

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        Health= 9999;
        CurrentHealth =Health;

        gameFacade = GameScript.GetFacade();
    }




    void OnCollisionEnter(Collision collision)
    {
        //Debug.log("Colision " + pointResource);
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
                if (name.Equals("GoldMine") || name.Equals("Forest"))
                {
                  
                    navMeshAgent.enabled = false;
                    if (citizenLabor ==  CitizenStates.Gathering)
                    {
                     
                        citizenState = citizenLabor;
                        resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    }

                }
                else if (name.Equals("UrbanCenter"))
                {
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
                if (collision.gameObject.CompareTag("Building"))
                {
                    if (citizenLabor == CitizenStates.Building)
                    {
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

        //ResourceCapacity =
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.log(pointToMove);
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

        AddDamage(10);
    }

    public string GetStatus()
    {
        return citizenState.ToString() + " "+GetHealthReason()  +" "+ citizenLabor.ToString() + " " + pointToMove;
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
            Destroy(gameObject, 1);
        }
    }

}
