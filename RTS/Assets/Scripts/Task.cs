using UnityEngine;
using System.Collections;

public abstract class Task: MonoBehaviour
{
    void Start() {
        TaskStart();
    }

    void Update() {
        TaskUpdate();
    }

    public virtual void TaskUpdate() {
    }

    public virtual void TaskStart() {
    }
 
    public abstract bool IsFinished();
}
