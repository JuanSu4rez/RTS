using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CitizenScript : MonoBehaviour, IAliveBeing, IFigther, IWorker, IStatus
{

    private Collider citizenCollider;
    private CitizenStates citizenState;
    private CitizenStates citizenLabor;
    private Vector3 pointToMove;
    private Vector3 pointResource;
    private float speedWalk;
    private ResourceScript resourceTemp = null;
    public bool IsAlive { get; set; }
    public float AttackPower { get; set; }
    public float ResourceCapacity { get; set; }
    public float DefensePower { get; set; }
    public float BuildingSpeed { get; set; }
    public float GatheringSpeed { get; set; }
    public Resources CurrentResource { get; set; }
    public float Life { get; set; }
    public float CurrentAmountResouce { get; set; }

    // Use this for initialization
    void Start()
    {
        citizenCollider = transform.GetComponent<Collider>();
        Debug.Log("citizenCollider " + citizenCollider != null);
        citizenState = CitizenStates.Idle;
        CurrentResource = Resources.None;
        speedWalk = 0.255F;
        GatheringSpeed = 0.2F;
        ResourceCapacity = 50;
        AttackPower = 0.1F;
        DefensePower = 0.1F;
        BuildingSpeed = 0.1F;
        citizenLabor = CitizenStates.None;

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision "+ pointResource);
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
                Debug.Log("Collision enter");
           

                if (name.Equals("GoldMine") || name.Equals("Forest"))
                {
                    if (citizenLabor != CitizenStates.None) {

                        citizenState = citizenLabor;
                        resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    }
                       
                }
                else if (name.Equals("UrbanCenter") )
                {
                    Debug.Log("Collision enter UrbanCenter");
                    if (citizenLabor != CitizenStates.None)
                    {
                       
                        if (CurrentAmountResouce > 0)
                        { 
                            //TODO add resources to the player
                            CurrentAmountResouce = 0;
                        }
                      
                     
                        pointToMove = pointResource;
                      

                    }

                }

                if (collision.gameObject.CompareTag("Building"))
                {
                    if (citizenLabor != CitizenStates.None)
                    {

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
        Debug.Log("Collisionstay  "+ pointResource);
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                var aux = collision.gameObject.GetComponent<BuildingBehaviour>();
                Debug.Log("CurrentBuiltAmount  " + aux.CurrentBuiltAmount);
                if (aux.IsBulding())
                    aux.AddCurrentBuiltAmount(this.BuildingSpeed);
                else if (aux.CheckState(BuildingStates.Built))
                {
                    //TODO check if there is other Building to build
                    SetState(CitizenStates.Idle);
                }
                   
                break;
            case CitizenStates.Died:
                break;
            case CitizenStates.Escaping:
                break;
            case CitizenStates.Gathering:
                if (resourceTemp == null){
                    resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    pointToMove = pointResource;
                }
                break;

            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                if (collision.gameObject.name.Equals("UrbanCenter"))
                {
                    Debug.Log("Collisionstay  UrbanCenter");
                    if (citizenLabor != CitizenStates.None)
                    {
                        //TODO  ahumentar recursos al jugador
                        if (CurrentAmountResouce > 0)
                        {
                            CurrentAmountResouce = 0;
                        }                                            
                        pointToMove = pointResource;

                    }

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
        Debug.Log(pointToMove);
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

               
                if (resourceTemp != null)
                {
                    if (resourceTemp.HasResource() && CurrentAmountResouce < ResourceCapacity)
                    {

                        //TODO Descontar del recurso con el que se colisiona
                        CurrentAmountResouce += resourceTemp.DiscountAmount(CalculateGatheringSpeed());

                    } else if (!resourceTemp.HasResource()) {
                        SetState(CitizenStates.Idle);
                        //TODO buscar recurso CERCANO del mapa explorado y construido
                    }
                    else
                    {
                        citizenState = CitizenStates.Walking;
                        citizenLabor = CitizenStates.Gathering;
                        //TODO CALCULAR EL PUNTO MAS CERCANO A DEPOSITAR
                        pointToMove = new Vector3(0, 1, 0);
                    }
                }
                break;
            case CitizenStates.Idle:
                break;
            case CitizenStates.None:
                break;
            case CitizenStates.Walking:
                Walking();
                break;
            default:
                break;
        }
    }

    public string GetStatus()
    {
        return citizenState.ToString()+" " + citizenLabor.ToString() + " " + pointToMove;
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
        pointToMove = newPointToMove;
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
}
