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

