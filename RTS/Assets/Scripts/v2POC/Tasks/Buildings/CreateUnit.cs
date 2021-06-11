using UnityEngine;
using System.Collections;
using V2.Interfaces;
using System.Collections.Generic;
using V2.Interfaces.Task;
using V2.Enums;

namespace V2.Tasks
{
    [System.Obsolete]
    public class CreateUnit : ITask{
        public GameObject GameObject { get; set; }
        private EntityType unitToBeCreated;
        private float time = 0;
        public CreateUnit(EntityType unitToBeCreated) {
            this.unitToBeCreated = unitToBeCreated;
            this.time = this.unitToBeCreated.TimeToCreate();
        }
        public bool IsComplete() {
            return time <= 0;
        }
        public void Update() {
            time -= Time.deltaTime;
        }
    }
}