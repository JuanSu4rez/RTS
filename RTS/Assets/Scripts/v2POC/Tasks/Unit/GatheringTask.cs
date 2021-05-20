using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit
{
    public class GatheringTask : ICompoundTask {
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public IMoveTask MoveTask { get; set; }
        private V2.Interfaces.IResource iResource;
        public TaskStates taskState;
        public ResourceTypes resourceType;
        public GatheringTask(GameObject gameObject,
            GameObject gameObjectResource,
            IMoveTask moveTask,
            bool hasToWait) {
            this.GameObject = gameObject;
            this.GameObjectResource = gameObjectResource;
            this.iResource = this.GameObjectResource.GetComponent<V2.Interfaces.IResource>();
            this.resourceType = iResource.GetResource();
            this.MoveTask = moveTask;
        }
        public bool IsComplete() {
            return taskState == TaskStates.Completed;
        }
        public void Update() {
            switch(taskState) {
                case TaskStates.OnTheWay:
                    MoveTask.Update();
                    if(MoveTask.IsComplete()) {
                        taskState = TaskStates.Performing;
                        //if there is no more resorce then task completed
                        //perhaps find another resource
                    }
                    break;
                case TaskStates.Performing:
                    //after a period of time 
                    //get an amount of resource and add it to the unit, until its capacity is reached

                    break;
             
                    break;
                case TaskStates.Completed:
                    break;
                case TaskStates.Waiting:
                    break;
            }
        }
    }
}