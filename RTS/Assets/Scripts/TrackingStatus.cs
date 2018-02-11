using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingStatus : MonoBehaviour {

    private IStatus status;
    private IWorker worker;


    // Use this for initialization
    void Start () {
       object aux = this.gameObject.GetComponent<NavAgentCitizenScript>();
        if (aux != null)
        {
            status = aux as IStatus;
            worker = aux as IWorker;
        }

        aux = this.gameObject.GetComponent<BuildingBehaviour>();
        if (aux != null)
        {
            status = aux as IStatus;
            worker = aux as IWorker;
        }


        //aux = this.gameObject.GetComponent<IStatus>();
        //if (aux != null)
        //{
        //   // status = aux as IStatus;
        //}

        //aux = this.gameObject.GetComponent<IWorker>();
        //if (aux != null)
        //{
        //   // worker = aux as IWorker;
        //}

        Debug.Log("Status " + status);
    }
	
	// Update is called once per frame
	void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button "+status.GetStatus()))
        //    print("You clicked the button!");
        //Debug.Log("ongui");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 250, 100), printMessage());
      
    }

    private string printMessage()
    {
        string finalString = this.gameObject.name;
        if (status != null){
            finalString = string.Concat(" status ", status.GetStatus());
        }
        if (worker != null){
            finalString = string.Concat(finalString, "\n CurrentAmountResouce ", worker.CurrentAmountResouce);
        }
        return finalString;
    }
}
