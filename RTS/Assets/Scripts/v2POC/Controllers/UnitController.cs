using UnityEngine;
using System.Collections;
using V2.Tasks;
using V2.Interfaces;
using V2.Tasks.Unit;
using V2.Interfaces.Task;
using System;

namespace V2.Controllers
{
    public partial class UnitController : MonoBehaviour, IUnitController,IHealthPoint{
        public ITask toDo;
        public float CurrentHealth { get; set; }
        void Start() {
            AssingTask(DoingNothing.nothing);
        }
        void Update() {
            //Debug.Log(Time.time + " " +this.GetType());
            if(IsHealthOk() &&
                toDo != null &&
                toDo as DoingNothing == null) {
                toDo.Update();
                if(toDo.IsComplete()) {
                    toDo = null;
                    AssingTask(DoingNothing.nothing);
                }
            }
        }
        public void AssingTask(ITask task) {
            toDo = task;
            if(
                !(task is DoingNothing) &&
                 toDo.GameObject != this.gameObject
                )
            toDo.GameObject = this.gameObject;
        }
        public ITask GetTask() {
            return toDo;
        }
        public bool IsHealthOk() {
            return CurrentHealth > 0;
        }
    }
}