using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using V2.Enums.Task;
using V2.Enums;

namespace V2.Tasks.Unit.Citizen
{
    public class CmpGatheringTask : CompundTask {//ICompoundTask {
        
        public GameObject GameObjectResource { get; set; }
        private V2.Interfaces.IResource iResource;
        public ResourceTypes resourceType;
        private bool hasToWait = false;
        public CmpGatheringTask(GameObject gameObject,  GameObject gameObjectResource,  IMoveTask moveTask, bool hasToWait):base(gameObject,moveTask) {
            this.GameObject = gameObject;
            this.GameObjectResource = gameObjectResource;
            this.iResource = this.GameObjectResource.GetComponent<V2.Interfaces.IResource>();
            this.resourceType = iResource.GetResource();
            this.MoveTask = moveTask;
            this.hasToWait = hasToWait;
        }
        public override void PerformTask() {
            if(hasToWait) {
                this.taskState = TaskStates.Completed;
                return;
            }
            //Gather the resorce until citizen capacity has been reached
        }
    }
}