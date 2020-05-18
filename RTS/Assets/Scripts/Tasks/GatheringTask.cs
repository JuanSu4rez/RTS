using UnityEngine;
using System.Collections;

public class GatheringTask : Task {

    public bool onwait = false;
    public Resources resourceType;
    public Vector3 positionBuldingtodeposit;
    public Vector3 position;
    public ResourceScript resourcescript;
    public float Gatheringspeed;
    public float MaxCapacity;
    public float CurrentAmountResouce;

    public void Execute() {

        if (onwait)
            return;

        var valuetodiscount = CurrentAmountResouce + Gatheringspeed;
        // si el aldeano supera su cantidad maxuia
        if (valuetodiscount > MaxCapacity) {
            valuetodiscount = MaxCapacity - CurrentAmountResouce;
        }
        else {
            valuetodiscount = Gatheringspeed;
        }

        // sele suma lo que le descuenta al recurso
        this.CurrentAmountResouce += resourcescript.DiscountAmount(valuetodiscount);

    }



    public bool MaxCapacityAchivied() {
        return CurrentAmountResouce >= MaxCapacity;
    }

    public bool IsValidTask() {
        //este metodo es necesario por que el recurso se autodestruye
        return resourcescript != null && resourcescript.IsEnabled();
    }


    public void ResetTask() {

        CurrentAmountResouce = 0;
    }

    /// <summary>
    /// Inform the resourse that the worker is stop working
    /// 
    /// this method is call when the worker complete its task
    /// 
    /// </summary>
    public void ReleaseWorkSpot(GameObject go) {

        if (!IsValidTask()) {
            return;
        }

        var queuecontroller = resourcescript.GetComponent<QueueController>();

        if (queuecontroller != null) {
            queuecontroller.RelasePostion(go);
        }

    }

}
