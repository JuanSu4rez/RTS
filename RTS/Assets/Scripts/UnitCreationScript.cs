using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitCreationScript : MonoBehaviour {

    public float durationCreationUnitTime { get; set; }
    public float CreationUnitTime { get; set; }
    public Queue<Units> creationQueue;
    private ITeamable myteam;

    private IGameFacade facade;
    [SerializeField]
    private int deltaPosition ;

    public int DeltaPosition
    {
        get
        {

            return deltaPosition;
        }
    }

    // Use this for initialization
    void Start () {
        durationCreationUnitTime = 5;
        CreationUnitTime = 0;
        creationQueue = new Queue<Units>();

        myteam = this.GetComponent<ITeamable>();

        facade =GameScript.GetFacade(myteam);

    }
	
	// Update is called once per frame
	void Update () {
        if (creationQueue == null)
            return;

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
        //TODO Implement flyweight  pattern , Implement Wrapper class to define the path of the resource
        //TODO you must know the team 
        GameObject unitToCreate = null;
        if (facade == null)
        {
             unitToCreate = UnityEngine.Resources.Load(unitType.ToString(), typeof(GameObject)) as GameObject;
        }
        else
        {
             unitToCreate  =  facade.GameResource.Load<GameObject>(unitType.ToString()) ;
        }

        var team = unitToCreate.GetComponent<ITeamable>();
        if (team != null)
        {
            team.Team = myteam.Team;
        }

        GameObject newUnit = Instantiate(unitToCreate);
        newUnit.transform.position = calculateUnitOrigin();
        newUnit.name = unitType.ToString();
    }

    private Vector3 calculateUnitOrigin() {
        Collider buildingCollider = this.gameObject.GetComponent<Collider>();
        Transform buildingPosition = this.gameObject.transform;
  
        Vector3 unitPosition = new Vector3();
        var v = buildingCollider.bounds.size;// buildingCollider.bounds.size.sqrMagnitude > this.transform.localScale.sqrMagnitude ? buildingCollider.bounds.size: this.transform.localScale;
        v.x /= 2;
        v.z /= 2;
        v.y = 0;
        unitPosition = (this.transform.position - v) - new Vector3(deltaPosition, 0, deltaPosition);

        return unitPosition;
    }


    void OnDrawGizmos()
    {

        Gizmos.color = myteam!= null && myteam.Team != null? myteam.Team.Color: Color.gray;
        Gizmos.DrawCube(calculateUnitOrigin(), new Vector3(1, 1, 1));

        Gizmos.DrawLine(this.transform.position, calculateUnitOrigin());


        //Gizmos.color = Color.red;
        //var v =this.transform.localScale;
        //v.x /= 2;
        //v.z /= 2;
        //v.y= 0;
        //var p1 =  (this.transform.position - v) - new Vector3(1, 0, 1);
        //Gizmos.DrawLine(this.transform.position, p1);
        //
        //Gizmos.DrawCube(p1, new Vector3(1, 1, 1));
    }


}
