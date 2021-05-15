using UnityEngine;
using UnityEditor;
namespace V2.Interfaces.Task
{
    public interface IMoveTask : ITask    {
        Vector3 Destiny { get; set; }
    }
}