using UnityEngine;
using UnityEditor;
namespace V2.Interfaces.Task
{
    public interface IComplexTask:Task.ITask, IPreviousTask, INextTask {
        bool CanBeContinued();
    }
}