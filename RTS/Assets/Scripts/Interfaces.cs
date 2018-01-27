using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAliveBeing {
    bool IsAlive { get; set; }
    float Life { get; set; }
}

public interface IFigther{
    float AttackPower { get; set; }
    float DefensePower { get; set; }
}

public interface IWorker {
    float ResourceCapacity { get; set; }
    float BuildingSpeed { get; set; }
    float GatheringSpeed { get; set; }
    Resources Resource { get; set; }
}

public interface IResource {
    float ResourceAmount { get; set; }
}