using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoMataMorre.Business.Entities
{
    public class PlayerMatch: Player
    {
        public PlayerMatch()
        {
            Kills = new List<Kill>();
            Deads = new List<DateTime>();
            Awards = new List<string>();
        }
 
        public List<Kill> Kills { get; set; }

        public List<DateTime> Deads { get; set; }

        public List<string> Awards { get; set; } 

    }
}
