using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;

namespace V2.Tasks.Unit
{
    public class BuildingTask : ICompoundTask {
        public IMoveTask MoveTask { get; set; }
        public GameObject GameObject { get; set; }
        public GameObject GameObjectBuilding{ get; set; }
        public bool IsComplete() {
            return false;
        }
        public void Update() {
            throw new System.NotImplementedException();
        }
    }
}