using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavAgentSoldierScript : MonoBehaviour, IAliveBeing, IControlable<SoldierStates>, IFigther, IStatus, ISelectable, ITeamable, IDamagable
{
    private IGameFacade gameFacade;
    private SoldierStates soldierState;
    private MilitaryTask militaryTask;

  

    private SphereCollider attackCollider;


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
            gameFacade = GameScript.GetFacade(team);
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
    private Animator animator;
    [SerializeField]
    private Vector3 vectorup = Vector3.up;

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
        animator = this.gameObject.GetComponent<Animator>();
        Health = 9999;
        CurrentHealth = Health;
        gameFacade = GameScript.GetFacade(this.team);
        SetState( SoldierStates.Idle);

        //AttackRange = gameObject.GetComponent<CapsuleCollider>().bounds.;
        AttackRange = 5;
        coolDown = 2f;
        lastShoot = 0f;

        //changeColor();

        gameFacade.AddUnit(this.gameObject, Units.Citizen);
        vectorup.x = 0;
        vectorup.z = 0;
    }

    public virtual void changeColor()
    {
        Utils.ChangeColor(gameObject.GetComponent<MeshRenderer>(), team);
    }

    public void ModifyRangeAttack(float newRange)
    {
        attackCollider.radius = attackCollider.radius * newRange;
    }

    void OnTriggerEnter(Collider collider)
    {
        SetMilitaryTaskByTrigger(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        SetMilitaryTaskByTrigger(collider);
    }
    private void SetMilitaryTaskByTrigger(Collider collider)
    {
        if (soldierState == SoldierStates.Idle)
        {
            var team = collider.gameObject.GetComponent<ITeamable>();

            if (team == null || team.Team == null)
                return;

            if (!UtilsMilitary.ValidateGameObjectStateToAttackByTrigger(collider.gameObject))
                return;


            if (gameFacade.ValidateDiplomacy(team.Team, Postures.Enemy))
            {
                militaryTask = new MilitaryTask(collider.gameObject, MilitaryTaskType.Attack);
                Vector3 targetDistance = Vector3.zero;
                var _distance = militaryTask.GetTargetDistance(this.gameObject, out targetDistance);

                if (_distance && targetDistance.sqrMagnitude > AttackRange)
                {
                    SetState(SoldierStates.Walking);
                    SetPointToMove(collider.gameObject.transform.position);
                }
                else
                    SetState(SoldierStates.Attacking);
            }

        }

    }


    void OnCollisionEnter(Collision collision)
    {
        ////Debug.log("Colision " + pointResource);
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
                if (militaryTask != null)
                {
                    if (militaryTask.Gameobject == collision.gameObject)
                        SetState( SoldierStates.Attacking);
                }
                break;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        ////Debug.log("Collisionstay  " + pointResource);
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
                //    ////Debug.log("Collisionstay  UrbanCenter");
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
        ////Debug.log(pointToMove);
        switch (soldierState)
        {
            case SoldierStates.Attacking:
                if (militaryTask != null && !militaryTask.IscompletedTask())
                {
                    var damagable = militaryTask.Gameobject.GetComponent<IDamagable>();
                    if (damagable != null)
                    {
                        //TODO calc damage depending on the distance and the attack Range
                        //damagable.AddDamage(AttackPower);
                        if (militaryTask.IscompletedTask())
                            SetState( SoldierStates.Idle);
                        else
                        {

                           Vector3 targetDistance = Vector3.zero;
                            var distance = militaryTask.GetTargetDistance(this.gameObject, out targetDistance);
                            

                             if (distance  && targetDistance.sqrMagnitude < AttackRange)
                            {
                                SetState( SoldierStates.Attacking);
                                //navMeshAgent.enabled = false;
                                gameObject.transform.LookAt(militaryTask.Gameobject.transform);
                                //Cooldwon
                                if (Time.time > lastShoot + coolDown)
                                {
                                    damagable.AddDamage(AttackPower);
                                    lastShoot = Time.time;
                                }
                            }
                            else
                            {
                                SetState(SoldierStates.Walking);
                                SetPointToMove(militaryTask.Gameobject.transform.position);
                            }
                        }
                    }
                }
                else
                {
                    SetState( SoldierStates.Idle);
                }

                break;
            case SoldierStates.Died:

                gameFacade.RemoveUnity(this.gameObject, Units.Citizen);
                break;
            case SoldierStates.Idle:
                break;
            case SoldierStates._None:
                break;
            case SoldierStates.Walking:

                if (militaryTask != null)
                {
                    Vector3 targetDistance = Vector3.zero;

                    var _distance = militaryTask.GetTargetDistance(this.gameObject, out targetDistance);


                    if (_distance && targetDistance.sqrMagnitude <= AttackRange)
                    {
                        SetState(SoldierStates.Attacking);
                        //navMeshAgent.enabled = false;
                    }
                    else if (militaryTask.Gameobject.transform.position != navMeshAgent.destination)
                    {
                        SetPointToMove(militaryTask.Gameobject.transform.position);
                    }
                }

                if ((this.transform.position - navMeshAgent.destination) == vectorup)
                    SetState(SoldierStates.Idle);
                break;
            default:
                break;
        }
        setAnimation();
    }

    public virtual void setAnimation()
    {
        if (animator == null)
            return;
        /*
                Attacking,  //  0        
                Idle,       //  1
                None,       //  2    
                Walking,    //  3
                Dying1,     //  4
                Dying2      //  5
         */
        int animationState = (int)soldierState;       

        if (soldierState == SoldierStates.Died)
        {
            System.Random random = new System.Random();
            animationState = (int)SoldierAnimationStates.Dying1 + random.Next(1, 2);
        }

        if (soldierState == SoldierStates.Idle){
            animationState = (int)SoldierAnimationStates.Idle;
        }

        if (soldierState == SoldierStates.Walking){
            animationState = (int)SoldierAnimationStates.Walking;
        }

        if (soldierState == SoldierStates.Attacking){
            animationState = (int)SoldierAnimationStates.Attacking;
        }

        animator.SetInteger("state", animationState);
    }


    public string GetStatus()
    {
        return "Military task [" + (militaryTask == null ? "null" : militaryTask.ToString()) + "]" + soldierState.ToString() + " " + GetHealthReason() + " " + soldierState.ToString() + " " + pointToMove;
    }

    public void SetState(SoldierStates _soldierStates)
    {

        if (_soldierStates == SoldierStates.Idle)
        {
            if(attackCollider!= null)
            attackCollider.enabled = true;
        }
        else
        {
            if (attackCollider != null)
                attackCollider.enabled = false;
        }
        soldierState = _soldierStates;
    }

    public void SetPointToMove(Vector3 newPointToMove)
    {
        //navMeshAgent.enabled = true;
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
            SetState(SoldierStates.Died);
            Destroy(gameObject, 1);
        }
    }

    public void ReleaseTask()
    {
        militaryTask = null;
    }

}
