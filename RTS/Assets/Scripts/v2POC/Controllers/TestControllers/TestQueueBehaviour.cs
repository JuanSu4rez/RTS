using UnityEngine;
using System.Collections;
using V2.Interfaces.Task;
using System.Collections.Generic;

namespace V2.Controllers.TestControllers
{
    public class TestQueueBehaviour : MonoBehaviour
    {
        IQueueTask task;
        // Use this for initialization
        void Start() {
            GameObject animal = null;
            Vector3 destiny = Vector3.zero;
            task = new V2.Tasks.Unit.QueueTask(this.gameObject);
            task.ListOfTask.Add(new V2.Tasks.Unit.MoveTaskWithAI(this.gameObject,destiny));
            task.ListOfTask.Add(new V2.Tasks.Unit.Citizen.HuntingTask(this.gameObject, animal));
            //Once the animal is dead
            //**Gathering
            GameObject goresource = animal;
            task.ListOfTask.Add(new V2.Tasks.Unit.MoveTaskWithAI(this.gameObject, destiny));
            task.ListOfTask.Add(new V2.Tasks.Unit.Citizen.GatheringTask(this.gameObject, goresource));
            //you have to calc
            var destinytodeposit = Vector3.zero;
            GameObject goBuilding = null;
            task.ListOfTask.Add(new V2.Tasks.Unit.MoveTaskWithAI(this.gameObject, destinytodeposit));
            task.ListOfTask.Add(new V2.Tasks.Unit.Citizen.DepositingTask(this.gameObject, goresource, goBuilding));
        }
    }
}