using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour
{
    public AssetTypes assettype;

    [SerializeField]
    private Player player;
    [SerializeField]
    public BuldingsInfo buldingsInfo;

    private static IGameFacade facade;
    // Use this for initialization
    void Start()
    {
    
        assettype = AssetTypes.NONE;
       


        var gameFacade = ScriptableObject.CreateInstance<GameFacade>() ;
        gameFacade.Player = player;
        gameFacade.BuldingsInfo = buldingsInfo;

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

     void OnGUI()
    {
        //dibu
        GUI.Label(new Rect(0, 100, 100, 50), "Oro " + player.GetResourceAmount(Resources.Gold).Amount);
    }

  
}
