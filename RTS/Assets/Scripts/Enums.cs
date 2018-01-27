using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum Resources{
       Food,
       Gold,
       Rock,
       Wood,    
    }

    public enum Citizen{
        Attacking,
        Building,
        Died,
        Escaping,
        Gathering,
        Idle,
        Walking
    }

    public enum Building {
        Builded,
        Building,
        Destroying,        
        Destroyed,
        Repairing
    }
}
