using UnityEngine;
using System.Collections;
using V2.Tasks;
using V2.Interfaces;

namespace V2.Controllers
{
    public class UnitController : MonoBehaviour, IUnitController{
        public V2.Interfaces.ITask toDo;
        // Use this for initialization
        void Start() {
            AssingTask( DoingNothing.nothing);
        }
        // Update is called once per frame
        void Update() {
            if(toDo != null) {
                toDo.Update();
                if(toDo.IsComplete()) {
                    //make transition
                    AssingTask(DoingNothing.nothing);
                }
            }
        }
        public void AssingTask(ITask task) {
            toDo = task;
        }
        public ITask GetTask() {
            return toDo;
        }
    }
}