using UnityEngine;


public class GameAreasManager : ScriptableObject, IGameAreas
{
    public Rect GameArea = new Rect();

    public Rect GuiArea = new Rect();

    public void Init()
    {
        GameArea.x = 0;
        GameArea.y = 0;
        GameArea.width = Screen.width;
        GameArea.height = Screen.height;

        GuiArea.x = 0;
        GuiArea.y = Screen.height - (Screen.height - Screen.width / 3.0f);
        GuiArea.width = Screen.width;
        GuiArea.height =  (Screen.height - Screen.width / 3.0f);
    }

    public void RecalculateAreas(CameraStates state)
    {
        switch (state)
        {
            case CameraStates.None:
                GameArea.height = Screen.height;

                break;
            default:
                GameArea.height = Screen.height - (Screen.height - Screen.width / 3.0f);
                break;
        }
    }
}