using UnityEngine;
using System.Collections;
using System;

public class UnitsGui : ScriptableObject,IGui
{


  

    private GameObject createdObject;

    public bool HasOptionSelected()
    {
        return createdObject != null;
    }

    // Update is called once per frame
    public void UpdateGui(GameObject selectedGameObject)
    {
        //Debug.Log(" UnitsGui Update " + HasOptionSelected());
        if (HasOptionSelected())
        {

            var navagentcitizen = selectedGameObject.GetComponent<NavAgentCitizenScript>();
            if (navagentcitizen == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            //TODO USE RayCasting with layer to avoid colliding with the createdObject 
            var arrayCollisions = Physics.RaycastAll(ray);
            if (arrayCollisions.Length > 0)
            {
                if (SeekForaGameObjectByName(ref arrayCollisions, "Land", out hit))
                {
                    createdObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                }

            }

            if (Input.GetMouseButtonDown(0))
            {

                if (
                    ValidateCollitionsToPutCreatedObject(ref arrayCollisions)

                    )
                {

                    arrayCollisions = Physics.BoxCastAll(createdObject.transform.position, createdObject.transform.localScale / 2.0f, Vector3.up);
                    // var arrayCollisionsByCollider =   Physics.SphereCastAll(ray, createdObject.gameObject.GetComponent<SphereCollider>().radius);
                    // if(arrayCollisionsByCollider.Length == 1 && arrayCollisionsByCollider[0].collider.gameObject.name.Equals("Land"))
                    if (ValidateCollitionsToPutCreatedObject(ref arrayCollisions))
                    {
                        createdObject.GetComponent<BuildingBehaviour>().Team = navagentcitizen.Team;
                        createdObject = null;
                    }
                       
                }
            }
        }

    }

    private bool ValidateCollitionsToPutCreatedObject(ref RaycastHit[] arrayCollisions)
    {
        return arrayCollisions.Length == 1 && (arrayCollisions[0].transform.gameObject.name.Equals("Land") || arrayCollisions[0].transform.gameObject.Equals(createdObject))
            ||
            arrayCollisions.Length == 2
            &&
            (arrayCollisions[0].transform.gameObject.name.Equals("Land") && arrayCollisions[1].transform.gameObject.Equals(createdObject)
            ||
            arrayCollisions[1].transform.gameObject.name.Equals("Land") && arrayCollisions[0].transform.gameObject.Equals(createdObject)
            );
    }

    private bool SeekForaGameObjectByName(ref RaycastHit[] hits, string name, out RaycastHit result)
    {
        bool found = false;
        result = new RaycastHit();
        if (hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length && !found; i++)
            {
                found = hits[i].transform.gameObject.name.Equals(name);
                if (found)
                {
                    result = hits[i];
                }
            }
        }
        return found;
    }


    public void ShowGUI(GameObject  selectedGameObject)
    {

        if (HasOptionSelected())
            return;

        if (selectedGameObject == null)
            return;

        var navagentcitizen = selectedGameObject.GetComponent<NavAgentCitizenScript>();
        if (navagentcitizen == null)
            return;

        var facade = GameScript.GetFacade(navagentcitizen.Team);

        var selected = false;

        selected = GUI.Button(new Rect(0, Screen.height - 100, Screen.width, Screen.height - (Screen.height - 100)), "Crear Casa");

        if (selected)
        {
            //TODO ON MOUSE MOVING SHOW THE BUILDING PREFAB
            //
            if (facade.CanCreateBuilding(Buildings.House))
            {
                Vector3 _mouseposition = Input.mousePosition;
                _mouseposition.z = Screen.height - _mouseposition.z;
                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(_mouseposition);
                mouseposition.y = 1;


                var objectToCreate = facade.GameResource.Load<GameObject>(Buildings.House.ToString());
                var BuildingBehaviour =  objectToCreate.GetComponent<BuildingBehaviour>();
                if (BuildingBehaviour != null)
                {
                    BuildingBehaviour.State = BuildingStates._Fundational;
                }

                createdObject = GameObject.Instantiate(objectToCreate, mouseposition, Quaternion.identity);
                createdObject.name = Buildings.House.ToString();
               
            }

        }

    }
}
