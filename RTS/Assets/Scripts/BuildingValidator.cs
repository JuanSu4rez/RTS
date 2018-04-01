using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BuildingValidator : MonoBehaviour,IStatus,ISelectable {

    int numberOfCollisions = 0;

    public int NumberOfCollisions
    {
        get
        {
            return numberOfCollisions;
        }
    }

    private Vector3 lastPosition = Vector3.zero;

    List<GameObject> objects = new List<GameObject>();

    public bool IsSelected
    {
        get
        {
            return true;
        }

        set
        {
          
        }
    }

    public GameObject objectToBuild;
    private Vector3 defPosition = Vector3.zero;

 
    // Use this for initialization
    void Start ()
    {
        defPosition = this.transform.position;

    }

    bool IsValidObject()
    {
        return objectToBuild != null;
    }

    private void FixedUpdate()
    {
        if (IsValidObject())
        {
            this.transform.position = objectToBuild.transform.position;
        }

        lastPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool IsIlde()
    {
        return (this.transform.position - lastPosition).sqrMagnitude < 0.6f;
    }

    void OnTriggerEnter( Collider other )
    {
        if (!IsValidObject())
            return;

        if (other.transform.gameObject.tag != "Land"
            &&
            other.transform.gameObject != objectToBuild
            &&
            !other.isTrigger
            &&
            other.transform.parent == null
            )
        {

            numberOfCollisions++;
            objects.Add(other.gameObject);
        } 
    }

    void OnTriggerExit(Collider other )
    {
        if (!IsValidObject())
            return;


        if (other.transform.gameObject.tag != "Land"
            &&
            other.transform.gameObject != objectToBuild
            &&
            !other.isTrigger
            &&
               other.transform.parent == null
            )
        {


            numberOfCollisions--;
            objects.Remove(other.gameObject);
        }
    }

    private void OnGUI()
    {
            
    }

    public void SetEnabled(GameObject objectToBuild)
    {
        numberOfCollisions = 0;
        objects.Clear();
        this.objectToBuild = objectToBuild;
        //this.transform.parent = this.objectToBuild.transform;
        this.transform.position = objectToBuild.transform.position;
        this.transform.localScale = objectToBuild.transform.localScale;
        this.transform.rotation = objectToBuild.transform.rotation;
        this.enabled = true;
    }

    public void SetDisable()
    {
        objects.Clear();
        numberOfCollisions = 0;
        objectToBuild = null;
        this.transform.position = defPosition;
        this.enabled = false;
        
    }

    public string GetStatus()
    {
        return "NumberOfCollisions " + numberOfCollisions+( IsValidObject()? " obj to Build "+ this.objectToBuild.name +" "+ GetCollidedObjects() : " ");
    }

    private string GetCollidedObjects()
    {
        StringBuilder builder = new StringBuilder();
       for(int i = 0;i< objects.Count; i++)
        {
            builder.AppendLine(objects[i].tag + " "+objects[i].name);
        }


        return builder.ToString();
    }
}
