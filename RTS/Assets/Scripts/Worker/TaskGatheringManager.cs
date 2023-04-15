using UnityEngine;
using System.Collections;
using System;

public class TaskGatheringManager : MonoBehaviour
{
    private RtsTask currentTask = null;
    private System.Collections.Generic.Queue<RtsTask> _taskQueue = new System.Collections.Generic.Queue<RtsTask>();

    private ResourceBehaviour _resourceBehaviour;
    private WorkerBehaviour _workerBehaviour;
    private Vector3 placeToDeposit = Vector3.zero;
    // Use this for initialization
    private void Awake() {
        enabled = false;
    }

    void Start() {
        _workerBehaviour = this.gameObject.GetComponent<WorkerBehaviour>();
    }
    // Update is called once per frame
    void Update() {
      
    }

    private void LateUpdate() {
        if(_resourceBehaviour == null) {
            return;
        }
        Debug.Log("LateUdapte");
        if(_taskQueue.Count > 0) {
            if(currentTask == null) {
                currentTask = _taskQueue.Peek();
                currentTask.enabled = true;
                System.Threading.Thread.Sleep(300);
                return;
            }
            if(currentTask.IsFinished()) {
                var doneTask = _taskQueue.Dequeue();
                doneTask.enabled = false;
                currentTask = _taskQueue.Count > 0 ? _taskQueue.Peek() : null;
                if(currentTask != null) { 
                    currentTask.enabled = true;
                }
                taskTransition(doneTask, currentTask);
                System.Threading.Thread.Sleep(300);
                return;
            }
        }
        else {
            //deposit the current
            //Current gets cleaned
            _workerBehaviour.GatheringCapacity.Current = 0;
            if(!_resourceBehaviour.IsEmpty) {
                Init(_resourceBehaviour);
            }
            else {
                //find a new resource kind or finish
             
            }
        }

        System.Threading.Thread.Sleep(300);
    }

    private void taskTransition(RtsTask doneTask, RtsTask newTask) {

        if(newTask == null) {
            return;
        }
        if(doneTask is GatherResourceTask gatherResourceTask
                     && newTask is GoTask goTask) {
            //place to deposit
            goTask.SetDestiny( Vector3.zero);
        }
    }

    public void Init(ResourceBehaviour resourceBehaviour) {
        _resourceBehaviour = resourceBehaviour;
        var gotask = this.gameObject.GetComponent<GoTask>();
        //place to gather
        gotask.SetDestiny(resourceBehaviour.gameObject.transform.position);
        var gatherResourceTask = this.gameObject.GetComponent<GatherResourceTask>();
        gatherResourceTask.SetResource(resourceBehaviour);
        _taskQueue.Enqueue(gotask);
        _taskQueue.Enqueue(gatherResourceTask);
        _taskQueue.Enqueue(gotask);
        this.enabled = true;
    }
}
