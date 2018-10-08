using UnityEngine;
using UnityEditor;

public class MilitaryGui : ScriptableObject, IGui
{
    public void ShowGUI(GameObject selectedGameObject)
    {
        GUI.Button(CameraScript.gameareas.GuiArea, "Military options");
    }

    public void UpdateGui(GameObject selectedGameObject)
    {
    
    }

    public bool HasOptionSelected()
    {
        return false;
    }
}