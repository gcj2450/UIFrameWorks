using System;
using GameEntities;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sqo;
namespace SiaqodbDemo
{
	public class KicksManager
	{
        
      
      
        public static int GetBestScore(string playerName)
        {
            Siaqodb database = DatabaseFactory.GetInstance();

            var query = from BallKick ballK in database
                        where ballK.PlayerName == playerName
                        orderby ballK.Score descending
                        select ballK.Score;
            
            return query.FirstOrDefault();

        }
        public static BallKick GetHighestScore()
        {
            Siaqodb database = DatabaseFactory.GetInstance();

            var query = from BallKick ballK in database
                        orderby ballK.Score descending
                        select ballK;

            return query.FirstOrDefault();

        }
        public static void SaveBallKick(BallKick b)
        {
            Siaqodb database = DatabaseFactory.GetInstance();
            database.StoreObject(b);
        }
	}
}

