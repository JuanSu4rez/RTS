using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour, IMovable, ISelectable {//, IAliveBeing, IControlable<CitizenStates>, IFigther, IWorker, IStatus, ISelectable, ITeamable, IDamagable {


    public CitizenStates CitizenState;

    private CitizenStates initialCitizenState;


    private float distanceTolerance = 2;

    public Task task;

    [SerializeField]
    private Team team;

    public Team Team {
        get {
            return team;
        }
        set {
            team = value;
        }
    }

    public bool IsSelected { get; set; }

    public PlayerController playercontroller;


    private NavMeshAgent navMeshAgent;

    private Animator animator;



    void Awake() {

    }

    // Use this for initialization
    void Start() {

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    void OnCollisionStay(Collision collision) {

    }


    void OnCollisionEnter(Collision collision) {

    }


    void Update() {
        initialCitizenState = CitizenState;


        if (task != null) {
            Debug.Log($"state {task.GetType().FullName}");
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

        setAnimation();
    }


    void ExcuteMovingTask(MoveTask mtask) {

        float distanceToDestiny = Mathf.Abs(Vector3.Distance(navMeshAgent.destination, this.transform.position));
        if (distanceTolerance >= distanceToDestiny) {

            navMeshAgent.ResetPath();
            this.task = null;
            CitizenState = CitizenStates.Idle;

            if (mtask.action != null) {

                mtask.action();
            }




            return;
        }

        CitizenState = CitizenStates.Walking;
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


                this.Move(gtask.positionBuldingtodeposit, animstate, () => {

                    //incrementar la capicidad del jugador del recurso dado
                    if (playercontroller != null) {

                        playercontroller.AddResourceAmount(gtask.resourceType, gtask.CurrentAmountResouce);
                    }


                });

            }
            else 
            {
                this.task = null;
            }
            return;
        }

        this.CitizenState = CitizenStates.Gathering;


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

            this.Move(taskcopy.positionBuldingtodeposit, animstate, () => {

                //incrementar la capicidad del jugador del recurso dado
                if (playercontroller != null) {

                    playercontroller.AddResourceAmount(taskcopy.resourceType, taskcopy.CurrentAmountResouce);

                }

                if (taskcopy.IsValidTask()) {
                    taskcopy.ResetTask();

                    //se mueve otra vez a la posicion del recurso
                    //TODO se tiene que hacer la logica de la cola por recruso

                    this.Move(taskcopy.position, () => {
                        // la tarea es nuevamente recolectar
                        this.task = taskcopy;
                    });

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

        task = new MoveTask() { position = position, action = action };

        navMeshAgent.SetDestination(position);
    }


    private void Move(Vector3 position, CitizeAnimationStates _animationstate, Action action = null) {

        task = new MoveTask() { position = position, action = action, animationstate = _animationstate };

        navMeshAgent.SetDestination(position);
    }


    public void SetTask(Task task) {

        this.task = task;
    }



    private void setAnimation() {
       

        int animationState = (int)CitizenState;

        if (task != null) {
            switch (task) {

                case MoveTask mtask:

                    if (mtask.animationstate != CitizeAnimationStates.None)
                        animationState = (int)mtask.animationstate;

                    break;
                case GatheringTask gtask:


                    switch (gtask.resourceType) {
                        case Resources.Food:
                            break;
                        case Resources.Gold:


                            animationState = (int)CitizeAnimationStates.Gold;

                            break;
                        case Resources.None:
                            break;
                        case Resources.Rock:
                            break;
                        case Resources.Wood:
                            break;
                    }

                    Debug.Log($"ANIMATION GATHERING {gtask.resourceType} {animationState}");


                    break;

            }
        }


        animator.SetInteger("state", animationState);
    }




}
