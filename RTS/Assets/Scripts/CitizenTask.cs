using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class CitizenTask// : ScriptableObject
{
    public static readonly CitizenTask Empty = new CitizenTask();

    private Vector3 position;
    private GameObject gameobject ;

    private CitizenStates citizenLabor;
    private Resources resource;

    private BuildingBehaviour buildingBehaviour = null;
    private ResourceScript resourceScript = null;

    public Vector3 Position{ get { return position; } }
    public GameObject Gameobject { get { return gameobject; } }

  

    public CitizenStates CitizenLabor { get { return citizenLabor; } }
    public Resources Resource { get { return resource; } }

    public BuildingBehaviour BuildingBehaviour { get { return buildingBehaviour;} }
    public ResourceScript ResourceScript { get { return resourceScript; } }




    private CitizenTask()
    {

    }

    public CitizenTask(  Vector3 _position ,GameObject _gameobject, CitizenStates _citizenLabor, Resources resource)
    {
        this.position= _position;
        this.gameobject = _gameobject;
        this.citizenLabor = _citizenLabor;
        this.resource = resource;
        this.resourceScript = this.gameobject.GetComponent<ResourceScript>();
        //this.SetOnTheWayState();
    }

    private CitizenTask(Vector3 _position, GameObject _gameobject)
    {
        this.position = _position;
        this.gameobject = _gameobject;
        this.citizenLabor = CitizenStates.Building;
        this.buildingBehaviour = this.gameobject.GetComponent<BuildingBehaviour>();
        //this.SetOnTheWayState();
    }

    public bool IsTaskOnPorgress()
    {
        bool result = false;
        switch (this.citizenLabor)
        {
            case CitizenStates.Gathering:
                result = resourceScript != null && resourceScript.HasResource();

                break;
            case CitizenStates.Building:
                result = buildingBehaviour != null && buildingBehaviour.IsBulding();

                break;
        }

        return result;
    }

    public float DiscountResource(float v)
    {
        return resourceScript.DiscountAmount(v);
    }

 


    public static CitizenTask CitizenTaskBulding(Vector3 _position, GameObject _gameobject)
    {
        return new CitizenTask( _position,  _gameobject);
    }

    public void AddCurrentBuiltAmount(float buildingSpeed)
    {
        buildingBehaviour.AddCurrentBuiltAmount(buildingSpeed);
    }


    public static bool IsValidCitizenTask(CitizenTask citizentask)
    {
        return citizentask != null && citizentask != Empty;
    }
    public static bool IsEmpty(CitizenTask citizentask)
    {
        return citizentask == Empty;
    }

}