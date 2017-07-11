using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEntities
{
    public class BallKick
    {
        public int OID { get; set; }//every entity must have this property defined
        public string PlayerName;
        public int Score;
        public DateTime DateRegistered;
    }
}
