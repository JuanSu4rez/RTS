using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGui : ScriptableObject {
    	
    public void ShowGUI(GameObject selectedBuilding) {

        if (selectedBuilding == null)
            return;

        string buildingName = selectedBuilding.name;
        Buildings buildingType = (Buildings) System.Enum.Parse(typeof(Buildings), buildingName);

        switch (buildingType)
        {
            case Buildings.Barracks:
                if (GUI.Button(new Rect(0, Screen.height - 100, Screen.width, Screen.height-( Screen.height-100)), "Crear Soldado")){
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null){
                        //TODO validate resources 
                        //Debug.log("aca llego esta mierda");
                        behaviour.addUnitToQueue(Units.SwordMan);
                    }                    
                }
                break;

            case Buildings.UrbanCenter:
                if (GUI.Button(new Rect(0, Screen.height - 100, Screen.width, Screen.height - (Screen.height - 100)), "Crear Aldeano"))
                {
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null)
                    {
                        //TODO validate resources 
                        //Debug.log("aca llego esta mierda");
                        behaviour.addUnitToQueue(Units.Citizen);
                    }
                }
                break;

            default:
                break;
        }

    }

    public void renderButtons(Buildings buildingType) {
        
    }
}
