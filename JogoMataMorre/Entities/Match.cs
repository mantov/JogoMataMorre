using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoMataMorre.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<Player> Players { get; set; }

    }
}
