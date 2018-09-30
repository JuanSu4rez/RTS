﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum CitizenTransformChilden
{
   One,
   Two,
   Pick ,
   Axe 
}

public enum CitizeAnimationStates
{
    Attacking,  //  0
    Building,   //  1
    Died,       //  2
    Escaping,   //  3
    Gathering,  //  4    
    Idle,       //  5
    None,       //  6    
    Walking ,    //  7
    Gold     ,   //  8
    Wood      ,  //  9
    Carrying   , //  10
}

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

public enum BuldingTypes
{
    Civilian,
    Military
}


public enum Units {
    Citizen,
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
    TWOD,
    THREED
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

