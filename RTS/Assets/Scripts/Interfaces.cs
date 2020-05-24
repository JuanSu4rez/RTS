using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameAreas
{

    void Init();

    void RecalculateAreas( CameraStates state);

   
}



public interface IAliveBeing
{
    bool IsAlive();
    float GetCurrentHealth();
    float GetHealth();
    float GetHealthReason();
}

public interface IControlable_v1<T> where T : struct
{
    void SetPointToMove(Vector3 newPointToMove);
    void SetState(T newState);
    void ReleaseTask();
}

public interface IControlable {

     void SetTask(Task T);

     void EnableTask();

    void ReleaseTask();
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

public interface ITeamable_v1 {
    Team Team { get; set; }

    int TeamId();
}


public interface ITeamable {
    void SetTeam(int team);

    int TeamId();
}

public interface IDamagable {
    void AddDamage(float damage);
}


public interface IGui
{
     void UpdateGui(GameObject selectedGameObject );

     void ShowGUI(GameObject selectedGameObject);

     bool HasOptionSelected();
}



