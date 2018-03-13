using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour
{
    [SerializeField]
    private AssetTypes assettype;
 
    [SerializeField]
    private BuildingsInfo buldingsInfo;

    [SerializeField]
    private Player player;

    [SerializeField]
    private UnitsInfo unitsInfo;
    
    [SerializeField]
    private Team team;

    private static IGameFacade facade;
    // Use this for initialization

    void Awake()
    {
        var gameFacade = ScriptableObject.CreateInstance<GameFacade>();
        gameFacade.Player = player;
        gameFacade.BuildingsInfo = buldingsInfo;
        gameFacade.UnitsInfo = unitsInfo;
        gameFacade.Assettype = assettype;
        gameFacade.Team = team;

        facade = gameFacade;
    }

    void Start()
    {    
        
        
    }

    public static IGameFacade GetFacade()
    {
        return facade;
    }

    // Update is called once per frame
    void Update()
    {

    }

     void OnGUI(){
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(10, 10, 100, 50), "Oro " + (int)player.GetResourceAmount(Resources.Gold).Amount);
        GUI.Label(new Rect(110, 10, 100, 50), "Alimento " + (int)player.GetResourceAmount(Resources.Food).Amount);
        GUI.Label(new Rect(210, 10, 100, 50), "Madera " + (int)player.GetResourceAmount(Resources.Wood).Amount);
        GUI.Label(new Rect(310, 10, 100, 50), "Piedra " + (int)player.GetResourceAmount(Resources.Rock).Amount);
    }

  
}
