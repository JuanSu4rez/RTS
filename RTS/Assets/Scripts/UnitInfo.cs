using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit info", menuName = "Units info")]
public class UnitsInfo : ScriptableObject {

    [SerializeField]
    private UnitInfo[] _UnitInformation;

    public UnitInfo[] UnitInformation {
        get { return _UnitInformation; }
    }

    public UnitsInfo() {
        int arraySize = System.Enum.GetNames(typeof(Units)).Length;
        _UnitInformation = new UnitInfo[arraySize];
        for (int i = 0; i < arraySize; i++){
            _UnitInformation[i] = new UnitInfo((Units)i);
        }
    }

    public UnitInfo GetUnitInfo(Units unitType) {
        return _UnitInformation[(int)unitType];
    }
}

[System.Serializable]
public class UnitInfo
{
    [SerializeField]
    private Units _units;

    public Units Unit
    {
        get
        {
            return _units;
        }
        set
        {

        }
    }

    [SerializeField]
    public string Description;

    [SerializeField]
    private Buildings developedBuilding;

    public Buildings DevelopedBuilding
    {
        get
        {
            return developedBuilding;
        }
    }


    [SerializeField]
    private UnitsTypes UnitType;

    [SerializeField]
    private Ages Initialage;

    [SerializeField]
    private bool Enabled;

    [SerializeField]
    private List<Cost> Cost;

    public List<Cost> Costs
    {
        get { return Cost; }
    }

    [SerializeField]
    private Texture2D  icon;


    [SerializeField]
    private UnitsTypes BuldingType;

    public UnitInfo(Units _units)
    {

        this._units = _units;

        Cost = new List<Cost>();

        Description = "";

        Initialage = Ages.CERO;

        Enabled = true;
    }
}

