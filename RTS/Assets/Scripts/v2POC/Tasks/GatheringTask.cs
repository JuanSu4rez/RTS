using UnityEngine;
using System.Collections;
using V2.Interfaces;
namespace V2.Tasks
{
    public class GatheringTask : ICompoundTask {
        public IMoveTask MoveTask { get; set; }
        public GameObject GameObject { get; set; }
        public GameObject GameObjectResource { get; set; }
        public bool IsComplete() {
            return false;
        }
        public void Update() {
           
        }
    }
}