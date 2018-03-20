using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGui : ScriptableObject, IGui
{

    private IGameFacade gameFacade;

    public BuildingGui() {
      
    }

    public void UpdateGui(GameObject selectedBuilding)
    {

    }

    public void ShowGUI(GameObject selectedBuilding) {
     
        if (selectedBuilding == null)
            return;

        var behavior = selectedBuilding.GetComponent<BuildingBehaviour>();
        if (behavior == null)
            return;

        gameFacade = GameScript.GetFacade(behavior.Team);
 


        Buildings buildingType = behavior.Building;

        switch (buildingType)
        {
            case Buildings.Barracks:
                if (GUI.Button(new Rect(0, Screen.height - 100, 100, Screen.height-( Screen.height-100)), "Crear Soldado")){
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
                if (GUI.Button(new Rect(100, Screen.height - 100, 100, Screen.height - (Screen.height - 100)), "Crear Arquero"))
                {
                    var behaviour = selectedBuilding.GetComponent<UnitCreationScript>();
                    if (behaviour != null)
                    {
                        if (gameFacade.HasRequiredResources(Units.Archer))
                        {
                            behaviour.addUnitToQueue(Units.Archer);
                            gameFacade.DiscountResources(Units.Archer);
                        }
                        else
                        {
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

    public bool HasOptionSelected()
    {
        return false;
    }
}
