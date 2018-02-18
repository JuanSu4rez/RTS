using UnityEngine;
using UnityEditor;
[System.Serializable]
public class CitizenTask// : ScriptableObject
{
    private Vector3 position;
    private GameObject gameobject ;
    private CitizenStates citizenLabor;


    public Vector3 Position{ get { return position; } }
    public GameObject Gameobject { get { return gameobject; } }
    public CitizenStates VitizenLabor { get { return citizenLabor; } }


    public CitizenTask(  Vector3 _position ,GameObject _gameobject, CitizenStates _citizenLabor)
    {
        position= _position;
        gameobject = _gameobject;
       citizenLabor = _citizenLabor;
    }
}