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
        public virtual void OnDrawGizmos() {
            if(!Application.isPlaying)
                return;
            points = unitController.GetSurroundingPoints();
            Gizmos.color = new Color(1, 0, 0, 0.33f);
            foreach(var point in points) {
                Gizmos.DrawSphere(point, 0.5f);
            }
        }
    }
}