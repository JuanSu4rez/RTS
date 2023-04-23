using UnityEngine;


public class PlayerFacade : MonoBehaviour{

    private PlayerBehaviour _playerBehaviour;
    private ViewScore _viewScore;

    public void AddResource(ResourceType resourceType, float amountToAdd) {
        switch(resourceType) {
            case ResourceType.Food:
                _playerBehaviour.AddFoodAmount(amountToAdd);
                _viewScore.ResourceFoodAmount.text = _playerBehaviour.FoodAmount.ToString();
                break;
        }
    }

    public void Init(PlayerBehaviour playerBehaviour, ViewScore view) {
        _playerBehaviour = playerBehaviour;
        _viewScore = view;
    }
}
