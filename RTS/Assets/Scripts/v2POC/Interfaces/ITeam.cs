﻿using UnityEngine;
using UnityEditor;
using V2.Interfaces.Task;
using V2.Enums;

namespace V2.Interfaces
{
    public interface ITeam {
        TeamsList Team { get; set; }
    }
}