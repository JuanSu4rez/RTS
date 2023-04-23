using UnityEngine;
using System.Collections;

public class WorkerBehaviour : MonoBehaviour{

    [SerializeField]
    private Capacity _gatheringCapacity;

    public Capacity GatheringCapacity => _gatheringCapacity;

    [SerializeField]
    private int _gatheringSpeed;

    public int GatheringSpeed => _gatheringSpeed;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int GetAmountToDiscount() { 
        var amountToDiscount = GatheringSpeed;
        if(GatheringCapacity.Current + amountToDiscount > GatheringCapacity.Limit) {
            amountToDiscount = GatheringCapacity.Limit - GatheringCapacity.Current;
        }
      return amountToDiscount;
    }
}
