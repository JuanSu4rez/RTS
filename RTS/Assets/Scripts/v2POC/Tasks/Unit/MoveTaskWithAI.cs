using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Interfaces.Task;
using UnityEngine.AI;
using V2.Enums.Task;

namespace V2.Tasks.Unit.Citizen
{
    public class MoveTaskWithAI : IMoveTask{
        public GameObject GameObject { get; set; }
        public Vector3 Destiny { get; set; }
        private TaskStates taskState;
        private NavMeshAgent goNavMeshAgent;
        private float distanceOfTolerance = 2f;
        public MoveTaskWithAI(GameObject _gameObject, Vector3 _destiny) {
            this.GameObject = _gameObject;
            this.Destiny = _destiny;
            goNavMeshAgent = this.GameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if(goNavMeshAgent == null)
                throw new System.Exception("The Game Object does not have a NavMeshAgent.");
            goNavMeshAgent.SetDestination(this.Destiny);
        }
        public bool IsComplete() {
            return taskState == TaskStates.Completed;
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