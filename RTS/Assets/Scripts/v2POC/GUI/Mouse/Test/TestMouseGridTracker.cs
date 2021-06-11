using UnityEngine;
using System.Collections;
using System.Linq;
public class TestMouseGridTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Vector3 initialTargetPosition;
    // Use this for initialization
    void Start() {
        if(!target) {
            this.enabled = false;
            return;
        }
        initialTargetPosition = target.transform.position;
    }

    // Update is called once per frame
    void Update() {
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        var results = Physics.RaycastAll(ray, Mathf.Infinity);
        RaycastHit? raycastHitOnLand = results.FirstOrDefault(p => p.collider.gameObject.tag == "Land");
        if(!raycastHitOnLand.HasValue) {
            target.transform.position = initialTargetPosition ;
            return;
        }
        target.transform.position = V2.Classes.Grid.grid.getCenteredGridPositionFromWorldPosition(raycastHitOnLand.Value.point);
        var sorrundpositions =   V2.Classes.Grid.grid.GetSorroundPositions(target.transform.position);
        for(int i = 0; i < sorrundpositions.Length; i++) {
            Debug.DrawLine(sorrundpositions[i], sorrundpositions[i] + Vector3.up * 0.2f, Color.cyan);
        }
    }
}
