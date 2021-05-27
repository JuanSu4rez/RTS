using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace V2.Enums.GUI
{
    [Flags]
    public enum MouseStates
    {
        _none = 0,
        Pressed = 1,
        Dragged = 2,
        
    }
}