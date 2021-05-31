using UnityEngine;
using System.Collections;
using V2.Interfaces;
using V2.Tasks.Unit;
using V2.Tasks.Unit.Citizen;

namespace V2.Controllers.TestControllers
{
    public class TestCitizenController : MonoBehaviour{
        private UnitController unitController;
    // Use this for initialization
        void Start() {
            unitController = this.GetComponent<UnitController>();
        }
        // Update is called once per frame
        void Update() {
            var destinyPosition = this.transform.position + ( 15 * ( Vector3.left + -Vector3.forward ) );
            unitController.AssingTask(new MoveTaskWithAI(this.gameObject, destinyPosition));
            unitController.IsSelected = true;
            this.enabled = false;
        }
    }
}