using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour, IBulding, IStatus, ISelectable, IDamagable,ITeamable_v1
{

    [SerializeField]
    private Team team;

    public Team Team { get { return team; } set { team = value;
            changeColor();
        } }




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

   // private IGameFacade facade = null;

    private float yscale = 0;

    private float fundationalyscale = 1;
    [SerializeField]
    private float DefaultYposition = 0;

    // Use this for initialization

    void Awake()
    {
     
    }
    void OnEnable()
    {
        SetFundationalBuildingData();
      
    }
    void Start()
    {
      
     

        changeColor();

       // facade = GameScript.GetFacade(team);
        //if (facade != null)
          //  facade.AddBuilding(this.gameObject, _building);

        GameScript.AddBuiding(TeamId(), this);
    }
    
  

    public void SetFundationalBuildingData()
    {
        if (State == BuildingStates._Fundational)
        {
            CurrentBuiltAmount = 5;
            LastCurrentBuiltAmount = CurrentBuiltAmount;
            TotalBuiltAmount = 200;
            Resistence = 0.3f;
            yscale = this.transform.localScale.y;
            this.transform.localScale = new Vector3(this.transform.localScale.x, fundationalyscale, this.transform.localScale.z);
        }
    }

    public virtual void changeColor() {
        Utils.ChangeColorBuildingWB(gameObject, team);
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
            case BuildingStates._Fundational:
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

              
                // facade.RemoveBuilding(this.gameObject, _building);
                //TODO llamar remove bulding
                Destroy(this.gameObject);
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
        this.gameObject.transform.localScale = new Vector3(this.transform.localScale.x, yscale, this.transform.localScale.z);

       

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

        if (CurrentBuiltAmount <= 0)
        {
            State = BuildingStates.Destroying;
          
        }
    }

    public bool IsOk()
    {
        return State != BuildingStates.Destroyed;
    }

    public bool IsBulding()
    {
        return State == BuildingStates._Fundational  || State == BuildingStates.Building || State == BuildingStates.Repairing;
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

    public int IdTeam = 0;
    public int TeamId() {
        //todo hay que remover los team de los script
        return Team != null ? Team.Id : 0;
    }

}
