using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour{

    [SerializeField]
    private float _foodAmount;
    public float FoodAmount { get => _foodAmount; }
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void AddResource(ResourceType resourceType, float amountToAdd) {
        switch(resourceType) {
            case ResourceType.Food:
                AddFoodAmount(amountToAdd);
                break;
        }
    }

    public void AddFoodAmount(float amountToAdd) {
        _foodAmount += amountToAdd;
    }
}
