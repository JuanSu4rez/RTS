using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums;
using V2.Enums.Task;

namespace V2.Animations
{
    public class CitizenControllerAnimations : MonoBehaviour
    {
        private Enums.Citizen.CitizenAnimationStates citizeAnimationState;
        private IUnitController unitController;
        private Animator animator;
        // Use this for initialization
        void Start() {
            citizeAnimationState = Enums.Citizen.CitizenAnimationStates.None;
            animator = this.gameObject.GetComponent<Animator>();
            unitController = this.gameObject.GetComponent<IUnitController>();
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Pick);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Axe);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Hammer);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Gold);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Meat);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Wood);
        }
        // Update is called once per frame
        void Update() {
        }
        private void LateUpdate() {
            SetAnimation();
        }
        private void SetAnimation() {
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Pick);
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Axe);
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Hammer);
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Gold);
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Meat);
            EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Wood);
            Enums.Citizen.CitizenAnimationStates current = this.citizeAnimationState;
            this.citizeAnimationState = Enums.Citizen.CitizenAnimationStates.None;
            int _intanimationState = (int)CitizeAnimationStates.None;
            var task = unitController.GetTask();
            switch(task) {
                case IMoveTask mtask:
                    citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Walking;
                    break;
                case V2.Tasks.Unit.GatheringTask gtask:
                    if(gtask.taskState == TaskStates.OnTheWay) {
                        citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Walking;
                    }
                    else {
                        switch(gtask.resourceType) {
                            case Enums.ResourceTypes.Food:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Axe, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Wood;
                                break;
                            case Enums.ResourceTypes.Gold:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Pick, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Gold;
                                break;
                            case Enums.ResourceTypes.None:
                                break;
                            case Enums.ResourceTypes.Rock:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Pick, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Gold;
                                break;
                            case Enums.ResourceTypes.Wood:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Axe, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Wood;
                                break;
                        }
                    }
                    break;
                case V2.Tasks.Unit.DepositingTask deptask:
                    if(deptask.taskState == TaskStates.OnTheWay) {
                        citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Walking;
                    }
                    else {
                        switch(deptask.resourceType) {
                            case Enums.ResourceTypes.Food:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Gathered_Meat, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingMeat;
                                break;
                            case Enums.ResourceTypes.Gold:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Gathered_Gold, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingGold;
                                break;
                            case Enums.ResourceTypes.None:
                                break;
                            case Enums.ResourceTypes.Rock:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Gathered_Gold, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingGold;
                                break;
                            case Enums.ResourceTypes.Wood:
                                EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren.Gathered_Wood, true);
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingWood;
                                break;
                        }
                    }
                    break;
            }
            _intanimationState = (int)citizeAnimationState;
            animator.SetInteger("state", _intanimationState);
        }
        private void InitChildrentTool(V2.Enums.Citizen.CitizenTransformChildren children) {
            EnableChildrentTool(children);
        }
        private void EnableChildrentTool(V2.Enums.Citizen.CitizenTransformChildren children, bool enable = false) {
            var id = (int)children;
            if(this.transform.childCount > id) {
                this.transform.GetChild(id).gameObject.SetActive(enable);
            }
        }
    }
}