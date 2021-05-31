using UnityEngine;
using UnityEditor;
namespace V2.Interfaces
{
    public interface IHealthPoint{
        float CurrentHealth { get; set; }
        bool IsAlive();
    }
}