

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QueueController : MonoBehaviour {



    public int maxwaitpositionscount = 20;

 


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


        //if (this.GetComponent<CapsuleCollider>() != null)

        var collider = this.GetComponents<Collider>().FirstOrDefault(p => p.enabled == true);
        pointstowWork = Utils.GetPoints(collider);

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
        //si el recurso se acabo debe detenerse los demas

     
        foreach (var go in waitlist) {
            int index = Array.FindIndex(worker, p => p == go);

            if (index >= 0) {

                Debug.LogError(worker[index].name + " object exists in two list");
                Application.Quit();
            }
        }
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

        int index = Array.FindIndex(worker, p => p == null);

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

        var t = Time.time;

        int index = Array.FindIndex(worker, p => p == obj);
        // Debug.Log(t + "Array.FindIndex(worker ");

        if (index > -1) {

            //Debug.Log(t + "remuevo en la lista de trabajo " + index+" "+ worker[index]);
            int cc = waitlist.Count;

            if (waitlist.Count > 0) {


                var otherworker = waitlist[0];

                if (otherworker == obj) {
                    Debug.Log("El objeto removido de la lista de trabajo no puede estar disponible a trabajar.");
                    return;
                }

                if (!waitlist.Remove(otherworker)) {

                    
                    Debug.Log("oTHER WROKER DEBIO DESTRUIRSE "+ otherworker.name+" "+obj.name);
                    
                    DestroyObject(otherworker);
                    DestroyObject(obj);

                    return;
                }


                worker[index] = null;

              
                int cc2 = waitlist.Count;
                //Debug.Log(t + "waitlist change " + cc + " " + cc2);
                var unitController = otherworker.GetComponent<UnitController>();

                var resourcescript = this.GetComponent<ResourceScript>();

                var hasunitcontroller = unitController != null;


                if (hasunitcontroller) {

                    //machetaso

                    var cmptask = unitController.GetTask<CompositeTask>();
                    GatheringTask gttask = null;

                    if (cmptask != null) {
                        gttask = cmptask.task as GatheringTask;
                        
                    }


                    if(gttask == null)
                        gttask = unitController.GetTask<GatheringTask>();


                    if (gttask != null) {

                      

                        worker[index] = otherworker;
                        var position = pointstowWork[index];

                        //si al hacer el move hago el release task entonces bi se realiza la tarea
                        unitController.Move(position, gttask, () => {
       
                            gttask.onwait = false;
                            unitController.SetTask(gttask);
                            // unitController.EnableTask();
                        });

                    }
                    else {
                        Debug.Log(t + unitController.gameObject.name + " The unit doest not have a peding task. " + (gttask != null ? gttask.GetType().Name : "Null"));

                    }

                }
                else {
                    //  Debug.Log(t + $" Worker no cumple con los componentes hasunitcontroller {hasunitcontroller} ");
                }

            }
            else {
                worker[index] = null;
            }


        }
        else {

            //Debug.Log(t + "RelasePostion "+ obj.name);
            if (waitlist.Remove(obj)) {
                //   Debug.Log(t+" remuevo en la lista de espera");
                return;
            }


        }




    }

}
