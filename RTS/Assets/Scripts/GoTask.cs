using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTask : Task{
    [SerializeField]
    private Vector3 Destiny;

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    public override void TaskUpdate() {

        Debug.Log("GoTask update");
        this.gameObject.transform.position = Destiny;
        
    }

    public override bool IsFinished() {
        return Destiny == this.gameObject.transform.position;
    }

    public void SetDestiny(Vector3 destiny) {
        Destiny = destiny;
    }

}
