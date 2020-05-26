using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    // Start is called before the first frame update

   public Color color;

    private Vector3[] points = null;

  
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDrawGizmos() {

        points = Utils.GetPoints(this.gameObject);

        Gizmos.color = Color.red;
        foreach (var point in points) {
                Gizmos.DrawSphere(point, 0.5f);
            }



        


    }

}
