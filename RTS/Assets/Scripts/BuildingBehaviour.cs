using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour, IBulding{

    private float LastCurrentBuiltAmount;

    public float Resistence {get;set;}
    public float CurrentBuiltAmount { get ; set ; }
    public float TotalBuiltAmount {get;set;}

    [SerializeField]
    public BuildingStates State { get; set; }
    [SerializeField]
    public BuildingType Type { get; set; }

    [SerializeField]
    private List<Cost> Costs;




    // Use this for initialization
    void Start () {
        State = BuildingStates.Building;
        Type = BuildingType.House;
        CurrentBuiltAmount = 5;
        TotalBuiltAmount = 200;
        Resistence = 0.3f;
    }
	
	// Update is called once per frame
	void Update ()
    {
     
        switch (State)
        {
            case BuildingStates.Building:
                if(CurrentBuiltAmount >= TotalBuiltAmount)
                {
                    State = BuildingStates.Built;
                    //TODO send a notification indicating that a building has been Built

                    SetBulding();
                }
                break;
            case BuildingStates.Built:

                if (CurrentBuiltAmount < TotalBuiltAmount )
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

                if(CurrentBuiltAmount >= LastCurrentBuiltAmount)
                {
                    State = BuildingStates.Repairing;
                }


                break;
            case BuildingStates.Destroyed:
            
                Destroy(this.gameObject);
                break;
            case BuildingStates.Fundational:
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

    private void SetBulding()
    {
        //change sprite or model
        //So far just increase the primitive
        this.gameObject.transform.localScale = new Vector3(5, 5, 5);
        

       
    }



    public void AddCurrentBuiltAmount(float BuiltAmount )
    {
        var aux = CurrentBuiltAmount + BuiltAmount;
        if (aux >= TotalBuiltAmount)
            aux = TotalBuiltAmount;

        CurrentBuiltAmount = aux;
    }

    public void AddDamage(float damage)
    {
        var aux = CurrentBuiltAmount-  damage;
        if (aux <= 0)
            aux = 0;
        CurrentBuiltAmount = aux;
    }

    public bool IsOk()
    {
        return State != BuildingStates.Destroyed;
    }

    public bool IsBulding()
    {
        return State == BuildingStates.Building || State == BuildingStates.Repairing;
    }

    public bool CheckState(BuildingStates _state )
    {
        return State == _state;
    }

    public List<Cost> GetCosts()
    {
        return Costs;
    }
}
