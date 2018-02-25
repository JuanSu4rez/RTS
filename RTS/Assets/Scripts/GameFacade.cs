﻿using UnityEngine;
using System.Collections;

public interface IGameFacade
{

    Ages GetCurrentAge();

    void AddNumberOfCitizens(int amount = 1);

    bool HasRequiredResources(Units type);

    bool HasRequiredResources(Buildings type);

    //bool HasRequiredResourcesToRepair(BuildingBehaviour buildingBehaviour);

    void AddResources(Resources type, float amount);

    void DiscountResources(Resources type, float amount);

    void DiscountResources(Units Unit);

    void DiscountResources(Buildings building);

    GameObject FindNearResource(Vector3 player);

    GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource);

    BuildingInfo GetBuldingInfo(Buildings type);
}


public class GameFacade : ScriptableObject, IGameFacade
{
    public Player Player { get; set; }

    public BuildingsInfo BuildingsInfo { get; set; }

    public UnitsInfo UnitsInfo { get; set; }

    public void AddResources(Resources type, float amount)
    {
        var ramount = Player.GetResourceAmount(type);
        ramount.AddResource(amount);
    }

    public void DiscountResources(Resources type, float amount)
    {
        
        ResourceAmount resourceAmount = Player.GetResourceAmount(type);
        if(resourceAmount.Amount >= amount)            
            resourceAmount.DiscountResource(amount);
        Debug.Log(" - START DiscountResources - tipo : " + type + " Amount jugador " + resourceAmount.Amount);
    }

    public GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource)
    {
        throw new System.NotImplementedException();
    }

    public GameObject FindNearResource(Vector3 player)
    {
        throw new System.NotImplementedException();
    }


    public bool HasRequiredResources(Units type){
        UnitInfo unitInfo = UnitsInfo.GetUnitInfo(type);
        var UnitCosts = unitInfo.Costs;

        for (int i = 0; i < UnitCosts.Count; i++){
            ResourceAmount resourceAmount = Player.GetResourceAmount(UnitCosts[i].Resource);
            if (resourceAmount.Amount < UnitCosts[i].Amount) {
                return false;
            }
        }
        return true;
    }

    public bool HasRequiredResources(Buildings type){
        BuildingInfo buildingInfo = BuildingsInfo.GetBuldingInfo(type);
        var Costs = buildingInfo.Costs;

        for (int i = 0; i < Costs.Count; i++){
            ResourceAmount resourceAmount = Player.GetResourceAmount(Costs[i].Resource);
            if (resourceAmount.Amount < Costs[i].Amount){
                return false;
            }
        }
        return true;
    }


    public BuildingInfo GetBuldingInfo(Buildings type)
    {
      return   BuildingsInfo.GetBuldingInfo(type);
    }

    public bool HasRequiredResourcesToRepair(BuildingBehaviour buildingBehaviour)
    {
        throw new System.NotImplementedException();
    }

    public void AddNumberOfCitizens(int amount = 1)
    {
        Player.NumberofCitizens += amount;
    }

    public Ages GetCurrentAge()
    {
        return Player.CurrentAge;
    }

    public void DiscountResources(Units Unit){
        UnitInfo unitInfo = UnitsInfo.GetUnitInfo(Unit);
        var unitCosts = unitInfo.Costs;

        for (int i = 0; i < unitCosts.Count; i++){            
            DiscountResources(unitCosts[i].Resource, unitCosts[i].Amount);
        }
    }

    public void DiscountResources(Buildings building)
    {
        BuildingInfo buildingInfo = BuildingsInfo.GetBuldingInfo(building);
        var unitCosts = buildingInfo.Costs;

        for (int i = 0; i < unitCosts.Count; i++) {
            DiscountResources(unitCosts[i].Resource, unitCosts[i].Amount);
        }
    }
}


public class Territory
{

}


[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class Player : ScriptableObject
{
    public ResourceAmount GoldAmount;

    public ResourceAmount RockAmount;

    public ResourceAmount WoodAmount;

    public ResourceAmount FoodAmount;
    [SerializeField]
    private int _NumberofCitizens;

    public int NumberofCitizens { get { return _NumberofCitizens; } set { _NumberofCitizens = value; } }

    public Ages CurrentAge { get; set; }

    public Player()
    {
        GoldAmount = new ResourceAmount(Resources.Gold, 0);

        RockAmount = new ResourceAmount(Resources.Rock, 0);

        WoodAmount = new ResourceAmount(Resources.Wood, 0);

        FoodAmount = new ResourceAmount(Resources.Food, 0);

        CurrentAge = Ages.I;
    }


    public ResourceAmount GetResourceAmount(Resources resource)
    {
        ResourceAmount result = null;
        switch (resource)
        {
            case  Resources.Food:
                result = FoodAmount;
                break;
            case  Resources.Gold:
                result = GoldAmount;
                break;
            case  Resources.Rock:
                result = RockAmount;
                break;
            case Resources.Wood:
                result = WoodAmount;
                break;
            default:
                Debug.Log("ResourceAmount default" + resource);
                break;
        }

        return result;
    }

}
[System.Serializable]
public class ResourceAmount
{
    [SerializeField]
    public Resources Resource;

    [SerializeField]
    public float Amount;

  

    public ResourceAmount(Resources _resource, float _amount)
    {
        Resource = _resource;
        Amount = _amount;
    }

    public void AddResource(float _amount)
    {
        //if(_amount>0)
            Amount += _amount;
    }

    public void DiscountResource(float _amount)
    {
        //if (_amount < 0)
            Amount -= _amount;
    }
}