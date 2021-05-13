using UnityEngine;
using System.Collections;
using V2.Interfaces;
namespace V2.Tasks
{
    public class MoveTask : IMoveTask{
        public GameObject GameObject { get; set; }
        public Vector3 Destiny { get; set; }
        public bool IsComplete() {
            return false;
        }
        public void Update() {

        }
    }
}