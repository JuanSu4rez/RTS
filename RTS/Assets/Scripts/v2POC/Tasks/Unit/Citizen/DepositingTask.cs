using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public class DepositingTask : ICompoundTask {
        public IMoveTask MoveTask { get; set; }
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public GameObject GameObjectBuilding { get; set; }
        private V2.Interfaces.IResource iResource;
        public TaskStates taskState;
        public ResourceTypes resourceType;
        public DepositingTask(GameObject gameObject,GameObject gameObjectResource,IMoveTask moveTask) {
            this.GameObject = gameObject;
            this.GameObjectResource = gameObjectResource;
            this.iResource = this.GameObjectResource.GetComponent<V2.Interfaces.IResource>();
            this.resourceType = iResource.GetResource();
            this.MoveTask = moveTask;
            this.taskState = TaskStates.OnTheWay;
        }

        public bool IsComplete() {
            //if the resource is finished the palyer return to its gateringposition
            //and stay still
            return false;
        }
        public void Update() {
            switch(taskState) {
                case TaskStates.OnTheWay:
                    MoveTask.Update();
                    if(MoveTask.IsComplete()) {
                        //Deposit the material
                        //reasigng gatering task
                    }
                    break;
                case TaskStates.Completed:
                    break;
                case TaskStates.Waiting:
                    break;
            }

        }
    }
}