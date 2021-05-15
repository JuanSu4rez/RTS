using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;

namespace V2.Tasks.Unit
{
    public class DoingNothing : ITask    {
        public static readonly DoingNothing nothing = new DoingNothing();
        public GameObject GameObject { get; set; }
        public bool IsComplete() {
            return false;
        }
        public void Update() {
          
        }
    }

}