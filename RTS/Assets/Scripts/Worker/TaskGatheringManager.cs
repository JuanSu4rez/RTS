using UnityEngine;
using System.Collections;
using System;

public class TaskGatheringManager : MonoBehaviour
{
    private ResourceBehaviour _resourceBehaviour;
    private WorkerBehaviour _workerBehaviour;
    private Vector3 _placeToDeposit = Vector3.zero;
    private TaskExecutor _taskExecutor;
    // Use this for initialization
    private void Awake() {
        enabled = false;
        _workerBehaviour = this.gameObject.GetComponent<WorkerBehaviour>();
        _taskExecutor = this.gameObject.GetComponent<TaskExecutor>();
        
    }

    void Start() {

    }
    // Update is called once per frame
    void Update() {
        if(_resourceBehaviour == null) {
            return;
        }
        if(_taskExecutor.HasEnded) {
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
    }

    private void taskTransition(Task doneTask, Task newTask) {
        //just to slow
        System.Threading.Thread.Sleep(500);
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
        _taskExecutor.SetTaskTransition(taskTransition);    
        _taskExecutor.Enqueue(gotask);
        _taskExecutor.Enqueue(gatherResourceTask);
        _taskExecutor.Enqueue(gotask);
        this.enabled = true;
        _taskExecutor.enabled  = true;
    }
}
