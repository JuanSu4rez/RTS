using UnityEngine;
using System.Collections;
using System;

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

    GameObject FindNearResource(Vector3 player, Resources resource);

    GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource);

    BuildingInfo GetBuldingInfo(Buildings type);


    bool CanCreateBuilding(Buildings building);


    bool CanCreateUnit(Units unit);

    Team Team { get; }

    AssetTypes Assettype { get; }

    GameResource GameResource { get; }
    String FacadeName { get; }

    bool ValidateDiplomacy(Team team, Postures posture);

    BuildingsInfo BuildingsInfo { get;  }


}


public class GameFacade : ScriptableObject, IGameFacade
{
    public AssetTypes Assettype { get; internal set; }
    public String FacadeName { get; internal set; }


    private GameResource gameResource = null;
    public GameResource GameResource
    {

        get
        {
            if (gameResource == null)
            {
                gameResource = new GameResource(Assettype);
            }
            return gameResource;
        }
    }

    public BuildingsInfo BuildingsInfo { get; set; }

    public Player Player { get; set; }

    public Team Team { get; internal set; }

    public UnitsInfo UnitsInfo { get; set; }

    public Diplomacy[] Diplomacies { get; set; }

    public void AddResources(Resources type, float amount)
    {
        var ramount = Player.GetResourceAmount(type);
        ramount.AddResource(amount);
    }

    public void DiscountResources(Resources type, float amount)
    {
        ResourceAmount resourceAmount = Player.GetResourceAmount(type);
        if (resourceAmount.Amount >= amount)
            resourceAmount.DiscountResource(amount);
        //Debug.Log(" - START DiscountResources - tipo : " + type + " Amount jugador " + resourceAmount.Amount);
    }

    public GameObject FindNearBuldingToDeposit(Vector3 player, Resources resource)
    {
        throw new System.NotImplementedException();
    }

    public GameObject FindNearResource(Vector3 player, Resources resource)
    {
        throw new System.NotImplementedException();
    }


    public bool HasRequiredResources(Units type)
    {
        UnitInfo unitInfo = UnitsInfo.GetUnitInfo(type);
        var UnitCosts = unitInfo.Costs;

        for (int i = 0; i < UnitCosts.Count; i++)
        {
            ResourceAmount resourceAmount = Player.GetResourceAmount(UnitCosts[i].Resource);
            if (resourceAmount.Amount < UnitCosts[i].Amount)
            {
                return false;
            }
        }
        return true;
    }

    public bool HasRequiredResources(Buildings type)
    {
        BuildingInfo buildingInfo = BuildingsInfo.GetBuldingInfo(type);
        var Costs = buildingInfo.Costs;

        for (int i = 0; i < Costs.Count; i++)
        {
            ResourceAmount resourceAmount = Player.GetResourceAmount(Costs[i].Resource);
            if (resourceAmount.Amount < Costs[i].Amount)
            {
                return false;
            }
        }
        return true;
    }


