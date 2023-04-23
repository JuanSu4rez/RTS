using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TaskExecutor : MonoBehaviour{

    private Queue<Task> _taskQueue = new Queue<Task>();
    private Task _currentTask = null;
    private Action<Task, Task> _taskTransitionAction;
    public bool HasEnded => _taskQueue.Count == 0;

    // Use this for initialization
    void Start() {
        _taskTransitionAction = (done, current) => { };
    }

    // Update is called once per frame
    void LateUpdate() {
        if(_taskQueue.Count > 0) {
            if(_currentTask == null) {
                _currentTask = _taskQueue.Peek();
                _currentTask.enabled = true;
                return;
            }

            if(_currentTask.IsFinished()) {
                var doneTask = _taskQueue.Dequeue();
                doneTask.enabled = false;

                _currentTask = _taskQueue.Count > 0 ? _taskQueue.Peek() : null;
                if(_currentTask != null) {
                    _currentTask.enabled = true;
                }

                _taskTransitionAction(doneTask, _currentTask);
            }
        }
    }

    public void Enqueue(Task task) {
        _taskQueue.Enqueue(task);
    }

    public void SetTaskTransition(Action<Task, Task> taskTransitionAction) {
        _taskTransitionAction = taskTransitionAction;
    }
}
