using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using UnityEngine.AI;

namespace V2.Tasks.Unit
{
    public class MoveTaskWithAI : IMoveTask{
        public GameObject GameObject { get; set; }
        public Vector3 Destiny { get; set; }
        private TaskStates taskState;
        private NavMeshAgent goNavMeshAgent;
        private float distanceOfTolerance = 2f;
        public MoveTaskWithAI(GameObject GameObject, Vector3 Destiny) {
            this.GameObject = GameObject;
            this.Destiny = Destiny;
            goNavMeshAgent = this.GameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if(goNavMeshAgent == null)
                throw new System.Exception("The Game Object does not have a NavMeshAgent.");
            goNavMeshAgent.SetDestination(this.Destiny);
        }
        public bool IsComplete() {
            return taskState != TaskStates.Completed;
        }
        public void Update() {
            var transform = this.GameObject.transform;
            float distanceToDestiny = Mathf.Abs(Vector3.Distance(transform.position, Destiny));
            if(distanceToDestiny <= distanceOfTolerance) {
                transform.position = Destiny;
                goNavMeshAgent.ResetPath();
                taskState = TaskStates.Completed;
                goNavMeshAgent.enabled = false;
            }
        }
    }
}