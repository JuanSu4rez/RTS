using UnityEngine;
using System.Collections;
using V2.Tasks.Unit;

public class TestControllerBuilding : MonoBehaviour
{
    public GameObject targetBuilding;
    private V2.Controllers.UnitController tatgetUnitController;
    private V2.Controllers.UnitController myUnitController;
    // Use this for initialization
    void Start() {
        tatgetUnitController = targetBuilding.GetComponent<V2.Controllers.UnitController>();
        if(!tatgetUnitController) {
            Destroy(this);
            return;
        }
        myUnitController = this.GetComponent<V2.Controllers.UnitController>();
        if(!myUnitController) {
            Destroy(this);
            return;
        }
    }
    float coolDown = 0;
    // Update is called once per frame
    void Update() {
        if( Time.time < coolDown) {
            return;
        }
        var indexPosition = tatgetUnitController.AssingPosition();
        if(indexPosition > -1) {
            //todo one call
            var points = tatgetUnitController.GetSurroundingPoints();
            var destiny = points[indexPosition];
            QueueTask queueTask = new QueueTask(this.gameObject);
            queueTask.ListOfTask.Add(new MoveTaskWithAI(this.gameObject, destiny));
            queueTask.ListOfTask.Add(new V2.Tasks.Unit.Citizen.BuildingTask(this.gameObject, targetBuilding, indexPosition));
            myUnitController.AssingTask(queueTask);

        }
        coolDown = Time.time + 20f;
    }
}
