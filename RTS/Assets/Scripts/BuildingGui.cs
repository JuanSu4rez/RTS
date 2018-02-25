using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGui : ScriptableObject {

    private IGameFacade gameFacade;

    public BuildingGui() {
        gameFacade = GameScript.GetFacade();
    }

    public void ShowGUI(GameObject selectedBuilding) {
        gameFacade = GameScript.GetFacade();
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
                        if (gameFacade.HasRequiredResources(Units.SwordMan)) {
                            behaviour.addUnitToQueue(Units.SwordMan);
                            gameFacade.DiscountResources(Units.SwordMan);
                        }
                        else{
                            Debug.Log("No hay recursos suficientes");
                        }
                    }                    
                }
                break;

            case Buildings.UrbanCenter:
                if (GUI.Button(new Rect(0, Screen.height - 100, Screen.width, Screen.height - (Screen.height - 100)), "Crear Aldeano"))
                {
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null){
                        if (gameFacade.HasRequiredResources(Units.Citizen)) {
                            behaviour.addUnitToQueue(Units.Citizen);
                            gameFacade.DiscountResources(Units.Citizen);                            
                        }
                        else{
                            Debug.Log("No hay recursos suficientes");
                        }
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
