using UnityEngine;
using UnityEditor;
using V2.Interfaces.Task;

namespace V2.Interfaces
{
    public interface IUnitController
    {
        void AssingTask(ITask task);
        ITask GetTask();
    }
}