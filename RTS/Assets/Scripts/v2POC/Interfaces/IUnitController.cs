using UnityEngine;
using UnityEditor;
namespace V2.Interfaces
{
    public interface IUnitController
    {
        void AssingTask(ITask task);
        ITask GetTask();
    }
}