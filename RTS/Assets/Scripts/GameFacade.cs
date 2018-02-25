using UnityEngine;
using System.Collections;

public interface IGameFacade
{

    Ages GetCurrentAge();

    void AddNumberOfCitizens(int amount = 1);

    bool HasRequiredResources(Units type);

    bool HasRequiredResources(Buildings type);

    //bool HasRequiredResourcesToRepair(BuildingBehaviour buildingBehaviour);

    void AddResources(Resources type, float amount);

    bool DiscountResources(Resources type, float amount);

    GameObject FindNearResource(Vector3 player);

    GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource);

    BuldingInfo GetBuldingInfo(Buildings type);
}


public class GameFacade : ScriptableObject, IGameFacade
{
    public Player Player { get; set; }

    public BuldingsInfo BuldingsInfo { get; set; }

    public void AddResources(Resources type, float amount)
    {
        var ramount = Player.GetResourceAmount(type);
        ramount.AddResource(amount);
    }

    public bool DiscountResources(Resources type, float amount)
    {
        throw new System.NotImplementedException();
    }

    public GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource)
    {
        throw new System.NotImplementedException();
    }

    public GameObject FindNearResource(Vector3 player)
    {
        throw new System.NotImplementedException();
    }


    public bool HasRequiredResources(Units type)
    {
        throw new System.NotImplementedException();
    }

    public bool HasRequiredResources(Buildings type)
    {
        throw new System.NotImplementedException();
    }


    public BuldingInfo GetBuldingInfo(Buildings type)
    {
      return   BuldingsInfo.GetBuldingInfo(type);
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
        if(_amount>0)
            Amount += _amount;
    }

    public void DiscountResource(float _amount)
    {
        if (_amount < 0)
            Amount -= _amount;
    }
}