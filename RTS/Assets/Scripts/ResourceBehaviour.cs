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
        int discountedAmount = ( _amount - discount );
        _amount = ( discountedAmount < 0 ) ? 0 : discountedAmount;
        return discount;
    }
}
