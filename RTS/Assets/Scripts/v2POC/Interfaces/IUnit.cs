using UnityEngine;
using UnityEditor;
using V2.Interfaces.Task;
using V2.Enums;

namespace V2.Interfaces
{
    public interface IUnit: IHealthPoint, IDamagable{
        bool IsSelected { get; set; }
        float MaxHealth { get; set; }
        float GetHealthReason();
    }
}