using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;

[Serializable]
public class PlayerController  {



    public ResourceAmount GoldAmount;

    public ResourceAmount RockAmount;

    public ResourceAmount WoodAmount;

    public ResourceAmount FoodAmount;

    private List<BuildingBehaviour> buildings = new List<BuildingBehaviour>(20);

    public string Name;

    public Color color;

  
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
                ////Debug.Log("ResourceAmount default" + resource);
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
                ////Debug.Log("ResourceAmount default" + resource);
                break;
        }

        return result;
    }

    private int numberofurbancenters = 0;

    public void AddBuilding(BuildingBehaviour building) {

        if (buildings.Contains(building)) {
            return;
        }

        if (buildings.Count > 0 && building.Building == Buildings.UrbanCenter) {

            buildings.Insert(numberofurbancenters, building);
            numberofurbancenters++;
        }
        else {
            buildings.Add(building);
        }
    }

    public GameObject FindResoruceBuidingToDeposit(Vector3 position ,Resources resource) {

        GameObject result = null;

        if (!UtilsCollections.IsNullOrEmpty(buildings)) {



            Buildings buidingtofind = resource.GetBuildingFromResource();

            float currentdistance = 0;

            buildings.ForEach(p => {

                if (p.Building == buidingtofind || p.Building == Buildings.UrbanCenter) {

                    if (result == null) {

                        result = p.gameObject;
                        currentdistance = Mathf.Abs( Vector3.Distance(position, p.gameObject.transform.position));
                    }
                    else 
                    { 
                      var newdistance  = Mathf.Abs(Vector3.Distance(position, p.gameObject.transform.position));
                        if (newdistance < currentdistance) {

                            result = p.gameObject;
                            currentdistance = newdistance;

                        }

                    }
                
                }

               });


        }
        return result;

    }

}