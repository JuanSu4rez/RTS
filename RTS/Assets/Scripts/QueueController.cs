

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueController : MonoBehaviour {



    public int maxwaitpositionscount = 20;

    private int workpointpositions = 4;


    /// <summary>
    /// Points to work
    /// </summary>
    private GameObject[] worker = null;


    /// <summary>
    /// Points to work
    /// </summary>
    private List<GameObject> waitlist = null;

    /// <summary>
    /// Points to work
    /// </summary>
    private Vector3[] pointstowWork = null;

    /// <summary>
    /// Poitns to wait untill you will be called
    /// </summary>
    private Vector3[] pointsTowait = null;
    // Start is called before the first frame update

    private int indexwaitlist = 0;

    private void IncreaseWaitListIndex() {
        indexwaitlist = indexwaitlist + 1 % maxwaitpositionscount;
    }



    void Start() {


        if (this.GetComponent<CapsuleCollider>() != null)
            pointstowWork = Utils.GetPoints(this.GetComponent<CapsuleCollider>());

        worker = new GameObject[pointstowWork.Length];


        pointsTowait = Utils.GetPointsToWait(maxwaitpositionscount,
            this.gameObject.transform.position,

            this.gameObject.transform.right * 10 + this.gameObject.transform.up * 18, -this.gameObject.transform.up * 2);

        waitlist = new List<GameObject>(pointsTowait.Length);


        Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.right * 10, Color.red, 100000);


        Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.up * 10, Color.blue, 100000);
    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void OnDrawGizmos() {
        if (pointstowWork != null) {
            Gizmos.color = Color.red;
            foreach (var point in pointstowWork) {
                Gizmos.DrawSphere(point, 0.5f);
            }

        }
        if (pointsTowait != null) {


            Gizmos.color = Color.cyan;
            foreach (var point in pointsTowait) {
                Gizmos.DrawSphere(point, 0.5f);
            }

        }

    }


    public Vector3 GetPosition(ref GameObject obj, out int result) {
        Vector3 position = Vector3.zero;
        result = -1;

        int index = Array.FindIndex(worker, i => i == null);

        if (index >= 0) {
            result = 1;
            worker[index] = obj;
            position = pointstowWork[index];

            return position;

        }


        if (waitlist.Count < maxwaitpositionscount) {

            result = 0;

            waitlist.Add(obj);

            position = pointsTowait[indexwaitlist % maxwaitpositionscount];


            IncreaseWaitListIndex();
            return position;
        }


        return position;

    }


    public void RelasePostion(GameObject obj) {
        int index = Array.FindIndex(worker, p => p == obj);

        if (index > -1) {

            worker[index] = null;

            if (waitlist.Count > 0) {

                var otherworker = waitlist[0];
                waitlist.RemoveAt(0);

                worker[index] = otherworker;


                var unitController = otherworker.GetComponent<UnitController>();

                var resourcescript = this.GetComponent<ResourceScript>();

                var hasunitcontroller = unitController != null;
                var hashresourceScript = resourcescript != null;

                if (hasunitcontroller) {




                    var position = pointstowWork[index];

                    unitController.Move(position, () => {


                        GatheringTask gatheringtask = new GatheringTask();

                        gatheringtask.onwait = false;
                        gatheringtask.resourceType = Resources.Gold;
                        //buscar edifico a depositar mina o centro urbano
                        gatheringtask.positionBuldingtodeposit = new Vector3(-7.4f, 1f, -7.75f);
                        gatheringtask.position = position;
                        gatheringtask.Gatheringspeed = 5;
                        gatheringtask.MaxCapacity = 1000;
                        gatheringtask.CurrentAmountResouce = 0;
                        gatheringtask.resourcescript = resourcescript;


                        unitController.SetTask(gatheringtask);



                    });




                }
                else {
                    Debug.Log($"Worker no cumple con los componentes hasunitcontroller {hasunitcontroller} ");
                }

            }

            return;
        }

        waitlist.Remove(obj);


        return;
    }

}
