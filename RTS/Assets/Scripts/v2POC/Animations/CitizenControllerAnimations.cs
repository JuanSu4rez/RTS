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
            HideChildrenElements();
        }
        // Update is called once per frame
        void Update() {
        }
        private void LateUpdate() {
            SetAnimation();
        }

        private void SetAnimation() {
            HideChildrenElements();
            var animationState = this.GetAnimationState();
            switch(animationState) {
                case Enums.Citizen.CitizenAnimationStates.Attacking:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Axe, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.Building:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Hammer, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.Gold:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Pick, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.Wood:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Axe, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.CarryingGold:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Gold, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.CarryingWood:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Wood, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.CarryingMeat:
                    EnableChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Meat, true);
                    break;
                case Enums.Citizen.CitizenAnimationStates.Died:
                    break;
                case Enums.Citizen.CitizenAnimationStates.Escaping:
                    break;
                case Enums.Citizen.CitizenAnimationStates.Gathering:

                    break;
                case Enums.Citizen.CitizenAnimationStates.Idle:
                    break;
                case Enums.Citizen.CitizenAnimationStates.None:
                    break;
                case Enums.Citizen.CitizenAnimationStates.Walking:
                    break;
                case Enums.Citizen.CitizenAnimationStates.Dying1:
                    break;
                case Enums.Citizen.CitizenAnimationStates.Dying2:
                    break;
            }
            animator.SetInteger("state", (int)animationState);
        }

        public void HideChildrenElements() {
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Pick);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Axe);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Hammer);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Gold);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Meat);
            InitChildrentTool(Enums.Citizen.CitizenTransformChildren.Gathered_Wood);
        }

        public Enums.Citizen.CitizenAnimationStates GetAnimationState() {
            this.citizeAnimationState = Enums.Citizen.CitizenAnimationStates.None;
            var task = unitController.GetTask();
            switch(task) {
                case IMoveTask mtask:
                    citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Walking;
                    break;
                case V2.Tasks.Unit.Citizen.CmpGatheringTask gtask:
                    if(gtask.taskState == TaskStates.OnTheWay) {
                        citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Walking;
                    }
                    else {
                        switch(gtask.resourceType) {
                            case Enums.ResourceTypes.Food:
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Wood;
                                break;
                            case Enums.ResourceTypes.Gold:
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Gold;
                                break;
                            case Enums.ResourceTypes.Rock:
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Gold;
                                break;
                            case Enums.ResourceTypes.Wood:
                                citizeAnimationState = Enums.Citizen.CitizenAnimationStates.Wood;
                                break;
                        }
                    }
                    break;
                case V2.Tasks.Unit.Citizen.CmpDepositingTask deptask:
                    switch(deptask.resourceType) {
                        case Enums.ResourceTypes.Food:
                            citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingMeat;
                            break;
                        case Enums.ResourceTypes.Gold:
                            citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingGold;
                            break;
                        case Enums.ResourceTypes.Rock:
                            citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingGold;
                            break;
                        case Enums.ResourceTypes.Wood:
                            citizeAnimationState = Enums.Citizen.CitizenAnimationStates.CarryingWood;
                            break;
                    }
                    break;
            }
            return citizeAnimationState;
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