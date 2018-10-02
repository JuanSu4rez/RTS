using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class was used at the begining 
 * So far is Obsolete
[System.Obsolete]
public class CitizenScript : MonoBehaviour, IAliveBeing, IFigther, IWorker, IStatus
{
    private IGameFacade gameFacade;

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
        ////Debug.log("citizenCollider " + citizenCollider != null);
        citizenState = CitizenStates.Idle;
        CurrentResource = Resources.None;
        speedWalk = 0.255F;
        GatheringSpeed = 0.2F;
        ResourceCapacity = 50;
        AttackPower = 0.1F;
        DefensePower = 0.1F;
        BuildingSpeed = 0.1F;
        citizenLabor = CitizenStates.None;
        gameFacade = GameScript.GetFacade();
    }

    void OnCollisionEnter(Collision collision)
    {
        ////Debug.log("Colision " + pointResource);
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
                ////Debug.log("Collision enter");


                if (name.Equals("GoldMine") || name.Equals("Forest"))
                {
                    if (citizenLabor != CitizenStates.None)
                    {

                        citizenState = citizenLabor;
                        resourceTemp = collision.gameObject.GetComponent<ResourceScript>();
                    }

                }
                else if (name.Equals("UrbanCenter"))
                {
                    ////Debug.log("Collision enter UrbanCenter");
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
        ////Debug.log("Collisionstay  " + pointResource);
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                if (collision.gameObject.name.Equals("Building"))
                {

           
                     var aux = collision.gameObject.GetComponent<BuildingBehaviour>();

                     ////Debug.log("CurrentBuiltAmount  " + aux.CurrentBuiltAmount);
                     if (aux.IsBulding())
                         aux.AddCurrentBuiltAmount(this.BuildingSpeed);
                     else if (aux.CheckState(BuildingStates.Built))
                     {
                         //TODO check if there is other Building to build
                         SetState(CitizenStates.Idle);
                     }
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
                    ////Debug.log("Collisionstay  UrbanCenter");
                    if (citizenLabor != CitizenStates.None){
                        gameFacade.AddResources(CurrentResource, CurrentAmountResouce);
                        if (CurrentAmountResouce > 0) {
                            CurrentAmountResouce = 0;
                        }
                        pointToMove = pointResource;
                    }

                }
                else if (collision.gameObject.name.Equals("POINTTOMOVE"))
                {
                    collision.gameObject.transform.position = Camera.main.transform.position;
                    SetState(CitizenStates.Idle);
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
        ////Debug.log(pointToMove);
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

                        //TODO Discount the resource
                        CurrentAmountResouce += resourceTemp.DiscountAmount(CalculateGatheringSpeed());

                    }
                    else if (!resourceTemp.HasResource())
                    {
                        SetState(CitizenStates.Idle);
                        //TODO Find the near resource to gather
                    }
                    else
                    {
                        citizenState = CitizenStates.Walking;
                        citizenLabor = CitizenStates.Gathering;
                        //TODO Find the near point to deposit
                        //just do one time by assining to the labor
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
        return citizenState.ToString() + " " + citizenLabor.ToString() + " " + pointToMove;
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

    bool IAliveBeing.IsAlive()
    {
        throw new System.NotImplementedException();
    }

    public float GetCurrentHealth()
    {
        throw new System.NotImplementedException();
    }

    public float GetHealth()
    {
        throw new System.NotImplementedException();
    }

    public float GetHealthReason()
    {
        throw new System.NotImplementedException();
    }
}
*/