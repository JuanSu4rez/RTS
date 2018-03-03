using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour
{
    public AssetTypes assettype;

    [SerializeField]
    private Player player;
    [SerializeField]
    public BuildingsInfo buldingsInfo;

    public UnitsInfo unitsInfo;

    private static IGameFacade facade;
    // Use this for initialization
    void Start()
    {    
        assettype = AssetTypes.NONE;      


        var gameFacade = ScriptableObject.CreateInstance<GameFacade>() ;
        gameFacade.Player = player;
        gameFacade.BuildingsInfo = buldingsInfo;
        gameFacade.UnitsInfo = unitsInfo;

        facade = gameFacade;
        
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
