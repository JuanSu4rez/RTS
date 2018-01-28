using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public enum Resources{
       Food,
       Gold,
       None,
       Rock,
       Wood,    
    }

    public enum CitizenStates{
        Attacking,
        Building,
        Died,
        Escaping,
        Gathering,
        Idle,
        None,
        Walking
    }

    public enum BuildingStates {
        Builded,
        Building,
        Destroying,        
        Destroyed,
        Repairing
    }


public static class ExtensionMethods {

    public static Resources GetResource(this Resources rec, string param) {

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

