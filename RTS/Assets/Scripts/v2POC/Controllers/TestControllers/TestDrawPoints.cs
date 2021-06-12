using UnityEngine;
using System.Collections;
namespace V2.Controllers.TestControllers
{
    public class TestDrawPoints : MonoBehaviour{
        private UnitController unitController;
        private Vector3[] points = null;
        // Use this for initialization
        void Start() {
            unitController = this.GetComponent<UnitController>();
            points = unitController.GetSurroundingPoints();
        }
        private void Update() {
            points = unitController.GetSurroundingPoints();
            if(points == null)
                return;
            foreach(var point in points) {
                Debug.DrawLine(point, point + Vector3.up * 0.2f, Color.red);
            }
        }
    }
}