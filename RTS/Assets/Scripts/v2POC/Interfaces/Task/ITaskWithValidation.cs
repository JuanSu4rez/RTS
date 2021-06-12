using UnityEngine;
using UnityEditor;
namespace V2.Interfaces.Task
{
    public interface ITaskWithValidation:Task.ITask {
        bool CanBeContinued();
    }
}