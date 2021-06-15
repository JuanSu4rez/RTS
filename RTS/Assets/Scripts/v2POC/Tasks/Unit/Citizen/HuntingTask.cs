using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public class HuntingTask : IComplexTask{
        public GameObject GameObject { get; set; }
        public GameObject GameObjectAnimal { get; set; }
        public TaskStates taskState;
        public HuntingTask(GameObject gameObject, GameObject GameObjectAnimal) {
            this.GameObject = gameObject;
            this.GameObjectAnimal = GameObjectAnimal;
            this.taskState = TaskStates.OnTheWay;
        }
        public bool IsComplete() {
            //until the animal is dead or the unit is far away
            return this.taskState == TaskStates.Completed;
        }
        public void Update() {
            //shoot arrow cool down
            //after cool down depening on the distance evalation calc the damage
            //if animal is dead enque new tasks to gather            
        }

        public ITask NextTask() {
            return null;
        }

        public ITask PreviousTask() {
            return null;
        }

        public bool CanBeContinued() {
            return false;
        }
    }
}