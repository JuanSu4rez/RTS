

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{

    private int index = 0;

    private int indextowait = 0;

    private int maximunofunits = 4;

    public GameObject[] queue = new GameObject[20];

    public List<GameObject> waitlist = new List<GameObject>();

    private Vector3[] points = null;

    private Vector3[] pointsTowait = null;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<CapsuleCollider>() != null)
            points = Utils.GetPoints(this.GetComponent<CapsuleCollider>());

        //if (this.GetComponent<BoxCollider>() != null)
        //  points = Utils.GetPoints(this.GetComponent<BoxCollider>());


        pointsTowait = Utils.GetPointsToWait(queue.Length - maximunofunits,
            this.gameObject.transform.position,

            this.gameObject.transform.right * 10, this.gameObject.transform.right * 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnDrawGizmos()
    {
        if (points != null)
        {
            Gizmos.color = Color.red;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, 0.5f);
            }



        }
        if (pointsTowait != null)
        {


            Gizmos.color = Color.cyan;
            foreach (var point in pointsTowait)
            {
                Gizmos.DrawSphere(point, 0.5f);
            }

        }

    }


    public Vector3 GetPosition(GameObject obj, out bool result)
    {
        Vector3 position = Vector3.zero;
        result = false;

        if ((index + indextowait) >= this.queue.Length)
        {

            return position;
        }

        result = true;

        if (index < maximunofunits)
        {

            int _index = Array.FindIndex(this.queue, i => i == null);

            this.queue[_index] = obj;
            position = points[_index];

            index++;


        }
        else
        {
            // la unidad debe quedar esperando
            //retornar la pocision en la uqe debe quedar esperando
            /*
                        int _index = Array.FindIndex(this.pointsTowait, i => i == null );


                       position =  pointsTowait[_index];
                        //this.queue[index+ indextowait] = obj;

                        waitlist.Add(obj);
                        indextowait++;*/


        }



        return position;

    }


    public void RelasePostion(GameObject obj)
    {
        int i = 0;
        bool found = false;
        for (; i < maximunofunits; i++)
        {
            if (this.queue[i] == obj)
            {
                this.queue[i] = null;
                found = true;
                this.index--;
            }
        }




        if (found && waitlist.Count > 0)
        {



            var _object = waitlist[0];
            waitlist.RemoveAt(0);


            var citizenTemp = _object.GetComponent<NavAgentCitizenScript>();
            if (citizenTemp != null)
            {
                // aldeano debe ir a la posicion ; y ponerse a trabajar
                bool flag = false;
                var position = this.GetPosition(citizenTemp.gameObject, out flag);

                if (flag)
                {
                    citizenTemp.SetPointToMove(position);

                    citizenTemp.SetState(CitizenStates.Gathering);
                    citizenTemp.CurrentResource = Resources.Gold;

                }
                else
                {
                    Debug.Log("Desde Queue controller no encontro punto del recurso");
                }

            }

        }

    }

}
