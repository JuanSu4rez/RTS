using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingStatus : MonoBehaviour {

    private IStatus status;
    private IWorker worker;
    private IBulding building;
    private IAliveBeing aliveBeing;
    public bool IsSelected { get; set; }

    private bool IsAliveBeing;


    private Texture2D textureHealt;

    private Texture2D textureCurrentHealt;
 

    // Use this for initialization
    void Start () {

        textureHealt = new Texture2D(1, 1);
        textureHealt.SetPixel(0, 0, Color.red);
        textureHealt.Apply();


        textureCurrentHealt = new Texture2D(1, 1);
        textureCurrentHealt.SetPixel(0, 0, Color.green);
        textureCurrentHealt.Apply();

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

        //Debug.log("Status " + status);
    }

    // Update is called once per frame
    void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button "+status.GetStatus()))
        //    print("You clicked the button!");
        ////Debug.log("ongui");
        if (IsSelected)
        {//
         //  GUIStyle style = new GUIStyle();
         //  style.normal.textColor = Color.cyan;
          Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 250, 100), printMessage());
          //  drawHealthGUIBar();
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
