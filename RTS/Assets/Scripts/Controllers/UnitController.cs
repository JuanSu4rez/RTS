using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour, IMovable, ISelectable, IFigther, IWorker, IDamagable, IStatus {//, , IControlable<CitizenStates>, IFigther, IWorker, IStatus, ISelectable, ITeamable, IDamagable {


    public CitizenStates citizenState;

    private CitizenStates initialCitizenState;


    private float distanceTolerance = 2;

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

            }
        }
        else {
            Debug.Log("state null");

        }

        SetAnimation();
    }

    float nextrecalc = 0;

    void ExcuteMovingTask(MoveTask mtask) {

        if (nextrecalc == 0) 
        nextrecalc = Time.time + 3f;

        if (Time.time > nextrecalc) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(mtask.position);
            nextrecalc = Time.time + 3f;
        }


        float distanceToDestiny = Mathf.Abs(Vector3.Distance(navMeshAgent.destination, this.transform.position));
        if (distanceTolerance >= distanceToDestiny) {

            nextrecalc = 0;

            navMeshAgent.ResetPath();
            this.task = null;
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

            // si hay recurso recojido se puede ir a depositar o buscar otro recurso parecido
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


             

            }
            else 
            {
                this.task = null;
            }
            return;
        }

        this.citizenState = CitizenStates.Gathering;


        gtask.Execute();

        if (gtask.MaxCapacityAchivied()) {


            //
            var taskcopy = gtask;
            //todo se notifica al recurso que sale de la pocision del trabajo

            CitizeAnimationStates animstate = CitizeAnimationStates.None;
            switch (taskcopy.resourceType) {
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

            taskcopy.ReleaseWorkSpot(this.gameObject);

            //Move to positionBuldingtodeposit
            this.Move(taskcopy.positionBuldingtodeposit, animstate, () => {

                //incrementar la capicidad del jugador del recurso dado
                GameScript.AddResource(Team, taskcopy.resourceType, taskcopy.CurrentAmountResouce);

                if (taskcopy.IsValidTask()) {
                    taskcopy.ResetTask();

                    //se mueve otra vez a la posicion del recurso
                    //TODO se tiene que hacer la logica de la cola por recruso

                    var queuecontroller = taskcopy.resourcescript.GetComponent<QueueController>();

                    if (queuecontroller != null) {

                        int flag = -1;
                        GameObject obj = this.gameObject;

                        var position = queuecontroller.GetPosition(ref obj, out flag);

                        if (flag >= 0) {
                            Move(position, () => {


                                if (flag == 1) {


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
                                    this.SetTask(taskcopy);
                                
                                }
                                else {
                                    //liberamos la copia del task
                                    this.transform.LookAt(taskcopy.resourcescript.transform.position);
                                }


                            });

                        }
                        else {
                            Debug.Log("Recurso no recibe mas trabajadores");
                        }

                    }



                   // taskcopy = null;

                }
                else {

                    this.task = null;

                }

            });
        }

    }

    void ExcuteBuldingTask() {

    }

    void ExcuteAtackingTask() {

    }


    public void Move(Vector3 position, Action action = null) {

        ReleaseTask();
        this.SetTask( new MoveTask() { position = position, action = action });

        navMeshAgent.SetDestination(position);
    }


    private void Move(Vector3 position, CitizeAnimationStates _animationstate, Action action = null) {

        task = new MoveTask() { position = position, action = action, animationstate = _animationstate };

        navMeshAgent.SetDestination(position);
    }


    public void SetTask(Task task) {

        this.task = task;
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

        int animationState = (int)citizenState;

        if (task != null) {

            switch (task) {

                case MoveTask mtask:
                    Debug.Log($"{frame} Amimation MoveTask");
                    if (mtask.animationstate != CitizeAnimationStates.None)
                        animationState = (int)mtask.animationstate;

                    switch (mtask.animationstate) {
                       
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
                    Debug.Log($"{frame} Amimation GatheringTask");

                    switch (gtask.resourceType) {
                        case Resources.Food:
                            break;
                        case Resources.Gold:

                            EnableChildrentTool(CitizenTransformChilden.Pick, true);
                            animationState = (int)CitizeAnimationStates.Gold;

                            break;
                        case Resources.None:
                            break;
                        case Resources.Rock:
                            break;
                        case Resources.Wood:
                            break;
                    }

                   // Debug.Log($"ANIMATION GATHERING {gtask.resourceType} {animationState}");


                    break;

            }
        }


        animator.SetInteger("state", animationState);
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


    public T GetTask<T>() where T : Task {
        return task as T;
    }


    public void ReleaseTask() {

        if (this.task == null)
            return;

        switch (this.task) {

            case GatheringTask gtask:
                gtask.ReleaseWorkSpot(this.gameObject);
                break;
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
        return citizenState.ToString() + " " + GetHealthReason() + " ";// + citizenTask.ToString() + " " + pointToMove;
    }

    #endregion
}
