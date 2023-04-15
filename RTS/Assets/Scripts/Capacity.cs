using UnityEngine;
using System.Collections;

[System.Serializable]
public class Capacity {

    [SerializeField]
    public int Current;
    [SerializeField]
    public int Limit;

    public bool HasReachedhMaxCapacity => Current >= Limit;

    public Capacity() {
     
    }
}
