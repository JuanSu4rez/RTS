using UnityEngine;


public class GameAreasManager : ScriptableObject, IGameAreas
{
    public Rect GameArea = new Rect();

    public Rect GuiArea = new Rect();

    public static float minheightgamearea = 0;
    public static  float buttonlengtharea = 0;
  


    public void Init()
    {
        minheightgamearea = Screen.height * 0.8F;
        buttonlengtharea = Screen.height - (minheightgamearea);

        GameArea.x = 0;
        GameArea.y = 0;
        GameArea.width = Screen.width;
        GameArea.height = minheightgamearea;

        GuiArea.x = 0;
        GuiArea.y = minheightgamearea;
        GuiArea.width = Screen.width;
        GuiArea.height = Screen.height- (Screen.height * 0.8F) ;//(Screen.height - Screen.width / 4.0f);
    }

    public void RecalculateAreas(CameraStates state)
    {
        
        switch (state)
        {
            case CameraStates.None:
                GameArea.height = minheightgamearea;// Screen.height;

                break;
            default:
                GameArea.height = minheightgamearea;
                break;
        }
    }
}