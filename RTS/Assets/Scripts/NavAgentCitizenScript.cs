using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavAgentCitizenScript : MonoBehaviour, IAliveBeing, IControlable<CitizenStates>, IFigther, IWorker, IStatus, ISelectable, ITeamable, IDamagable
{
    private IGameFacade gameFacade;

    private CitizenTask citizenTask = CitizenTask.Empty;

    private Collider citizenCollider;
    private CitizenStates citizenState;

  
    public Resources CurrentResource { get; set; }

    [SerializeField]
    private Team team;

    public Team Team {
        get
        {
            return team;
        }
        set
        {
            team = value;
            gameFacade = GameScript.GetFacade(team);
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
      

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        Health= 9999;
        CurrentHealth =Health;

       
        gameFacade = GameScript.GetFacade(team);

        changeColor();

        gameFacade.AddUnity(this.gameObject, Units.Citizen);
    }

    public virtual void changeColor()
    {
        Utils.ChangeColor(gameObject.GetComponent<MeshRenderer>(), team);
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger)
            return;
     
       
        var tag = collision.gameObject.tag;
        var name = collision.gameObject.name;
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
             
                if(CitizenTask.IsValidCitizenTask( citizenTask) && citizenTask.IsTaskOnPorgress())
                {
                    if (citizenTask.CheckState(CitizenTaskStates.OnTheWay))
                    {


                        if (citizenTask.Gameobject == collision.transform.gameObject)
                        {
                            navMeshAgent.enabled = false;
                            citizenState = citizenTask.CitizenLabor;
                            citizenTask.SetDoingState();
                        }
                    }
                    else if (  citizenTask.CheckState(CitizenTaskStates.Carrying) )
                    {

                        if (name.Equals("UrbanCenter") && gameFacade.IsMemberOfMyTeam(collision.gameObject.GetComponent<ITeamable>()))
                        {
                            //Debug.log("Collision enter UrbanCenter")
                            if (CurrentAmountResouce > 0)
                            {
                                gameFacade.AddResources(CurrentResource, CurrentAmountResouce);
                                CurrentAmountResouce = 0;
                            }

                          
                                if (citizenTask.IsTaskOnPorgress())
                                {
                                    citizenTask.SetOnTheWayState();
                                    SetState(CitizenStates.Walking);
                                    SetPointToMove(citizenTask.Position);
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

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collisionstay  " );
        switch (citizenState)
        {
            case CitizenStates.Attacking:
                break;
            case CitizenStates.Building:
                Debug.Log("Collisionstay  Building ");
                

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

                    Debug.Log("Collisionstay  UrbanCenter " );


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
        //Debug.log(pointToMove);
        switch (citizenState)
        {
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

                if (CitizenTask.IsValidCitizenTask(citizenTask) )
                {
                    if (citizenTask.IsTaskOnPorgress())
                    {
                        if (CurrentAmountResouce < ResourceCapacity)
                        {
                            CurrentAmountResouce += citizenTask.DiscountResource(CalculateGatheringSpeed());
                        }
                        else
                        {
                            
                            //TODO find near object to deposit
                            var go=  gameFacade.FindNearBuldingToDeposit(this.transform.position, this.CurrentResource);
                            if(go!= null)
                            {
                                citizenTask.SetCarryingState();
                                SetState(CitizenStates.Walking);
                                SetPointToMove(go.transform.position);
                            }
                            else
                            {
                                SetState(CitizenStates.Idle);
                            }

                        }
                    }
                    else
                    {
                        //TODO find near resource of the same type , if it does not exits
                        //Idle and reset citizentask
                        SetState(CitizenStates.Idle);
                        ReleaseTask();
                    }

                }
                else
                {

                  
                    //TODO find near object to deposit
                    var go = gameFacade.FindNearBuldingToDeposit(this.transform.position, this.CurrentResource);
                    if (go != null)
                    {
                        SetState(CitizenStates.Walking);
                        SetPointToMove(go.transform.position);
                    }
                    else
                    {
                        SetState(CitizenStates.Idle);
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

        //AddDamage(10);
    }

    public string GetStatus()
    {
        return citizenState.ToString() + " "+GetHealthReason()  +" "+ citizenTask.ToString() + " " + pointToMove;
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
            navMeshAgent.enabled = false;

       
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

        if (CitizenTask.IsValidCitizenTask( citizenTask))
        {
            //Debug.log("CurrentBuiltAmount  " + building.CurrentBuiltAmount);
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
            Destroy(gameObject, 1);
        }
    
    }

    public void ReleaseTask()
    {
        if (this.citizenTask != CitizenTask.Empty)
        this.citizenTask = CitizenTask.Empty;
    }

    public void SetCitizenTask(CitizenTask citizenTask)
    {
        this.citizenTask = citizenTask;
    }
}
