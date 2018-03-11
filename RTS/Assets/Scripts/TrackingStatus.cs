using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingStatus : MonoBehaviour {

    private IStatus status;
    private IWorker worker;
    private IBulding building;
    private IAliveBeing aliveBeing;
    public bool IsSelected{ get; set; }

    private bool IsAliveBeing;

    private Slider healthBar;
    

    // Use this for initialization
    void Start () {

        object aux = this.gameObject.GetComponent<NavAgentCitizenScript>();
        if (aux != null)
        {
            status = aux as IStatus;
            worker = aux as IWorker;
            aliveBeing = aux as IAliveBeing;
        }

        aux = this.gameObject.GetComponent<BuildingBehaviour>();
        if (aux != null)
        {
            status = aux as IStatus;
            worker = aux as IWorker;
            building = aux as IBulding;
        }

        IsAliveBeing = aliveBeing != null;

        //healthBar
        if (gameObject.transform.childCount > 0)
        {
            Canvas canvas = gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<Canvas>();
            if (canvas != null)
            {
                healthBar = this.gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetChild(0).gameObject.GetComponent<Slider>();
                healthBar.enabled = true;
                    
                healthBar.transform.forward = -Camera.main.transform.forward;
                setHealthBarValue();  
                
           
            }             
        }
    }

    private void setHealthBarValue(){        
        if (!IsAliveBeing)
            healthBar.value = building.CurrentBuiltAmount / building.TotalBuiltAmount;
        if (IsAliveBeing)
        {
  
            float HealthReason = aliveBeing.GetHealthReason();
            if (!float.IsNaN(HealthReason))
            {
                healthBar.value = HealthReason;
            }
            Debug.Log("Health " + HealthReason);
        }
    }
    

    void Update() {
        if (IsSelected)
        {
            drawHealthBar();
        }
    }

	// Update is called once per frame
	void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button "+status.GetStatus()))
        //    print("You clicked the button!");
        ////Debug.log("ongui");
        if (IsSelected)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 250, 100), printMessage());
          
        }
       
      
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
        
    public void drawHealthBar()
    {
        if (healthBar != null)
        {
            setHealthBarValue();           
            healthBar.transform.forward = -Camera.main.transform.forward;
        }
    }


    public void drawHealthGUIBar()
    {
       
    }
}
