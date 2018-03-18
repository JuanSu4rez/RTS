using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavAgentArcherScript : MonoBehaviour, IAliveBeing, IFigther, IStatus, ISelectable, ITeamable, IDamagable
{
    private IGameFacade gameFacade;
    private SoldierStates soldierState;
    private MilitaryTask militaryTask;

    private SphereCollider attackCollider;
    

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
        }
    }

    public bool IsSelected { get; set; }

    private Vector3 pointToMove;
    private float speedWalk;
    
    public float AttackPower { get; set; }
    public float DefensePower { get; set; }
    public float AttackRange { get; set; }

    private float Health;
    private float CurrentHealth;

    //Cooldown
    private float coolDown;
    private float lastShoot;

    public NavMeshAgent navMeshAgent;

    void Awake()
    {

    }

        // Use this for initialization
    void Start()
    {
        speedWalk = 0.255F;
        AttackPower = 100F;
        DefensePower = 0.1F;
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        Health= 9999;
        CurrentHealth =Health;
        gameFacade = GameScript.GetFacade(team);
        soldierState = SoldierStates.Idle;

        //AttackRange = gameObject.GetComponent<CapsuleCollider>().bounds.;
        AttackRange = 900;

        coolDown = 2f;
        lastShoot = 0f;

        changeColor();
    }

    public virtual void changeColor()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Team.Color;
    }

    public void shootArrow(){
        //ArrowPoint
        Vector3 arrowOrigin = gameObject.transform.Find("ArrowPoint").position;

    }

    public void ModifyRangeAttack(float newRange) {
        attackCollider.radius = attackCollider.radius * newRange;
    }

    void OnTriggerEnter(Collider collider) {
        if (soldierState == SoldierStates.Idle)
        {
            var team = collider.gameObject.GetComponent<ITeamable>();
            if (team != null)
            {
                if (this.Team.Id != team.Team.Id)
                {                    
                    militaryTask = new MilitaryTask(collider.gameObject, MilitaryTaskType.Attack);
                    Vector3 targetDistance = getTargetDistance();
                    if (targetDistance.sqrMagnitude > AttackRange){
                        soldierState = SoldierStates.Walking;
                        SetPointToMove(collider.gameObject.transform.position);
                    }
                    else
                        soldierState = SoldierStates.Attacking;
                }

            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.log("Colision " + pointResource);
        var name = collision.gameObject.name;
        var tag = collision.gameObject.tag;
        switch (soldierState)
        {
            case SoldierStates.Attacking:
                break;          
            case SoldierStates.Died:
                break;           
            case SoldierStates.Idle:
                break;
            case SoldierStates._None:
                break;
            case SoldierStates.Walking:
                if (militaryTask != null) {
                    if(militaryTask.Gameobject == collision.gameObject)
                        soldierState = SoldierStates.Attacking;                   
                }
                break;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.log("Collisionstay  " + pointResource);
        switch (soldierState)
        {
            case SoldierStates.Attacking:
                break;
     
            case SoldierStates.Died:
                break;
            case SoldierStates.Idle:
                break;
            case SoldierStates._None:
                break;
            case SoldierStates.Walking:
                //if (collision.gameObject.name.Equals("UrbanCenter"))
                //{
                //    //Debug.log("Collisionstay  UrbanCenter");
                //    if (citizenLabor != CitizenStates.None)
                //    {
                //        //TODO  ahumentar recursos al jugador
                //        if (CurrentAmountResouce > 0){
                //            CurrentAmountResouce = 0;
                //        }
                //        //pointToMove = pointResource;
                //        SetPointToMove(pointResource);
                //    }

                //}
                //else if (collision.gameObject.name.Equals("POINTTOMOVE"))
                //{
                //    collision.gameObject.transform.position = Camera.main.transform.position;
                //    SetState(CitizenStates.Idle);
                //    navMeshAgent.enabled = false;
                //}
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
        switch (soldierState)
        {
            case SoldierStates.Attacking:                
                if (militaryTask != null && !militaryTask.IscompletedTask())
                {
                    var damagable = militaryTask.Gameobject.GetComponent<IDamagable>();
                    if (damagable != null) {
                        //TODO calc damage depending on the distance and the attack Range
                        //damagable.AddDamage(AttackPower);
                        if (militaryTask.IscompletedTask())
                            soldierState = SoldierStates.Idle;
                        else
                        {
                            if (getTargetDistance().sqrMagnitude < AttackRange)
                            {
                                soldierState = SoldierStates.Attacking;
                                navMeshAgent.enabled = false;
                                gameObject.transform.LookAt(militaryTask.Gameobject.transform);
                                //Cooldwon
                                if (Time.time > lastShoot + coolDown)
                                {
                                    shootArrow();
                                    damagable.AddDamage(AttackPower);
                                    lastShoot = Time.time;
                                }                                
                            }
                            else
                            {   
                                soldierState = SoldierStates.Walking;
                                SetPointToMove(militaryTask.Gameobject.transform.position);
                            }
                        }
                    }
                }
                else {                    
                    soldierState = SoldierStates.Idle;
                }

                break;
            case SoldierStates.Died:
                break;
            case SoldierStates.Idle:
                break;
            case SoldierStates._None:
                break;
            case SoldierStates.Walking:
                if (militaryTask != null)
                {
                    Vector3 targetDistance = getTargetDistance();
                    if (targetDistance.sqrMagnitude <= AttackRange) { 
                        soldierState = SoldierStates.Attacking;
                        navMeshAgent.enabled = false;
                    }
                else if (militaryTask.Gameobject.transform.position != navMeshAgent.destination)
                    {
                        navMeshAgent.enabled = true;
                        SetPointToMove(militaryTask.Gameobject.transform.position);
                    }
                    
                }
               
                break;
            default:
                break;
        }
    }

    public Vector3 getTargetDistance() {
        return transform.position - militaryTask.Gameobject.transform.position;
    }

    public string GetStatus()
    {
        return soldierState.ToString() + " "+GetHealthReason()  +" "+ soldierState.ToString() + " " + pointToMove;
    }

    public void SetState(SoldierStates _soldierStates)
    {

        //if (_citizenStates == CitizenStates.Attacking || _citizenStates == CitizenStates.Building || _citizenStates == CitizenStates.Gathering)
        //{
        //    this.soldierState = CitizenStates.Walking;
        //    this.citizenLabor = _citizenStates;
        //}
        //else
        //{
        //    this.citizenState = _citizenStates;
        //    this.citizenLabor = CitizenStates.None;
        //}
    }

    public void SetPointToMove(Vector3 newPointToMove)
    {
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(newPointToMove);
    }



    public void Walking()
    {
        Vector3 positionDif = pointToMove - gameObject.transform.position;
        var distance = positionDif.magnitude;
        var direction = positionDif / distance;
        gameObject.transform.position += direction * speedWalk;
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
            soldierState = SoldierStates.Died;
            Destroy(gameObject, 1);
        }       
    }
}
