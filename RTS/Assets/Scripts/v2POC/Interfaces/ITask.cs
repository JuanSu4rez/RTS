using UnityEngine;
using UnityEditor;
namespace V2.Interfaces
{
    public interface ITask
    {
        GameObject GameObject { get; set; }
        bool IsComplete();
        void Update();
    }

    public interface IMoveTask : ITask
    {
        Vector3 Destiny { get; set; }
    }

    public interface ICompoundTask : ITask
    {
        IMoveTask MoveTask { get; set; }
    }
}