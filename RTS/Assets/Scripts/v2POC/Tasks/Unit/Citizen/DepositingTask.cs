using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public class DepositingTask : ITask {
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public GameObject GameObjectBuilding { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public DepositingTask(GameObject gameObject,GameObject gameObjectResource, GameObject gameObjectBuilding ) {
            this.GameObject = gameObject;
            this.GameObjectResource = gameObjectResource;
            this.GameObjectBuilding = gameObjectBuilding;
            ResourceType =  this.GameObjectResource.GetComponent<V2.Controllers.UnitController>().EntityType.GetResourceType();
        }
        public bool IsComplete() {
            //if the resource is finished the palyer return to its gateringposition
            //and stay still
            return false;
        }
        public void Update(){
        }
    }
}