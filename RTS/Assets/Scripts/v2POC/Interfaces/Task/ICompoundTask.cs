using UnityEngine;
using UnityEditor;
namespace V2.Interfaces.Task
{
    public interface ICompoundTask : ITask{
        IMoveTask MoveTask { get; set; }
    }
}