using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour, IMovable, ISelectable, IFigther, IWorker, IDamagable, IStatus, IControlable, ITeamable {//, , IControlable<CitizenStates>, IFigther, IWorker, IStatus, ISelectable, ITeamable, IDamagable {


    public CitizenStates citizenState;

    private CitizenStates initialCitizenState;


    private CitizeAnimationStates animationstate;


    private float distanceTolerance =2f;

    public Task task;


    public int Team = 0;

    public bool IsSelected { get; set; }

    private NavMeshAgent navMeshAgent;

    private Animator animator;



    void Awake() {

    }

    // Use this for initialization
    void Start() {

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        animator = this.gameObject.GetComponent<Animator>();

        navMeshAgent.enabled = false;

        InitChildrentTool(CitizenTransformChilden.Pick);
        InitChildrentTool(CitizenTransformChilden.Axe);
        InitChildrentTool(CitizenTransformChilden.Hammer);
        InitChildrentTool(CitizenTransformChilden.Gathered_Gold);
        InitChildrentTool(CitizenTransformChilden.Gathered_Meat);
        InitChildrentTool(CitizenTransformChilden.Gathered_Wood);
    }

    void OnCollisionStay(Collision collision) {

    }


    void OnCollisionEnter(Collision collision) {

    }

    int frame = 0;
    void Update() {
        frame++;
        initialCitizenState = citizenState;


        if (task != null) {
            // Debug.Log($"state {task.GetType().FullName}");
            switch (task) {

                case MoveTask mtask:
                    ExcuteMovingTask(mtask);
                    break;
                case GatheringTask gtask:
                    ExcuteGatheringTask(gtask);
                    break;
                default:

                    break;


            }
        }
        else {
            Debug.Log("state null");

        }

        SetAnimation();
    }

    float nextrecalc = 0;

    void ExcuteMovingTask(MoveTask mtask) {
        Debug.Log("ExcuteMovingTask");

        /*
        if (nextrecalc == 0)
            nextrecalc = Time.time + 3f;

        if (Time.time > nextrecalc) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(mtask.position);
            nextrecalc = Time.time + 3f;
        }
        */

        float distanceToDestiny = Mathf.Abs(Vector3.Distance(this.transform.position, mtask.position));
        if (distanceToDestiny <= distanceTolerance  ) {
            this.transform.position = mtask.position;
            nextrecalc = 0;
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
            //si es composite evaluar la posibilidad de hacer la transerencia de tarea
            this.SetTask(null);

            citizenState = CitizenStates.Idle;

            if (mtask.action != null) {

                mtask.action();
            }




            return;
        }

        citizenState = CitizenStates.Walking;
    }

    void ExcuteGatheringTask(GatheringTask gtask) {

        // si el recurso se destruyo
        if (!gtask.IsValidTask()) {


            //if the task was invalid  fin for another mine
            if (gtask.CurrentAmountResouce > 0) {

                CitizeAnimationStates animstate = CitizeAnimationStates.None;
                switch (gtask.resourceType) {
                    case Resources.Food:
                        animstate = CitizeAnimationStates.CarryingMeat;
                        break;
                    case Resources.Gold:
                        animstate = CitizeAnimationStates.CarryingGold;
                        break;
                    case Resources.None:
                        break;
                    case Resources.Rock:
                        animstate = CitizeAnimationStates.CarryingGold;
                        break;
                    case Resources.Wood:
                        animstate = CitizeAnimationStates.CarryingWood;
                        break;
                }

                //correccion

                gtask.ReleaseWorkSpot(this.gameObject);
                var taskcopy = gtask;
                //antes de movove se puede llamar a releasetask
                var buildinggameobject = GameScript.FindResoruceBuidingToDeposit(Team, gtask.resourceType, this.transform.position);

             
                this.Move(Utils.PositionSubHalfBounsdssizeXZ(buildinggameobject), animstate, () => {

                    //incrementar la capicidad del jugador del recurso dado
                    GameScript.AddResource(Team, taskcopy.resourceType, taskcopy.CurrentAmountResouce);
                    taskcopy.ResetTask();

                });
                //correccion



            }



            return;
        }

        this.citizenState = CitizenStates.Gathering;


        gtask.Execute();

        if (gtask.MaxCapacityAchivied()) {


            //

            //todo se notifica al recurso que sale de la pocision del trabajo

            CitizeAnimationStates animstate = CitizeAnimationStates.None;
            switch (gtask.resourceType) {
                case Resources.Food:
                    animstate = CitizeAnimationStates.CarryingMeat;
                    break;
                case Resources.Gold:
                    animstate = CitizeAnimationStates.CarryingGold;
                    break;
                case Resources.None:
                    break;
                case Resources.Rock:
                    animstate = CitizeAnimationStates.CarryingGold;
                    break;
                case Resources.Wood:
                    animstate = CitizeAnimationStates.CarryingWood;
                    break;
            }

            gtask.ReleaseWorkSpot(this.gameObject);
            //antes de move se puede llamar a release task
            var taskcopy = gtask;

            var buildinggameobject = GameScript.FindResoruceBuidingToDeposit(Team, gtask.resourceType, this.transform.position);
        

            //Move to positionBuldingtodeposit
            this.Move(Utils.PositionSubHalfBounsdssizeXZ(buildinggameobject), animstate, () => {

                //incrementar la capicidad del jugador del recurso dado
                GameScript.AddResource(Team, taskcopy.resourceType, taskcopy.CurrentAmountResouce);
                taskcopy.ResetTask();

                //se mueve otra vez a la posicion del recurso
                //TODO se tiene que hacer la logica de la cola por recruso

                var queuecontroller = taskcopy.resourcescript.GetComponent<QueueController>();

                if (queuecontroller != null) {

                    int flag = -1;
                    GameObject obj = this.gameObject;

                    var position = queuecontroller.GetPosition(ref obj, out flag);

                    if (flag >= 0) {

                        Move(position, taskcopy, () => {

                            /*GatheringTask gatheringtask = new GatheringTask();

                             gatheringtask.onwait = flag == 0;
                             gatheringtask.resourceType = Resources.Gold;
                             //buscar edifico a depositar mina o centro urbano
                             gatheringtask.positionBuldingtodeposit = new Vector3(-7.4f, 1f, -7.75f);
                             gatheringtask.position = position;
                             gatheringtask.Gatheringspeed = 0.1f;
                             gatheringtask.MaxCapacity = 50;
                             gatheringtask.CurrentAmountResouce = 0;
                             gatheringtask.resourcescript = taskcopy.resourcescript;

*/
                       
                            taskcopy.onwait = flag == 0;
                        
                            SetTask(taskcopy);
                         

                        });

                    }
                    else {
                        Debug.Log("Recurso no recibe mas trabajadores");
                    }

                }



                // taskcopy = null;



            });
        }

    }

    void ExcuteBuldingTask() {

    }

    void ExcuteAtackingTask() {

    }


    public void Move(Vector3 _position,Task _task ,Action _action ){//, Action _releaseaction = null) {
       // Debug.Log(this.gameObject.name+" Move "+ _position+" "+ _task.GetType()+" "+ (_action!= null) + " " + GetStatusTask());
        this.SetTask(new CompositeTask(_position) {  task = _task, action = _action });
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(_position);
        this.animationstate = CitizeAnimationStates.Walking;
    }




    public void Move(Vector3 _position, Action _action ) {//, Action _releaseaction = null) {
       // Debug.Log(this.gameObject.name + " Move " + _position + " "  + " " + (_action != null)+" "+ GetStatusTask());
        this.SetTask(new MoveTask(_position) {  action = _action });
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(_position);
        this.animationstate = CitizeAnimationStates.Walking;
    }

    private void Move(Vector3 _position, CitizeAnimationStates _animationstate, Action _action ) {

        //Debug.Log(this.gameObject.name + " Move State " + _position + " " + _animationstate + " " + (_action != null) + " " + GetStatusTask());
        this.SetTask(new MoveTask(_position) {action = _action });
        this.animationstate = _animationstate;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(_position);

        this.animationstate = _animationstate;
    }








    private void SetAnimation() {

        //if (animator != null && this.transform.childCount == (int)CitizenTransformChilden.Axe) 
        {
            //this.transform.GetChild((int)CitizenTransformChilden.Pick).gameObject.SetActive(false);
            EnableChildrentTool(CitizenTransformChilden.Pick, false);
            //this.transform.GetChild((int)CitizenTransformChilden.Axe).gameObject.SetActive(false)
            EnableChildrentTool(CitizenTransformChilden.Axe, false);
            EnableChildrentTool(CitizenTransformChilden.Hammer, false);
            EnableChildrentTool(CitizenTransformChilden.Gathered_Gold, false);
            EnableChildrentTool(CitizenTransformChilden.Gathered_Meat, false);
            EnableChildrentTool(CitizenTransformChilden.Gathered_Wood, false);
        }

        int _intanimationState = (int)citizenState;

        if (task != null) {

            switch (task) {

                case MoveTask mtask:
                    //Debug.Log($"{frame} Amimation MoveTask");
                    if (animationstate != CitizeAnimationStates.None)
                        _intanimationState = (int)animationstate;

                    switch (animationstate) {

                        case CitizeAnimationStates.Walking:

                            break;
                        case CitizeAnimationStates.CarryingGold:
                            EnableChildrentTool(CitizenTransformChilden.Gathered_Gold, true);
                            break;
                        case CitizeAnimationStates.CarryingWood:
                            EnableChildrentTool(CitizenTransformChilden.Gathered_Wood, true);
                            break;
                        case CitizeAnimationStates.CarryingMeat:
                            EnableChildrentTool(CitizenTransformChilden.Gathered_Meat, true);
                            break;

                    }

                    break;
                case GatheringTask gtask:
                    // Debug.Log($"{frame} Amimation GatheringTask");

                    if (!gtask.onwait) {


                        switch (gtask.resourceType) {
                            case Resources.Food:
                                break;
                            case Resources.Gold:

                                EnableChildrentTool(CitizenTransformChilden.Pick, true);
                                _intanimationState = (int)CitizeAnimationStates.Gold;

                                break;
                            case Resources.None:
                                break;
                            case Resources.Rock:
                                break;
                            case Resources.Wood:
                                break;
                        }

                    }

                    // Debug.Log($"ANIMATION GATHERING {gtask.resourceType} {animationState}");


                    break;

            }
        }


        animator.SetInteger("state", _intanimationState);
    }

    private void InitChildrentTool(CitizenTransformChilden children) {
        EnableChildrentTool(children, false);
    }

    private void EnableChildrentTool(CitizenTransformChilden children, bool enable) {
        var id = (int)children;
        if (this.transform.childCount > id) {
            this.transform.GetChild(id).gameObject.SetActive(enable);
        }
    }





    #region IsAlive implementation 

    private float Health;
    private float CurrentHealth;

    public float GetCurrentHealth() {
        return CurrentHealth;
    }

    public float GetHealth() {
        return Health;
    }

    public float GetHealthReason() {
        return CurrentHealth / Health;
    }

    public bool IsAlive() {
        return CurrentHealth > 0;
    }



    #endregion

    #region IFighter implementation 
    public float AttackPower { get; set; }
    public float AttackRange { get; set; }
    public float DefensePower { get; set; }

    #endregion

    #region IFighter implementation 
    public float ResourceCapacity { get; set; }

    public float CurrentAmountResouce { get; set; }

    public float BuildingSpeed { get; set; }

    public float GatheringSpeed { get; set; }

    public Resources CurrentResource { get; set; }
    #endregion


    #region IDamagable implementation 
    public void AddDamage(float damage) {
        if (IsAlive()) {
            if (damage > CurrentHealth) {
                CurrentHealth = 0;
            }
            else
                CurrentHealth = (int)(CurrentHealth - damage);
        }
        if (CurrentHealth <= 0) {
            citizenState = CitizenStates.Died;
            Destroy(gameObject, 3);
        }
    }



    #endregion

    #region IStatus implementation 
    public string GetStatus() {

        string _task = "NO TASK";

        if (task != null) { 
        _task = this.task.GetType().ToString()+" ";
        switch (task) {
            case CompositeTask ctask:
                _task = (ctask.position.ToString() + " has action " + (ctask.action != null))+" "+ ctask.task.GetType();

                break;
            case MoveTask mtask:
                _task = mtask.position.ToString()+" has action "+(mtask.action!= null);
                break;
            case GatheringTask gtask:
                _task = gtask.IsValidTask()+" ";
                break;
           
            default:

                break;


        }
        }

        return citizenState.ToString() + " " + GetHealthReason() + " "+ _task;// + citizenTask.ToString() + " " + pointToMove;
    }


    public string GetStatusTask() {

        string _task = "NO TASK";

        if (task != null) {
            _task = this.task.GetType().ToString() + " ";
            switch (task) {
                case CompositeTask ctask:
                    _task = (ctask.position.ToString() + " has action " + (ctask.action != null)) + " " + ctask.task.GetType();

                    break;
                case MoveTask mtask:
                    _task = mtask.position.ToString() + " has action " + (mtask.action != null);
                    break;
                case GatheringTask gtask:
                    _task = gtask.IsValidTask() + " ";
                    break;

                default:

                    break;


            }
        }

        return _task;
    }


 
        public void SetTask(Task _task) {


        // navMeshAgent.enabled = false;// = true;

        this.task = _task;

        switch (task) {

            case GatheringTask gatheringtask:
                if (gatheringtask.IsValidTask())
                    this.transform.LookAt(gatheringtask.resourcescript.gameObject.transform.position);
                break;

        
        }




    }


    public T GetTask<T>() where T : Task {

        return this.task as T;
    }

    public void EnableTask() {
        if (this.task == null)
            return;

        switch (this.task) {

            case GatheringTask gtask:
                gtask.onwait = false;
                break;
        }
    }

    public void ReleaseTask() {

        /*
        case CompositeTask cmptask:
                if (gatheringtask.IsValidTask())
                this.transform.LookAt(gatheringtask.resourcescript.gameObject.transform.position);
           break;*/

        Task t = this.task; 

        if (this.task is CompositeTask) {
           var cptask = t as CompositeTask;
            t = cptask.task;
        }


        switch (t) {

            case GatheringTask gtask:
                gtask.ReleaseWorkSpot(this.gameObject);
                break;

                /*
            case MoveTask mtask:
                if (mtask.releaseaction != null) {

                    mtask.releaseaction();
                }
                break;*/
        }
    }


    #endregion


    #region IStatus implementation 
    public void SetTeam(int team) {
        Team = team;
    }



    public int TeamId() {
        return Team;
    }


    #endregion
}
