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
    public class QueueTask : IQueueTask  {
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
                AssingNextTask();
            }
            else {
                AssingPreviousTask();
            }
        }
        private void AssingNextTask() {
            var INextTask = ListOfTask[0] as INextTask;
            if(INextTask != null) {
                var nextTask = INextTask.NextTask();
                if(nextTask != null)
                    //in case of multiple task
                    ListOfTask.Insert(0, nextTask);
            }
        }
        private void AssingPreviousTask() {
            var ITaskWithStates = ListOfTask[0] as ITaskWithValidation;
            var IPreviousTask = ListOfTask[0] as IPreviousTask;
            if(ITaskWithStates != null && IPreviousTask != null && !ITaskWithStates.CanBeContinued()) {
                var previousTask = IPreviousTask.PreviousTask();
                if(previousTask != null)
                    ListOfTask.Insert(0, previousTask);
            }
        }
    }
}