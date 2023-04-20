using UnityEngine;
using System.Collections;
using System;

public class TestSceneOne : MonoBehaviour
{

    // Use this for initialization
    void Start() {
        startPlayerOneWorkerGatheringTask();
        startPlayerTwoWrokerGatheringTask();
    }

    private void startPlayerOneWorkerGatheringTask() {

        startPlayerWorkerGatheringTask(
           playerName: "player_one",
           workerName: "worker",
           buildingName: "building_1_001",
           resourceName: "resource");
    }

    private void startPlayerTwoWrokerGatheringTask() {
   
        startPlayerWorkerGatheringTask(
            playerName: "player_two",
            workerName: "worker_2_001", 
            buildingName: "building_2_001",
            resourceName: "resource_002");
    }

    private void startPlayerWorkerGatheringTask(string playerName, string workerName , string buildingName, string resourceName) {
        GameObject player = GameObject.Find(playerName);
        var worker = GameObject.Find(workerName);
        var building_to_deposit = GameObject.Find(buildingName);
        var resource = GameObject.Find(resourceName);

        startGatheringTask(player, worker, building_to_deposit, resource);
    }

    private void startGatheringTask(GameObject player, GameObject worker, GameObject building_to_deposit, GameObject resource) {
        var playerBehaviour = player.GetComponent<PlayerBehaviour>();
        var taskGatheringManager = worker.GetComponent<TaskGatheringManager>();
        taskGatheringManager.PlaceToDeposit = building_to_deposit.transform.position;
        taskGatheringManager.AddResourceAction = playerBehaviour.AddFoodAmount;
        taskGatheringManager.Init(resource.GetComponent<ResourceBehaviour>());
    }

    // Update is called once per frame
    void Update() {

    }
}
