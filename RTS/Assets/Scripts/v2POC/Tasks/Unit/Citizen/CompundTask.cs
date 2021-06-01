using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public abstract class CompundTask : ICompoundTask {
        public GameObject GameObject { get; set; }
        public IMoveTask MoveTask { get; set; }
        public TaskStates taskState;
        public CompundTask(GameObject gameObject, IMoveTask moveTask) {
            this.GameObject = gameObject;
            this.MoveTask = moveTask;
            taskState = TaskStates.OnTheWay;
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
                    }
                    break;
                case TaskStates.Performing:
                    PerformTask();
                    break;
            }
        }
        public abstract void PerformTask();
    }
}