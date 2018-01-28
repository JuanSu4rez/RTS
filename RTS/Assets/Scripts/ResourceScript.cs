using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour {

    public Resources resourceType;
    public float totalAmount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!HasResource()) {
            Destroy(this.gameObject);
        }
	}

    public float DiscountAmount(float newAmount) {

        if (totalAmount > newAmount){

            totalAmount -= newAmount;
            return newAmount;
        }
        else {
            var aux = totalAmount;
            totalAmount = 0;

            return aux;
        }
       
    }




    public bool HasResource()
    {
        return totalAmount > 0;
    }

}
