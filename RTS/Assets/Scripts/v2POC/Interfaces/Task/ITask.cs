using UnityEngine;
using UnityEditor;
namespace V2.Interfaces.Task
{
    public interface ITask
    {
        GameObject GameObject { get; set; }
        bool IsComplete();
        void Update();
    }
}