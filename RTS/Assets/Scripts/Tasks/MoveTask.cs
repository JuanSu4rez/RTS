using UnityEngine;
using System.Collections;
using System;

public class MoveTask : Task
{
   // public CitizeAnimationStates animationstate = CitizeAnimationStates.None;

    public readonly  Vector3 position;

    public Action action;


    public MoveTask(Vector3 _position) {
        this.position = _position;
    }
    // public Action releaseaction;
}
