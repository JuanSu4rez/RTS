using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {
    [SerializeField]
    private AssetTypes assettype;

    [SerializeField]
    private BuildingsInfo buldingsInfo;

    [SerializeField]
    private UnitsInfo unitsInfo;




    [SerializeField]
    private Team Playerteam;


    [SerializeField]
    private Team[] teams;

    public Team[] Teams {
        get { return teams; }
        set {
            teams = value;
            this.diplomacies = new Diplomacy[teams.Length];
            for (int i = 0; i < diplomacies.Length; i++) {
                diplomacies[i] = new Diplomacy(teams[i], Postures.Enemy);
            }
        }

    }

    [SerializeField]
    private Diplomacy[] diplomacies;


    public PlayerController[] players;

    private static PlayerController[] _splayers;

    private PlayerController player;

    public Diplomacy[] Diplomacies {
        get { return diplomacies; }
        set { diplomacies = value; }

    }

    //private static IGameFacade facade;
    // Use this for initialization
    private static IGameFacade[] facades;


    void Awake() {

        if (players == null || players.Length == 0) {
            throw new UnityException("Debe asignar los respectivos equipos.");
        }

        _splayers = players;
        player = players[0];

        if (teams == null || teams.Length == 0) {
            throw new UnityException("Debe asignar los respectivos equipos.");
        }


        for (int i = 0; i < unitsInfo.UnitInformation.Length; i++) {
            var unit = unitsInfo.UnitInformation[i];
            if (unit.Costs != null && unit.Costs.Count > 0)
                buldingsInfo.BuldingInformation[(int)unit.DevelopedBuilding].AddUnitToCreate(unitsInfo.UnitInformation[i]);
        }

        var gameFacade = ScriptableObject.CreateInstance<GameFacade>();
        gameFacade.Team = Playerteam;
        gameFacade.Player = ScriptableObject.CreateInstance<Player>();
        gameFacade.Player.SetData(gameFacade.Team.InitalTeamData);
        gameFacade.BuildingsInfo = buldingsInfo;
        gameFacade.UnitsInfo = unitsInfo;
        gameFacade.Assettype = assettype;
        gameFacade.Diplomacies = diplomacies;
  
        gameFacade.FacadeName = "Player";
        facades = new IGameFacade[teams.Length + 1];

        facades[0] = gameFacade;

        for (int i = 0; i < teams.Length; i++) {
            var gameFacadei = ScriptableObject.CreateInstance<GameFacade>();
            gameFacadei.Team = teams[i];
            gameFacadei.Player = ScriptableObject.CreateInstance<Player>();
            gameFacadei.Player.SetData(teams[i].InitalTeamData);
            gameFacadei.BuildingsInfo = buldingsInfo;
            gameFacadei.UnitsInfo = unitsInfo;
            gameFacadei.Assettype = assettype;
            gameFacadei.Diplomacies = diplomacies;
            gameFacadei.FacadeName = "Player [" + (i + 1) + "]";
            facades[i + 1] = gameFacadei;
            if (gameFacadei.Team == null ||
                gameFacadei.Player == null ||
                gameFacadei.BuildingsInfo == null ||
                gameFacadei.UnitsInfo == null ||

                gameFacadei.Diplomacies == null)
                throw new UnityException("Invalid Team Index " + i);
        }

    }


    void Start() {


    }

    public static IGameFacade GetFacade(Team team) {
        if (team != null)
            return facades[team.Id];
        else
            return null;
    }


    public static IGameFacade GetFacade(ITeamable team) {
        if (team != null)
            return GetFacade(team.Team);
        else
            return null;
    }


    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {
        //GUI.contentColor = Color.black;
        //GUI.Label(new Rect(10, 10, 100, 50), "Oro " + (int)player.GetResourceAmount(Resources.Gold).Amount);
        //GUI.Label(new Rect(110, 10, 100, 50), "Alimento " + (int)player.GetResourceAmount(Resources.Food).Amount);
        //GUI.Label(new Rect(210, 10, 100, 50), "Madera " + (int)player.GetResourceAmount(Resources.Wood).Amount);
        //GUI.Label(new Rect(310, 10, 100, 50), "Piedra " + (int)player.GetResourceAmount(Resources.Rock).Amount);
        //GUI.Label(new Rect(10, 20, 100, 50), "NU " + (int)player.NumberofUnits);
        //GUI.Label(new Rect(110, 20, 100, 50), "CU " + (int)player.UnitsCapacity);

       


            GUI.contentColor = Color.cyan;
          
            GUI.Label(new Rect(10, 40, 100, 50), "V2 -> Oro " + (int)player.GetResourceAmount(Resources.Gold).Amount);
            GUI.Label(new Rect(110, 40, 100, 50), "Alimento " + (int)player.GetResourceAmount(Resources.Food).Amount);
            GUI.Label(new Rect(210, 40, 100, 50), "Madera " + (int)player.GetResourceAmount(Resources.Wood).Amount);
            GUI.Label(new Rect(310, 40, 100, 50), "Piedra " + (int)player.GetResourceAmount(Resources.Rock).Amount);
            GUI.Label(new Rect(410, 40, 100, 50), "NU " + (int)player.NumberofUnits);
            GUI.Label(new Rect(510, 40, 100, 50), "CU " + (int)player.UnitsCapacity);

      
    }


    public static void AddResource(int team ,Resources resource ,  float amount) {

        if(team< _splayers.Length )
        _splayers[team].AddResourceAmount(resource, amount);

    }


}
