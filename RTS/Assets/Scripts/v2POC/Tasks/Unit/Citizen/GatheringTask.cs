using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public class GatheringTask : ITask {//ICompoundTask {
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public ResourceTypes ResourceType;
        public GatheringTask(GameObject gameObject, GameObject gameObjectResource) {
            this.GameObject = gameObject;
            this.GameObjectResource = gameObjectResource;
            ResourceType = this.GameObjectResource.GetComponent<V2.Controllers.UnitController>().EntityType.GetResourceType();
        }

        public bool IsComplete() {
            return false;
        }
        public void Update() {
           //gathering
        }
    }
}