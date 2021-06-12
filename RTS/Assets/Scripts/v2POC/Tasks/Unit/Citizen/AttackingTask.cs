using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;

namespace V2.Tasks.Unit.Citizen
{
    public class AttackingTask : ITask{
        public GameObject GameObject { get; set; }
        public GameObject GameTarget { get; set; }
        public bool IsComplete() {
            return false;
        }
        public void Update() {
           
        }
    }
}