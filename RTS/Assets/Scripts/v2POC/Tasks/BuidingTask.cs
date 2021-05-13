using UnityEngine;
using System.Collections;
using V2.Interfaces;
namespace V2.Tasks
{
    public class BuidingTask : ICompoundTask {
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