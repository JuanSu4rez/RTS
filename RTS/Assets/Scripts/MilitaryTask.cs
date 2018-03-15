using UnityEngine;
using UnityEditor;
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
}