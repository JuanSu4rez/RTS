using UnityEngine;
using System.Collections;

public class GatherResourceTask : RtsTask
{
    private ResourceBehaviour _resourceBehaviour;
    private WorkerBehaviour _workerBehaviour;
    // Use this for initialization
    private void Awake() {
        enabled = false;
    }

    public override void MyStart() {
        _workerBehaviour = this.gameObject.GetComponent<WorkerBehaviour>();
    }

    // Update is called once per frame
    public override void MyUpdate() {
        var discounted = _resourceBehaviour.DiscountAmount(_workerBehaviour.GatheringSpeed);
        _workerBehaviour.GatheringCapacity.Current += discounted;
    }

    public override bool IsFinished() {
        return _workerBehaviour.GatheringCapacity.HasReachedhMaxCapacity || _resourceBehaviour.IsEmpty;
    }

    public void SetResource(ResourceBehaviour resourceBehaviour) {
        _resourceBehaviour = resourceBehaviour;
    }
  
}
