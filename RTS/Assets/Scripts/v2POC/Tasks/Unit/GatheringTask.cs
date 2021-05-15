using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;

namespace V2.Tasks.Unit
{
    public class GatheringTask : ICompoundTask {
        public IMoveTask MoveTask { get; set; }
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public bool HasToWait { get; set; }
        private TaskStates taskState;
        public bool IsComplete() {
            return taskState == TaskStates.Completed;
        }
        public void Update() {
            switch(taskState) {
                case TaskStates.OnTheWay:
                    MoveTask.Update();
                    if(MoveTask.IsComplete()) {
                        taskState = HasToWait ? TaskStates.Waiting : TaskStates.Performing;
                        //if there is no more resorce then task completed
                        //perhaps find another resource
                    }
                    break;
                case TaskStates.Performing:
                    //after a period of time 
                    //get an amount of resource and add it to the unit, until its capacity is reached

                    break;
                case TaskStates.Depositing:
                    MoveTask.Update();
                    if(MoveTask.IsComplete()) {
                       //add the resource to the player
                       //go to the resource
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