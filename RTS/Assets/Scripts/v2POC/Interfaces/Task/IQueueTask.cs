using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace V2.Interfaces.Task
{
    public interface IQueueTask : ITask{
        IList<ITask> ListOfTask { get; set; }
        Task.ITask Current();
    }
}