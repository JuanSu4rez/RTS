using UnityEngine;
using System.Collections;
using System;

public class CompositeTask : MoveTask
{
    public Task task;

    public CompositeTask(Vector3 _position):base(_position) {
       
    }



}
