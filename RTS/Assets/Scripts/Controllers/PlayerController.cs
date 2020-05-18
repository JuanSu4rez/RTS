using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class PlayerController  {



    public ResourceAmount GoldAmount;

    public ResourceAmount RockAmount;

    public ResourceAmount WoodAmount;

    public ResourceAmount FoodAmount;

  
    [SerializeField]
    private int _NumberofUnits;

    public int NumberofUnits { get { return _NumberofUnits; } set { _NumberofUnits = value; } }


    [SerializeField]
    private int _UnitsCapacity;

    public int UnitsCapacity { get { return _UnitsCapacity; } set { _UnitsCapacity = value; } }

    public Ages CurrentAge { get; set; }



    public PlayerController() {

        
        GoldAmount = new ResourceAmount(Resources.Gold, 0);

        RockAmount = new ResourceAmount(Resources.Rock, 0);

        WoodAmount = new ResourceAmount(Resources.Wood, 0);

        FoodAmount = new ResourceAmount(Resources.Food, 0);

        CurrentAge = Ages.I;

        _NumberofUnits = 0;

        _UnitsCapacity = 0;
   

    }


    public void AddResourceAmount(Resources resource, float amount) {
   
        switch (resource) {
            case Resources.Food:
                FoodAmount.AddResource(amount);
                break;
            case Resources.Gold:
                GoldAmount.AddResource(amount);
                break;
            case Resources.Rock:
                RockAmount.AddResource(amount);
                break;
            case Resources.Wood:
                WoodAmount.AddResource(amount);
                break;
            default:
                //Debug.Log("ResourceAmount default" + resource);
                break;
        }

    
    }


    public ResourceAmount GetResourceAmount(Resources resource) {
        ResourceAmount result = null;
        switch (resource) {
            case Resources.Food:
                result = FoodAmount;
                break;
            case Resources.Gold:
                result = GoldAmount;
                break;
            case Resources.Rock:
                result = RockAmount;
                break;
            case Resources.Wood:
                result = WoodAmount;
                break;
            default:
                //Debug.Log("ResourceAmount default" + resource);
                break;
        }

        return result;
    }

}