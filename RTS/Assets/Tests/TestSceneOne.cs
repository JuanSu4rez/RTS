using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class TestSceneOne : MonoBehaviour
{
   
    // Use this for initialization
    void Start() {
        startPlayerOneWorkerGatheringTask();
        startPlayerTwoWrokerGatheringTask();
       
      
    }

    private void startPlayerOneWorkerGatheringTask() {

        startPlayerWorkerGatheringTask(
           playerGameObjectName: "player_one",
           workerGameObjectName: "worker_1_001",
           buildingGameObjectName: "building_1_001",
           resourceGameObjectName: "resource_001",
           playerScoreFoodGameObjectName: "playerOneScore");
    }

    private void startPlayerTwoWrokerGatheringTask() {
   
        startPlayerWorkerGatheringTask(
            playerGameObjectName: "player_two",
            workerGameObjectName: "worker_2_001", 
            buildingGameObjectName: "building_2_001",
            resourceGameObjectName: "resource_002",
            playerScoreFoodGameObjectName: "playerTwoScore");
    }

    private void startPlayerWorkerGatheringTask(string playerGameObjectName, string workerGameObjectName , string buildingGameObjectName, string resourceGameObjectName, string playerScoreFoodGameObjectName) {
        var playerGameObject = GameObject.Find(playerGameObjectName);
        var workerGameObject = GameObject.Find(workerGameObjectName);
        var buildingToDepositGameObject = GameObject.Find(buildingGameObjectName);
        var resourceGameObject = GameObject.Find(resourceGameObjectName);
        var playerScoreFoodGameObject = GameObject.Find(playerScoreFoodGameObjectName);
        startGatheringTask(playerGameObject, workerGameObject, buildingToDepositGameObject, resourceGameObject, playerScoreFoodGameObject);
    }

    private void startGatheringTask(GameObject player, GameObject worker, GameObject building_to_deposit, GameObject resource, GameObject playerScoreFoodGameObject) {
        var playerBehaviour = player.GetComponent<PlayerBehaviour>();
        var taskGatheringManager = worker.GetComponent<TaskGatheringManager>();
        taskGatheringManager.PlaceToDeposit = building_to_deposit.transform.position;
        taskGatheringManager.AddResourceAction = playerBehaviour.AddResource;
        taskGatheringManager.Init(resource.GetComponent<ResourceBehaviour>());
        var viewComponent =  playerScoreFoodGameObject.GetComponentInChildren<PlayerScoreResourceView>();
        viewComponent.Init(()=> playerBehaviour.FoodAmount.ToString());
    }

    // Update is called once per frame
    void Update() {

    }
}
