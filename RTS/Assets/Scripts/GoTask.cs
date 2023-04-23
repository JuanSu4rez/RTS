using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTask : Task{
    [SerializeField]
    private Vector3 _destination;

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    public override void TaskUpdate() {

        Debug.Log("GoTask update");
        this.gameObject.transform.position = _destination;
        
    }

    public override bool IsFinished() {
        return _destination == this.gameObject.transform.position;
    }

    public void SetDestination(Vector3 destiny) {
        _destination = destiny;
    }

}
