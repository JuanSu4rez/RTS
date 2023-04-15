using UnityEngine;
using System.Collections;

public class TestSceneOne : MonoBehaviour
{

    // Use this for initialization
    void Start() {
        var player = GameObject.Find("player");
        var tg = player.GetComponent<TaskGatheringManager>();
        var resource = GameObject.Find("resource");
        tg.Init(resource.GetComponent<ResourceBehaviour>());
    }

    // Update is called once per frame
    void Update() {

    }
}
