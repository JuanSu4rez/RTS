﻿using System;
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

public enum SoldierStates
{
    _None,
    Attacking,
    Died,
    Idle,    
    Walking
}

public enum MilitaryTaskType
{
    Attack,
    Patroll,
}

public enum BuildingStates
{
    //INDICATES DEFAULT STATUS
    _Fundational,
    Built,
    Building,
    Destroying,
    Destroyed,
    Repairing
}

public enum CameraStates
{
    //There is no selection made
    None,
    // one to many by mousewheel or double click
    UnitsSelection,
    // one to many by mousewheel or  double click
    BuildingsSelection,
   
}


/// <summary>
/// Types of buildings that can be created by the citizens
/// Taken from
/// http://ageofempires.wikia.com/wiki/Buildings_(Age_of_Empires_II)
/// </summary>
public enum Buildings
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
    //TownCenter,
    University,
    UrbanCenter,
    WatchTower,
    Wonder
}

public enum UnitsTypes
{
    Civilian,
    Military
}

public enum CameraSelectionTypes
{
    _None,
    //onlycitizens
    Citizen,
    //onlymilitar
    Military,
    //both citizens and people
    People,
    //only buldings
    Bulding
}


public enum Units {
    Archer,
    Citizen,
    Knight,
    SwordMan
}

public enum Ages
{
    CERO,
    I,
    II,
    III,
    IV,
    VI,
    VII,
    VIII,

}



public enum AssetTypes
{
    NONE,
    MINIMAL,
    TWOD,
    THREED
}


public enum Postures
{
    Ally,
    Enemy,
    Neutral

}




    public static class ExtensionMethodsResources
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


    public static string GetPath(this AssetTypes AssetType)
    {
        var result = "";
        switch (AssetType)
        {
            case AssetTypes.THREED:
                result = "3d"+ System.IO.Path.DirectorySeparatorChar;
                break;
            case AssetTypes.TWOD:
                result = "2d" + System.IO.Path.DirectorySeparatorChar;
                break;
            case AssetTypes.MINIMAL:
                result = "Minimal" + System.IO.Path.DirectorySeparatorChar;
                break;
        }

        return result;
    }



}

