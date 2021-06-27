using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using UnityEngine.AI;
using V2.Enums.Task;
using System.Collections.Generic;
using System;

namespace V2.Tasks.Unit
{
    public class QueueTask : IQueueTask, IDiposeTask
    {
        public GameObject GameObject { get; set; }
        public IList<ITask> ListOfTask { get; set; }
        public int TaskIndex { get; set; }
        public QueueTask(GameObject gameObject) : this(gameObject, new List<ITask>()) {
        }
        public QueueTask(GameObject gameObject, IList<ITask> listOfTask) {
            GameObject = gameObject;
            ListOfTask = listOfTask;
        }
        public bool IsComplete() {
            //is complete till GameObjectTarget is dead or all my tasks have finished
            return ListOfTask.Count == 0;
        }
        public void Update() {
            if(IsComplete())
                return;
            var currentTask = ListOfTask[0];
            currentTask.Update();
            if(currentTask.IsComplete()) {
                ListOfTask.RemoveAt(0);
                AssingNextTask(ref currentTask);
            }
            else {
                AssingPreviousTask(ref currentTask);
            }
        }
        private void AssingNextTask(ref ITask currentTask) {
            var IComplexTask = currentTask as IComplexTask;
            if(IComplexTask != null) {
                var nextTask = IComplexTask.NextTask();
                if(nextTask != null) {
                    //this method is claa after removing the currentTask
                    //in case of multiple task we can not override them
                    ListOfTask.Insert(0, nextTask);
                }
            }
        }
        private void AssingPreviousTask(ref ITask currentTask) {
            var IComplexTask = currentTask as IComplexTask;
            if(IComplexTask != null && !IComplexTask.CanBeContinued()) {
                var previousTask = IComplexTask.PreviousTask();
                if(previousTask != null) {
                    ListOfTask.Insert(0, previousTask);
                }
            }
        }

        public void Dispose() {
            foreach(var task in ListOfTask) {
                switch(task) {
                    case IDiposeTask disposeTask:
                        disposeTask.Dispose();
                        break;
                }
            }
        }

        public ITask Current() {
            return ListOfTask.Count > 0? ListOfTask[0]: null;
        }
    }
}