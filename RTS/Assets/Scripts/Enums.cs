using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum Resources
{
    Food,
    Gold,
    None,
    Rock,
    Wood,
}

public enum CitizenStates
{
    Attacking,
    Building,
    Died,
    Escaping,
    Gathering,
    Idle,
    None,
    Walking
}

public enum BuildingStates
{
    Built,
    Building,
    Destroying,
    Destroyed,
    Fundational,
    Repairing
}

/// <summary>
/// Types of buildings that can be created by the citizens
/// Taken from
/// http://ageofempires.wikia.com/wiki/Buildings_(Age_of_Empires_II)
/// </summary>
public enum BuildingType
{
    ArcheryRange,
    Barracks,
    Blacksmith,
    BombardTower,
    Castle,
    Dock,
    Farm,
    Feitoria,
    FishTrap,
    FortifiedWall,
    Gate,
    GuardTower,
    Harbor,
    House,
    Keep,
    LumberCamp,
    Market,
    Mill,
    MiningCamp,
    Monastery,
    Outpost,
    PalisadeGate,
    PalisadeWall,
    SiegeWorkshop,
    Stable,
    StoneWall,
    //UrbanCenter
    TownCenter,
    University,
    WatchTower,
    Wonder
}



public static class ExtensionMethods
{

    public static Resources GetResource(this Resources rec, string param)
    {

        if (param.Equals("GoldMine"))
        {
            return Resources.Gold;
        }
        else if (param.Equals("Forest"))
        {
            return Resources.Wood;
        }
        else if (param.Equals("Rock"))
        {
            return Resources.Rock;
        }
        else if (param.Equals("Food"))
        {
            return Resources.Food;
        }
        return Resources.None;
    }



}

