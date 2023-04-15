using UnityEngine;
using System.Collections;

public abstract class RtsTask: MonoBehaviour
{
    void Start() {
        MyStart();
    }

    void Update() {
        MyUpdate();
    }

    public virtual void MyUpdate() {

    }

    public virtual void MyStart() {

    }

    public abstract bool IsFinished();
}
