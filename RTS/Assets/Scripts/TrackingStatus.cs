﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingStatus : MonoBehaviour {

    private IStatus status;
    private IWorker worker;
    private IBulding building;
    private IAliveBeing aliveBeing;
    private ISelectable selectable;


    private bool IsAliveBeing;


    private Texture2D textureHealt;

    private Texture2D textureCurrentHealt;


    private string[] a = new string[]{
    "NavAgentCitizenScript",

    };


   
 

    // Use this for initialization
    void Start () {

        textureHealt = new Texture2D(1, 1);
        textureHealt.SetPixel(0, 0, Color.red);
        textureHealt.Apply();


        textureCurrentHealt = new Texture2D(1, 1);
        textureCurrentHealt.SetPixel(0, 0, Color.green);
        textureCurrentHealt.Apply();

        var result = setInterfacesByBuilding();
        if (result)
            goto end;

        result = setInterfacesByAliveBeing();
       

   

        end:
        IsAliveBeing = aliveBeing != null;

    

      
    }

    private bool setInterfacesByAliveBeing()
    {
        bool result = false;
        
        object aux = this.gameObject.GetComponent<IAliveBeing>();
        if (aux != null)
        {
            result = true;
            status = aux as IStatus;
            worker = aux as IWorker;
            aliveBeing = aux as IAliveBeing;
            selectable = aux as ISelectable;
        }
        return result;
    }

    private bool setInterfacesByBuilding() 
    {
        bool result = false;

        object aux = this.gameObject.GetComponent<IBulding>();
        if (aux != null)
        {
            result = true;
            status = aux as IStatus;
            worker = aux as IWorker;
            building = aux as IBulding;
            selectable = aux as ISelectable;
        }
        return result;
    }




    // Update is called once per frame
    void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button "+status.GetStatus()))
        //    print("You clicked the button!");
        ////Debug.log("ongui");
        if (IsSelected())
        {//
         //  GUIStyle style = new GUIStyle();
         //  style.normal.textColor = Color.cyan;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.contentColor = Color.cyan;
            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 250, 100), printMessage());
            drawHealthGUIBar();
        }
       
      
    }

    private bool IsSelected()
    {
        return selectable != null && selectable.IsSelected;
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

    public void drawHealthGUIBar()
    {
        var sqwidth = 100;
        var sqheight = 10;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        screenPos += new Vector3(-(sqwidth / 2),30);

   
        GUI.DrawTexture(new Rect(screenPos.x , Screen.height - screenPos.y , sqwidth, sqheight), textureHealt);

   
        var rect = new Rect(new Vector2(screenPos.x, Screen.height - screenPos.y),new Vector2(sqwidth * GetHealthReason(), 10));
        GUI.DrawTexture(rect, textureCurrentHealt);
    }


    public float GetHealthReason()
    {
        if (IsAliveBeing)
            return aliveBeing.GetHealthReason(); 
        else
            return building.CurrentBuiltAmount / building.TotalBuiltAmount;
    }

}
