using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;

namespace V2.Tasks.Unit.Citizen
{
    public class BuildingTask : ITask, IDiposeTask
    {
        public GameObject GameObject { get; set; }
        public GameObject GameObjectBuilding { get; set; }
        public int IndexPosition { get; set; }
        private bool isLookingAtBuilding = false;
        public BuildingTask(GameObject gameObject, GameObject goBuilding, int indexPosition) {
            this.GameObject = gameObject;
            this.GameObjectBuilding = goBuilding;
            IndexPosition = indexPosition;
            Debug.Log("Buldingtask  " + IndexPosition);
        }
        public bool IsComplete() {
            return false;
        }

        public void Update() {
            if(!isLookingAtBuilding) {
                GameObject.transform.LookAt(GameObjectBuilding.transform.position);
                isLookingAtBuilding = true;
            }
        }
        public void Dispose() {
            var unitBuldingController = GameObjectBuilding.GetComponent<V2.Controllers.UnitController>();
            unitBuldingController.ReleasePosition(IndexPosition);
            Debug.Log("Resource disposed");
        }
    }
}