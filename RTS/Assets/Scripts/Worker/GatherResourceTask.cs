﻿using UnityEngine;
using System.Collections;

public class GatherResourceTask : Task{

    private ResourceBehaviour _resourceBehaviour;
    private WorkerBehaviour _workerBehaviour;
    // Use this for initialization
    private void Awake() {
        enabled = false;
    }

    public override void TaskStart() {
        _workerBehaviour = this.gameObject.GetComponent<WorkerBehaviour>();
    }

    // Update is called once per frame
    public override void TaskUpdate() {
       
        var discounted = _resourceBehaviour.DiscountAmount(_workerBehaviour.GetAmountToDiscount());
        _workerBehaviour.GatheringCapacity.Current += discounted;
        //TODO remove sleep
        System.Threading.Thread.Sleep(100);
    }

    public override bool IsFinished() {
        return _workerBehaviour.GatheringCapacity.HasReachedhMaxCapacity || _resourceBehaviour.IsEmpty;
    }

    public void SetResource(ResourceBehaviour resourceBehaviour) {
        _resourceBehaviour = resourceBehaviour;
    }
  
}