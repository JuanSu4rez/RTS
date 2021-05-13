using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public bool debugGrid = false;
    public static GridScript gridScript;

    public Grid gird = null;



    private void Awake() {
        if (gridScript == null) {
            gridScript = this;
        }
        else {

            Destroy(this);
        }

        gird = new Grid(10, 10, 1, Vector3.zero+(Vector3.up*0.1f), this.debugGrid);
    }

    // Start is called before the first frame update
    void Start()
    {
  

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
