using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum CameraMovementType
{
    Type1,
    Type2
    
}
public enum CitizenTransformChilden
{
   One,
   Two,
   Axe ,
   Pick,
   Hammer,
   Gathered_Gold,
   Gathered_Meat,
   Gathered_Wood
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
    CarryingGold, //  10
    CarryingWood,  //11
    CarryingMeat,  //12
    Dying1,         //13
    Dying2          //14
}

public enum SoldierAnimationStates
{
    Attacking,  //  0        
    Idle,       //  1
    None,       //  2    
    Walking,    //  3
    Dying1,     //  4
    Dying2      //  5
}




public enum CitizenTaskStates
{
    _None,
   Carrying,
   Doing,
   OnTheWay,
   
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
    IsSettingOnScene,
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
    _none,
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

    public static int Ordinal(this Enum rec) {

       
        return Convert.ToInt32(rec);
    }

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


    public static Buildings GetBuildingFromResource(this Resources rec) {

        Buildings buld = Buildings._none;
        switch (rec) {
            case Resources.Food:
                buld =  Buildings.Farm;

                break;
            case Resources.Gold:
                buld = Buildings.MiningCamp;
                break;
            case Resources.None:
                break;
            case Resources.Rock:
                buld = Buildings.MiningCamp;
                break;
            case Resources.Wood:
                buld = Buildings.Farm;
                break;
        }

        return buld;
    }



    public static string GetPath(this AssetTypes AssetType)
    {
        var result = "";
        switch (AssetType)
        {
            case AssetTypes.THREED:
                result = "3D"+ System.IO.Path.DirectorySeparatorChar;
                break;
            case AssetTypes.TWOD:
                result = "2D" + System.IO.Path.DirectorySeparatorChar;
                break;
            case AssetTypes.MINIMAL:
                result = "Minimal" + System.IO.Path.DirectorySeparatorChar;
                break;
        }

        return result;
    }



}

