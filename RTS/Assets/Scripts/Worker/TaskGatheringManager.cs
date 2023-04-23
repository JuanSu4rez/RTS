﻿using UnityEngine;
using System.Collections;
using System;

public class TaskGatheringManager : MonoBehaviour{

    private static Action<ResourceType, float> defAction = ( resourceType , amountOfResource) => { };
    private ResourceBehaviour _resourceBehaviour;
    private WorkerBehaviour _workerBehaviour;
    private Vector3 _placeToDeposit;
    public Vector3 PlaceToDeposit { set { _placeToDeposit = value; } }
    public Action<ResourceType, float> AddResourceAction { get; set; } = defAction;
    private ResourceType _resourceType;
    private TaskExecutor _taskExecutor;
    private Vector3 _lastGatheringPosition;
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

        if(_taskExecutor.HasEnded) {
            //deposit the current
            
            AddResourceAction(_resourceType, _workerBehaviour.GatheringCapacity.Current);
            //Current gets cleaned
            _workerBehaviour.GatheringCapacity.Current = 0;
            if(_resourceBehaviour != null && !_resourceBehaviour.IsEmpty) {
                Init(_resourceBehaviour);
            }
            else {
                //find a new resource or finish
                //we have to check by the range of view
                var gotask = this.gameObject.GetComponent<GoTask>();
                gotask.SetDestination(_lastGatheringPosition);
                gotask.enabled = true;
            }
        }
    }

    private void taskTransition(Task doneTask, Task newTask) {
        //TODO remove sleep
        System.Threading.Thread.Sleep(250);
        if(newTask == null) {
            return;
        }
        if(doneTask is GatherResourceTask gatherResourceTask
                     && newTask is GoTask goTask) {
            //place to deposit
            goTask.SetDestination(_placeToDeposit);
        }
    }

    public void Init(ResourceBehaviour resourceBehaviour) {
        _resourceBehaviour = resourceBehaviour;
        _resourceType = _resourceBehaviour.ResourceType;

        var gotask = this.gameObject.GetComponent<GoTask>();
        //place to gather
        _lastGatheringPosition = resourceBehaviour.gameObject.transform.position;
        gotask.SetDestination(resourceBehaviour.gameObject.transform.position);

        var gatherResourceTask = this.gameObject.GetComponent<GatherResourceTask>();
        gatherResourceTask.SetResource(resourceBehaviour);

        _taskExecutor.SetTaskTransition(taskTransition);
        _taskExecutor.Enqueue(gotask);
        _taskExecutor.Enqueue(gatherResourceTask);
        _taskExecutor.Enqueue(gotask);
        this.enabled = true;
        _taskExecutor.enabled = true;
    }
}