using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISelectable
{
    bool IsSelected { get; set; }

}

public interface IAliveBeing
{
    bool IsAlive();
    float GetCurrentHealth();
    float GetHealth();
    float GetHealthReason();
}

public interface IFigther{
    float AttackPower { get; set; }
    float AttackRange { get; set; }
    float DefensePower { get; set; }
}

public interface IWorker {
    float ResourceCapacity { get; set; }
    float CurrentAmountResouce { get; set; }
    float BuildingSpeed { get; set; }
    float GatheringSpeed { get; set; }
    Resources CurrentResource { get; set; }
}

public interface IBulding
{
    float Resistence { get; set; }
    float TotalBuiltAmount { get; set; }
    float CurrentBuiltAmount { get; }
    BuildingStates State { get; set; }
    Buildings Building { get; set; }

}

public interface IResource {
    float ResourceAmount { get; set; }
}

public interface IStatus {
    string GetStatus();
}

public interface ITeamable {
    Team Team { get; set; }
}

public interface IDamagable {
    void AddDamage(float damage);
}