    public BuildingInfo GetBuldingInfo(Buildings type)
    {
        return BuildingsInfo.GetBuldingInfo(type);
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

    public void DiscountResources(Units Unit)
    {
        UnitInfo unitInfo = UnitsInfo.GetUnitInfo(Unit);
        var unitCosts = unitInfo.Costs;

        for (int i = 0; i < unitCosts.Count; i++)
        {
            DiscountResources(unitCosts[i].Resource, unitCosts[i].Amount);
        }
    }

    public void DiscountResources(Buildings building)
    {
        BuildingInfo buildingInfo = BuildingsInfo.GetBuldingInfo(building);
        var unitCosts = buildingInfo.Costs;

        for (int i = 0; i < unitCosts.Count; i++)
        {
            DiscountResources(unitCosts[i].Resource, unitCosts[i].Amount);
        }
    }

    public bool CanCreateBuilding(Buildings building)
    {
        var result = HasRequiredResources(building);
        if (result)
        {
            DiscountResources(building);
        }

        return result;
    }

    public bool CanCreateUnit(Units unit)
    {
        var result = HasRequiredResources(unit);
        if (result)
        {
            DiscountResources(unit);
        }

        return result;
    }

    public bool ValidateDiplomacy(Team team, Postures posture)
    {
        try
        {


            if (team.Id != this.Team.Id)
                return Diplomacies[team.Id].Posture == posture;
            else
                return false;
        }
        catch (UnityException ex)
        {
            Debug.Log("ERROR ValidateDiplomacy" +ex.Message + " " + FacadeName);
          
        }
        catch (Exception ex)
        {
            Debug.Log("ERROR ValidateDiplomacy" + ex.Message + " " + FacadeName);

        }
        return false;
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

        _NumberofCitizens = 3;
    }

    public void SetData(TeamData data)
    {
        GoldAmount = new ResourceAmount(data.GoldAmount);

        RockAmount = new ResourceAmount(data.RockAmount);

        WoodAmount = new ResourceAmount(data.WoodAmount);

        FoodAmount = new ResourceAmount(data.FoodAmount);

        CurrentAge = data.CurrentAge;

        _NumberofCitizens = data.NumberofCitizens;
    }



    public ResourceAmount GetResourceAmount(Resources resource)
    {
        ResourceAmount result = null;
        switch (resource)
        {
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
                Debug.Log("ResourceAmount default" + resource);
                break;
        }

        return result;
    }

}
[System.Serializable]
public class Diplomacy
{
    [SerializeField]
    private Postures posture;
    public Postures Posture { get { return posture; } }
    [SerializeField]
    public Team team;
    [SerializeField]
    public Team Team { get { return team; } }


    public Diplomacy(Team team, Postures posture)
    {
        this.team = team;
        this.posture = posture;
    }
}

[System.Serializable]
public class ResourceAmount
{
    [SerializeField]
    public Resources Resource;

    [SerializeField]
    public float Amount;


    public ResourceAmount(Resources _resource)
    {
        Resource = _resource;
        Amount = 100;
    }
    public ResourceAmount(Resources _resource, float _amount)
    {
        Resource = _resource;
        Amount = _amount;
    }

    public ResourceAmount(ResourceAmount resourceAmount)
    {
        Resource = resourceAmount.Resource;
        Amount = resourceAmount.Amount;
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

[CreateAssetMenu(fileName = "New Team", menuName = "Team")]
public class Team : ScriptableObject
{
    [SerializeField]
    private int id;

    public int Id { get { return this.id; } }

    [SerializeField]
    private string teamname;

    public string TeamName { get { return this.teamname; } }

    [SerializeField]
    private Color color;

    public Color Color { get { return this.color; } }

    [SerializeField]
    private TeamData initialteamdata;

    public TeamData InitalTeamData { get { return initialteamdata; } }


    public Team()
    {

        this.id = 0;

        this.teamname = "";

        this.color = Color.black;

        initialteamdata = new TeamData();

    }


    public Team(int Id, string Name, Color color)
    {

        this.id = Id;

        this.teamname = Name;

        this.color = color;

        initialteamdata = new TeamData();

    }
}

[System.Serializable]
public class TeamData
{
    public ResourceAmount GoldAmount;

    public ResourceAmount RockAmount;

    public ResourceAmount WoodAmount;

    public ResourceAmount FoodAmount;

    [SerializeField]
    private int _NumberofCitizens;

    public int NumberofCitizens { get { return _NumberofCitizens; } set { _NumberofCitizens = value; } }

    public Ages CurrentAge { get; set; }

    public TeamData()
    {
        GoldAmount = new ResourceAmount(Resources.Gold);

        RockAmount = new ResourceAmount(Resources.Rock);

        WoodAmount = new ResourceAmount(Resources.Wood);

        FoodAmount = new ResourceAmount(Resources.Food);

        _NumberofCitizens = 3;

        CurrentAge = Ages.I;
    }
}

