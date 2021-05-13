using UnityEngine;
using System.Collections;
using V2.Interfaces;
namespace V2.Tasks
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