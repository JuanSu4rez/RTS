using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName ="New Bulding info", menuName = "Buldings info")]
public class BuldingsInfo : ScriptableObject
{
    [SerializeField]
    private BuldingInfo[] _BuldingInformation;

    public BuldingInfo[] BuldingInformation
    {
        get
        {
            return _BuldingInformation;
        }
        set
        {

        }
    }

    public BuldingsInfo()
    {
        var myEnumMemberCount = System.Enum.GetNames(typeof(Buildings)).Length;
        _BuldingInformation = new BuldingInfo[myEnumMemberCount];
        for (int i =0;i< _BuldingInformation.Length;i++)
        {
            Debug.Log(((Buildings)i).ToString());

            _BuldingInformation[i] = new BuldingInfo((Buildings)i);
        }
    }


    public BuldingInfo GetBuldingInfo(Buildings type)
    {
        return _BuldingInformation[(int)type];
    }
 

}
[System.Serializable]
public class BuldingInfo
{
    [SerializeField]
    private  Buildings _bulding;
    
    public Buildings Building
    {
        get
        {
            return _bulding;
        }
        set
        {

        }
    }

  


    [SerializeField]
    public  string Description;

    [SerializeField]
    private BuldingTypes BuldingType;

    [SerializeField]
    private Ages Initialage;

    [SerializeField]
    private bool Enabled;

    [SerializeField]
    private List<Cost> Cost;


    public BuldingInfo(Buildings _building)
    {

        this._bulding = _building;

        Cost = new List<Cost>();

        Description = "";

        Initialage = Ages.CERO;

        Enabled = true;
    }
}
