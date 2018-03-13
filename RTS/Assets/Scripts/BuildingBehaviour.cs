using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour, IBulding, IStatus, ISelectable
{

    [SerializeField]
    private Team team;

    public Team Team { get { return team; } set { team = value; } }


    private float LastCurrentBuiltAmount;

    [SerializeField]
    private float resistence;

    public float Resistence { get { return resistence; } set { resistence = value; } }

    [SerializeField]
    private float currentBuiltAmount;

    [SerializeField]
    public float CurrentBuiltAmount { get { return currentBuiltAmount; } set { currentBuiltAmount = value; } }

    [SerializeField]
    private float totalBuiltAmount;

    [SerializeField]
    public float TotalBuiltAmount { get { return totalBuiltAmount; } set { totalBuiltAmount = value; } }


    [SerializeField]
    private BuildingStates _State;

    [SerializeField]
    public BuildingStates State { get { return _State; } set { _State = value; } }

    [SerializeField]
    private Buildings _building;

    public Buildings Building { get { return _building; } set { _building = value; } }



    public bool IsSelected { get; set; }






    // Use this for initialization
    void Start()
    {
      

        if (State == BuildingStates._Fundational)
        {

            CurrentBuiltAmount = 5;
            LastCurrentBuiltAmount = CurrentBuiltAmount;
            TotalBuiltAmount = 200;
            Resistence = 0.3f;
        }

    }

    // Update is called once per frame
    void Update()
    {

        switch (State)
        {
            case BuildingStates.Building:
                if (CurrentBuiltAmount >= TotalBuiltAmount)
                {
                    State = BuildingStates.Built;
                    //TODO send a notification indicating that a building has been Built

                    SetBulding();
                    DisabledTrackingStatus();
                }
                else
                {
                    EnabledTrackingStatus();
                }
                break;
            case BuildingStates.Built:

                if (CurrentBuiltAmount < TotalBuiltAmount)
                {

                    State = BuildingStates.Destroying;
                    //TODO send a notification indicating that a building has been Built
                }

                break;

            case BuildingStates.Destroying:


                if (CurrentBuiltAmount <= 0)
                {
                    State = BuildingStates.Destroyed;
                    //TODO send a notification indicating that a building has been Destroyed
                }

                if (CurrentBuiltAmount >= LastCurrentBuiltAmount)
                {
                    State = BuildingStates.Repairing;
                }


                break;
            case BuildingStates.Destroyed:

                Destroy(this.gameObject);
                break;
            case BuildingStates._Fundational:
                //this state represents the Building fundations 

                break;
            case BuildingStates.Repairing:
                if (CurrentBuiltAmount >= TotalBuiltAmount)
                {
                    State = BuildingStates.Built;
                    //TODO send a notification indicating that a building has been Built

                    SetBulding();
                }
                break;
        }
        LastCurrentBuiltAmount = CurrentBuiltAmount;
    }

    public void InitBuilding()
    {
        if (State == BuildingStates._Fundational)
            State = BuildingStates.Building;
    }

    private void EnabledTrackingStatus()
    {
        var status = this.gameObject.GetComponent<TrackingStatus>();
        if (status != null && !status.enabled)
            status.enabled = true;
    }

    private void DisabledTrackingStatus()
    {
        var status = this.gameObject.GetComponent<TrackingStatus>();
        if (status != null && !status.enabled)
            status.enabled = true;
    }

    private void SetBulding()
    {
        //change sprite or model
        //So far just increase the primitive
        this.gameObject.transform.localScale = new Vector3(5, 5, 5);



    }



    public void AddCurrentBuiltAmount(float BuiltAmount)
    {
        var aux = CurrentBuiltAmount + BuiltAmount;
        if (aux >= TotalBuiltAmount)
            aux = TotalBuiltAmount;

        CurrentBuiltAmount = aux;
    }

    public void AddDamage(float damage)
    {
        var aux = CurrentBuiltAmount - damage;
        
        CurrentBuiltAmount = aux <= 0?0 : aux;
    }

    public bool IsOk()
    {
        return State != BuildingStates.Destroyed;
    }

    public bool IsBulding()
    {
        return State == BuildingStates.Building || State == BuildingStates.Repairing;
    }

    public bool CheckState(BuildingStates _state)
    {
        return State == _state;
    }



    public string GetStatus()
    {
        var result = "Bulding " + this.State.ToString() + ", CurrentBuiltAmount = " + this.CurrentBuiltAmount;
        return result;
    }
}
