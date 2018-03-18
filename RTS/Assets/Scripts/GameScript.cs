﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour
{
    [SerializeField]
    private AssetTypes assettype;
 
    [SerializeField]
    private BuildingsInfo buldingsInfo;

    [SerializeField]
    private UnitsInfo unitsInfo;

 
    private Player player;

    [SerializeField]
    private Team Playerteam;


    [SerializeField]
    private Team[] teams;

    public Team[] Teams
    {
        get { return teams; }
        set
        {
            teams = value;
            this.diplomacies = new Diplomacy[teams.Length];
            for(int i = 0;i< diplomacies.Length; i++)
            {
                diplomacies[i] = new Diplomacy(teams[i],Postures.Enemy);
            }
        }

    }

    [SerializeField]
    private Diplomacy[] diplomacies;

  
    public Diplomacy[] Diplomacies
    {
        get { return diplomacies; }
        set { diplomacies = value; }

    }

    //private static IGameFacade facade;
    // Use this for initialization
    private static IGameFacade[] facades;


    void Awake()
    {
        if(teams == null || teams.Length == 0)
        {
            throw new UnityException("Debe asignar los respectivos equipos.");
        }


        var gameFacade = ScriptableObject.CreateInstance<GameFacade>();
        gameFacade.Team = Playerteam;
        gameFacade.Player = ScriptableObject.CreateInstance<Player>();
        gameFacade.Player.SetData(gameFacade.Team.InitalTeamData);
        gameFacade.BuildingsInfo = buldingsInfo;
        gameFacade.UnitsInfo = unitsInfo;
        gameFacade.Assettype = assettype;
        gameFacade.Diplomacies = diplomacies;
        player = gameFacade.Player;

        facades = new IGameFacade[teams.Length+1];
        facades[0] = gameFacade;

        for (int i = 0; i< teams.Length; i++)
        {
            var gameFacadei = ScriptableObject.CreateInstance<GameFacade>();
            gameFacadei.Team = teams[i];
            gameFacadei.Player = ScriptableObject.CreateInstance<Player>();
            gameFacadei.Player.SetData(teams[i].InitalTeamData);
            gameFacadei.BuildingsInfo = buldingsInfo;
            gameFacadei.UnitsInfo = unitsInfo;
            gameFacadei.Assettype = assettype;
            gameFacadei.Diplomacies = diplomacies;
            facades[i+1] = gameFacadei;
        }

    }

   


    void Start()
    {    
        
        
    }

    public static IGameFacade GetFacade()
    {
        return facades[0];
    }

    public static IGameFacade GetFacade(Team team)
    {
        return facades[team.Id];
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
