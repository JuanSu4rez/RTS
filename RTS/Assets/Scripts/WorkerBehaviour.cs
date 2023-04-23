using UnityEngine;
using System.Collections;

public class WorkerBehaviour : MonoBehaviour{

    [SerializeField]
    private Capacity _gatheringCapacity;

    public Capacity GatheringCapacity => _gatheringCapacity;

    [SerializeField]
    private int _gatheringSpeed;
 
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int GetAmountToDiscount() { 
        var gatheringSpeed = _gatheringSpeed;
        if(GatheringCapacity.Current + gatheringSpeed > GatheringCapacity.Limit) {
            gatheringSpeed = GatheringCapacity.Limit - GatheringCapacity.Current;
        }
      return gatheringSpeed;
    }
}
