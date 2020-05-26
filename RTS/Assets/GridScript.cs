using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{

    public static GridScript gridScript;

    public Grid gird = null;



    private void Awake() {
        if (gridScript == null) {
            gridScript = this;
        }
        else {

            Destroy(this);
        }

        gird = new Grid(1000, 1000, 1, Vector3.zero, false);
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
