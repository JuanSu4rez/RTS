using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGui : ScriptableObject {
    	
    public void ShowGUI(GameObject selectedBuilding) {

        if (selectedBuilding == null)
            return;

        string buildingName = selectedBuilding.name;
        BuildingType buildingType = (BuildingType) System.Enum.Parse(typeof(BuildingType), buildingName);

        switch (buildingType)
        {
            case BuildingType.Barracks:
                if (GUI.Button(new Rect(0, Screen.height - 100, 100, 100), "Crear Soldado")){
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null){
                        //TODO validate resources 
                        //Debug.log("aca llego esta mierda");
                        behaviour.addUnitToQueue(UnitType.SwordMan);
                    }                    
                }
                break;

            case BuildingType.UrbanCenter:
                if (GUI.Button(new Rect(0, Screen.height - 100, 100, 100), "Crear Aldeano"))
                {
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null)
                    {
                        //TODO validate resources 
                        //Debug.log("aca llego esta mierda");
                        behaviour.addUnitToQueue(UnitType.Citizen);
                    }
                }
                break;

            default:
                break;
        }

    }

    public void renderButtons(BuildingType buildingType) {
        
    }
}
