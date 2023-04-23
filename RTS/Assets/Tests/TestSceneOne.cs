using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class TestSceneOne : MonoBehaviour{
   
    // Use this for initialization
    void Start() {
        startPlayerOneWorkerGatheringTask();
        startPlayerTwoWrokerGatheringTask();
    }

    private void startPlayerOneWorkerGatheringTask() {

        startPlayerWorkerGatheringTask(
           playerGameObjectName: "Player_one",
           workerGameObjectName: "Worker_1_001",
           buildingGameObjectName: "Building_1_001",
           resourceGameObjectName: "Resource_001",
           playerScoreFoodGameObjectName: "PlayerOneScore");
    }

    private void startPlayerTwoWrokerGatheringTask() {
   
        startPlayerWorkerGatheringTask(
            playerGameObjectName: "Player_two",
            workerGameObjectName: "Worker_2_001", 
            buildingGameObjectName: "Building_2_001",
            resourceGameObjectName: "Resource_002",
            playerScoreFoodGameObjectName: "PlayerTwoScore");
    }

    private void startPlayerWorkerGatheringTask(string playerGameObjectName, string workerGameObjectName , string buildingGameObjectName, string resourceGameObjectName, string playerScoreFoodGameObjectName) {

        var playerGameObject = GameObject.Find(playerGameObjectName);
        var selectedWorkerGameObject = GameObject.Find(workerGameObjectName);
        var selectedBuildingToDepositGameObject = GameObject.Find(buildingGameObjectName);
        var selectedResourceGameObject = GameObject.Find(resourceGameObjectName);
        var playerScoreFoodGameObject = GameObject.Find(playerScoreFoodGameObjectName);

        startGatheringTask(playerGameObject, selectedWorkerGameObject, selectedBuildingToDepositGameObject, selectedResourceGameObject, playerScoreFoodGameObject);
    }

    private void startGatheringTask(GameObject player, GameObject worker, GameObject building_to_deposit, GameObject resource, GameObject playerScoreFoodGameObject) {

        var playerBehaviour = player.GetComponent<PlayerBehaviour>();
        var viewScore = playerScoreFoodGameObject.GetComponent<ViewScore>();
        var playerFacade = player.GetComponent<PlayerFacade>();
        playerFacade.Init(playerBehaviour, viewScore);

        var taskGatheringManager = worker.GetComponent<TaskGatheringManager>();
        taskGatheringManager.PlaceToDeposit = building_to_deposit.transform.position;
        taskGatheringManager.AddResourceAction = playerFacade.AddResource;
        taskGatheringManager.Init(resource.GetComponent<ResourceBehaviour>());


    }

    // Update is called once per frame
    void Update() {

    }
}
