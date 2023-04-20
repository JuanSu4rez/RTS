using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResourceBehaviour : MonoBehaviour
{
    [SerializeField]
    private int _amount;

    public bool IsEmpty => _amount == 0;

    void Update() {
        if(_amount <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }
  
    public int DiscountAmount(int discount) {
        if(discount > _amount) {
            var discounted = _amount;
            _amount = 0;
            return discounted;
        }
        else {
            _amount -= discount;
            return discount;
        }
    }
}
