using System;
using GameEntities;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sqo;
namespace SiaqodbDemo
{
	public class PlayerManager
	{
		
	
        public static List<Player> GetRecentPlayers(int count)
        {
            Siaqodb database = DatabaseFactory.GetInstance();

            IEnumerable<Player> query = (from Player player in database
                                         orderby player.LastLogin descending
                                         select player).Take(count);

            return query.ToList<Player>();
        }
        public static Player LoadPlayer(string name)
        {
            
            Siaqodb database = DatabaseFactory.GetInstance();

            Player p = (from Player player in database
                         where player.Name == name
                         select player).FirstOrDefault<Player>();

            if (p == null)//not exists so create and save
            {
                p = new Player();
                p.Name = name;
                p.LastLogin = DateTime.Now;

                database.StoreObject(p);
            }
            return p;

        }
        public static int TotalPlayers()
        {
            Siaqodb database = DatabaseFactory.GetInstance();

            return database.Count<Player>();
        }
        public static void SavePlayer(Player p)
        {
            Siaqodb database = DatabaseFactory.GetInstance();
            database.StoreObject(p);
        }
	}
}

