using UnityEngine;
using System.Collections;
[System.Serializable]
public class Cost
{
    [SerializeField]
    private Resources _resource;

    public Resources Resource
    {
        get { return _resource; }
    }

    [SerializeField]
    private float _amount;

    public float Amount
    {
        get { return _amount; }
    }

    public Cost()
    {

    }
}

public class BuildingCostInfo
{

    public  Cost cost { get; set; }

    public BuildingType Type { get; set; }

    public BuildingCostInfo()
    {
        
    }
}
