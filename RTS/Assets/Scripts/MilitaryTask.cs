using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class MilitaryTask// : ScriptableObject
{
  
    private GameObject gameobject ;
 

  
    public GameObject Gameobject { get { return gameobject; } }
    private MilitaryTaskType militarTaskType;
    private Vector3[] positions = new Vector3[] { };

    public MilitaryTask(  GameObject _gameobject, MilitaryTaskType _militarTaskType)
    {
     
        gameobject = _gameobject;
        militarTaskType = _militarTaskType;
    }

    public MilitaryTask(Vector3[] _positions)
    {
        positions = _positions;
        militarTaskType = MilitaryTaskType.Patroll;

    }

    public bool IscompletedTask() {
        return gameobject == null;
    }

    public bool GetTargetDistance(GameObject gameobject, out Vector3 targetDistance)
    {
        targetDistance = Vector3.zero;
        if (gameobject != null && gameobject.transform != null)
            return GetTargetDistance(gameobject.transform, out targetDistance);
        else
            return false;
    }

    public bool GetTargetDistance(Transform transform, out Vector3 targetDistance)
    {
       targetDistance = Vector3.zero;
       var result = false;
       if(gameobject!= null)
        {
            result = true;
            targetDistance = transform.position - gameobject.transform.position;
        }
        return result;
    }
}