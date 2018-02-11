using UnityEngine;
using System.Collections;

public class UserGui : MonoBehaviour
{
    private CameraScript cameraScript;

    private GameObject objectToCreate;


    private GameObject createdObject;
    // Use this for initialization
    void Start()
    {
        cameraScript = this.gameObject.GetComponent<CameraScript>();

    }

    // Update is called once per frame
    void Update()
    {

        if (createdObject != null)
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            var arrayCollisions = Physics.RaycastAll(ray);
            if ( arrayCollisions.Length >0 )
            {
                hit = arrayCollisions[0];
                createdObject.transform.position  = new Vector3(hit.point.x, 1, hit.point.z);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (arrayCollisions.Length == 1 && hit.collider.gameObject.name.Equals("Land"))
                {
                   // var arrayCollisionsByCollider =   Physics.SphereCastAll(ray, createdObject.gameObject.GetComponent<SphereCollider>().radius);
                   // if(arrayCollisionsByCollider.Length == 1 && arrayCollisionsByCollider[0].collider.gameObject.name.Equals("Land"))
                    createdObject = null;
                }                
            }
        }
    }

    private void OnGUI()
    {
        if (cameraScript.CurrentSelected != null && cameraScript.HasScript<NavAgentCitizenScript>())
        {
            var selected = false;
            if (createdObject == null)
            {
                selected = GUI.Button(new Rect(0, Screen.height - 100, 100, 100), "Crear Casa");
            }   

            if (selected)
            {
                //TODO ON MOUSE MOVING SHOW THE BUILDING PREFAB
                //

               Vector3 _mouseposition=  Input.mousePosition;
                _mouseposition.z = Screen.height - _mouseposition.z;
                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(_mouseposition);
                mouseposition.y = 1;
                objectToCreate = UnityEngine.Resources.Load("House", typeof(GameObject)) as GameObject;
                createdObject = GameObject.Instantiate(objectToCreate, mouseposition, Quaternion.identity);


            }
        }
    }
}
