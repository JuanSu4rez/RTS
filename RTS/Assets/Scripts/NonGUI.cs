using UnityEngine;


public class NonGUI : ScriptableObject, IGui
{
    public bool HasOptionSelected()
    {
        return false;
    }

    public void ShowGUI(GameObject selectedGameObject)
    {
        return;
    }

    public void UpdateGui(GameObject selectedGameObject)
    {
        return;
    }
}