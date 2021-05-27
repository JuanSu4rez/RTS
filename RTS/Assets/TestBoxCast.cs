using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestBoxCast : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject targetObject;
    void Start()
    {
        if (targetObject != null) {
            //targetObject.GetComponent<Material>().SetColor("_Color", new Color(255,255,255,1));
        }

     }

    // Update is called once per frame
    void Update()
    {


        if (targetObject == null)
            return;
        //Debug.Log("Path cleared2: " + CheckPath(this.gameObject.transform.position, targetObject.transform.position));

        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetObject.transform.forward * 10, Color.blue );
        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetObject.transform.right * 10, Color.red );
        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetObject.transform.up * 10, Color.green );


        if (CameraScript.selection != null) {

            //this.targetObject.transform.position = CameraScript.firstclick;

      


            Vector3 initialPoint = Camera.main.ScreenToWorldPoint(CameraScript.firstclick);

            Vector3 second = Camera.main.ScreenToWorldPoint(CameraScript.secondclick);

            Vector3 verticalmouseproyection = Camera.main.ScreenToWorldPoint(CameraScript.firstclick + (Vector2.down * CameraScript.selection.height));

            Vector3 horizaontalmouseproyection = Camera.main.ScreenToWorldPoint(CameraScript.firstclick + (Vector2.right * CameraScript.selection.width));
          
           // this.targetObject.transform.position = initialPoint;

            Vector3 initialPointLand = Vector3.zero;

            Vector3 secondPointLand = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(CameraScript.firstclick);

            var results =    Physics.RaycastAll(ray, Mathf.Infinity);

            RaycastHit? result = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");

            if (result.HasValue ) 
            {
                initialPointLand = result.Value.point;
                Debug.DrawLine(initialPoint, initialPointLand, Color.magenta);

                var proyeccionvertical = CameraScript.firstclick + (Vector2.down * CameraScript.selection.height);

                ray = Camera.main.ScreenPointToRay(proyeccionvertical);

                var results0 = Physics.RaycastAll(ray, Mathf.Infinity);



                RaycastHit? result0 = results0.FirstOrDefault(p => p.collider.gameObject.tag == "Land");

                var proyeccionhorizontal = CameraScript.firstclick + (Vector2.right * CameraScript.selection.width);

                ray = Camera.main.ScreenPointToRay(proyeccionhorizontal);

                var results1 = Physics.RaycastAll(ray, Mathf.Infinity);

                RaycastHit? result1 = results1.FirstOrDefault(p => p.collider.gameObject.tag == "Land");



                if (result0.HasValue &&  result1.HasValue) {
                    secondPointLand = result.Value.point;
                 
                    Debug.DrawLine(initialPointLand, result0.Value.point, Color.cyan);

                    Debug.DrawLine(initialPointLand, result1.Value.point, Color.gray);

                    var auxpostion =( initialPointLand - ((initialPointLand -result0.Value.point  ) * 0.5f  ) - ((initialPointLand - result1.Value.point) * 0.5f)) ;

                    Debug.DrawLine(initialPointLand, auxpostion, Color.white);

                    this.targetObject.transform.position = auxpostion;
               
                    this.targetObject.transform.localScale = new Vector3((Mathf.Abs((initialPointLand- result0.Value.point).magnitude) ), 1,
                  (Mathf.Abs((initialPointLand - result1.Value.point).magnitude))
                        );


                }


            }



        }
    }


    private bool CheckPath(Vector3 position, Vector3 target) {


        Vector3 halfExtents = Vector3.one;

        Quaternion rotation = QuaternionFromMatrix(targetObject.transform.localToWorldMatrix);//Quaternion.LookRotation(target - position);
        Vector3 direction = target - position;
        float distance = Vector3.Distance(position, target);

        RaycastHit[] rhit = Physics.BoxCastAll(position, halfExtents, direction, targetObject.transform.rotation, distance);
        //Debug.Log("hits "+rhit.Length);

        //Debug.Log("hits " + string.Join(";", rhit.Where(r=> r.collider.tag != "Land").Select(p=> p.collider.gameObject.name)));
        bool result = rhit.Count(r => r.collider.tag != "Land")>0;

        Vector3 center = Vector3.Lerp(position, target, 0.5f);
        halfExtents = new Vector3(1, 1, (target - position).magnitude) / 2;
        // Debug.(center, halfExtents, rotation, result ? Color.green : Color.red);
   
      //  Debug.DrawRay(position, direction, result ? Color.green : Color.red);
       // DrawBox(center, halfExtents, rotation, result ? Color.green : Color.red);

        return result;
    }


    private void OnDrawGizmos() {
        if (targetObject == null)
            return;
        Vector3 halfExtents = Vector3.one;
        Gizmos.color = Color.red;

        //var M = Gizmos.matrix;
        Gizmos.matrix = targetObject.transform.localToWorldMatrix;

        Gizmos.DrawCube(targetObject.transform.position, halfExtents * 1.2f) ;

      //  Gizmos.matrix = M;
    }


    public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
        // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
        return q;
    }

}
