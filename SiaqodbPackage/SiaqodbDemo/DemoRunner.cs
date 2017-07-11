using UnityEngine;
using System.Collections;
using SiaqodbDemo;
using Sqo;
using GameEntities;
using System.Collections.Generic;
public class DemoRunner : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

        GameSimulator simulator = new GameSimulator();
        simulator.Play();

       

        //Debug.Log(msg);
        List<Player> recentPlayers = PlayerManager.GetRecentPlayers(5);
        
        Debug.Log("These are recent 5 players logged in:");
        foreach (Player p in recentPlayers)
        {
            Debug.Log(p.Name+" logged in on:"+p.LastLogin.ToShortDateString());
        }

        string name = simulator.GetRandomPlayerName();
        Player currentPlayer = PlayerManager.LoadPlayer(name);
        Debug.Log("Current player is:" + currentPlayer.Name);

        int bestScore = KicksManager.GetBestScore(currentPlayer.Name);

        Debug.Log("Best score for player:" + currentPlayer.Name + " is:" + bestScore.ToString());

        BallKick bestScoreOfAll = KicksManager.GetHighestScore();
        Debug.Log("Best score ever is:" + bestScoreOfAll.Score + " obtained by:" +bestScoreOfAll.PlayerName+" on date:"+bestScoreOfAll.DateRegistered.ToShortDateString());


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
