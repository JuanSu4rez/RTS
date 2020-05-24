using UnityEngine;
using UnityEditor;
using System;

public interface IMovable {

    void Move(Vector3 position, Action action = null);//, Action releaseaction= null);

}