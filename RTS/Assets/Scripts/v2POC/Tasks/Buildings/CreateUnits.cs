using UnityEngine;
using System.Collections;
using V2.Interfaces;
using System.Collections.Generic;
using V2.Interfaces.Task;

namespace V2.Tasks
{
    public class CreateUnits : ITask{
        public GameObject GameObject { get; set; }
        private List<UnitsEnum> unitsToBeCreated;
        private float time;
        public CreateUnits(int capacity = 20) {
            if(capacity <= 0)
                throw new System.ArgumentException("Invalid capacity");
            unitsToBeCreated = new List<UnitsEnum>(capacity);
        }
        public bool IsComplete() {
            return unitsToBeCreated.Count == 0;
        }
        public void Update() {
            time -= Time.deltaTime;
            if(time <= 0) {
                unitsToBeCreated.RemoveAt(0);
                //create the unit
                if(!IsComplete()) {
                    time = unitsToBeCreated[0].TimeToCreate();
                }
            }
        }
        public void AddUnit(UnitsEnum unitToCreate) {
            if(unitsToBeCreated.Count == 0)
                time = unitToCreate.TimeToCreate();
            unitsToBeCreated.Add(unitToCreate);
        }
        public void RemoveUnit(int index) {
            unitsToBeCreated.RemoveAt(index);
        }
    }
}