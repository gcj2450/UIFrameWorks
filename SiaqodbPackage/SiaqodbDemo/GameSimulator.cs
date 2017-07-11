using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEntities;

namespace SiaqodbDemo
{
    public class GameSimulator
    {
      
        Random random = new Random();
        private string players = "RIGOBERTO,ALPHONSO,TY,SHELBY,RICKIE,NOE,VERN,BOBBIE,REED,JEFFERSON,ELVIS,BERNARDO,MAURICIO,HIRAM,DONOVAN,BASIL,RILEY,OLLIE,NICKOLAS,MAYNARD,SCOT";

        public GameSimulator()
        {
           
        }
        public void Play()
        {
            this.GeneratePlayers();
            this.GenerateScores();
        }
        public string GetRandomPlayerName()
        {
            string[] playerNames = players.Split(',');
            return playerNames[random.Next(20)];
        }
        public void GeneratePlayers()
        {
            if (PlayerManager.TotalPlayers() == 0)
            {
                string[] playerNames = players.Split(',');
                foreach (string pName in playerNames)
                {
                    Player player = new Player();
                    player.Name = pName;
                    player.LastLogin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, random.Next(1, DateTime.Now.Day));
                    PlayerManager.SavePlayer(player);
                }
            }
                
        }
        public void GenerateScores()
        {
            int n = random.Next(1, 5);
             string[] playerNames = players.Split(',');
             foreach (string pName in playerNames)
             {
                 for (int i = 0; i < n; i++)
                 {
                     BallKick bk = new BallKick();
                     bk.Score = random.Next(100);
                     bk.PlayerName = pName;
                     bk.DateRegistered = new DateTime(DateTime.Now.Year, DateTime.Now.Month, random.Next(1, DateTime.Now.Day));
                     KicksManager.SaveBallKick(bk);
                 }
             }
        }
       
    }
}
