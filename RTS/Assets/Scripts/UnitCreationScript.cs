using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitCreationScript : MonoBehaviour {

    public float durationCreationUnitTime { get; set; }
    public float CreationUnitTime { get; set; }
    public Queue<Units> creationQueue;

    int deltaPosition = -5;

    // Use this for initialization
    void Start () {
        durationCreationUnitTime = 5;
        CreationUnitTime = 0;
        creationQueue = new Queue<Units>();
    }
	
	// Update is called once per frame
	void Update () {
        if (creationQueue.Count > 0){
            if (Time.time >= CreationUnitTime + durationCreationUnitTime){              
                createUnit(creationQueue.Dequeue());
                CreationUnitTime = Time.time;
            }
        }
	}
    
    //solo se llama si los costes de la unidad estan disponibles
    public void addUnitToQueue(Units unitType) {       
        creationQueue.Enqueue(unitType);
        if (creationQueue.Count == 1){
            CreationUnitTime = Time.time;
        }
    }

    private void createUnit(Units unitType) {
        GameObject unitToCreate = UnityEngine.Resources.Load(unitType.ToString(), typeof(GameObject)) as GameObject;
        GameObject newUnit = Instantiate(unitToCreate, calculateUnitOrigin(), Quaternion.identity);
        newUnit.name = unitType.ToString();
    }

    private Vector3 calculateUnitOrigin() {
        Collider buildingCollider = this.gameObject.GetComponent<Collider>();
        Transform buildingPosition = this.gameObject.GetComponent<Transform>();

        Vector3 unitPosition = new Vector3();
        unitPosition.x = buildingPosition.position.x - buildingCollider.bounds.size.x/2 + deltaPosition;
        unitPosition.z = buildingPosition.position.z - buildingCollider.bounds.size.z/2 + deltaPosition;
        unitPosition.y = buildingPosition.position.y;

        return unitPosition;
    }
}
