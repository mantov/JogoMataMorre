﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoMataMorre.Entities
{
    public class Player
    {
        public Player()
        {
            Kills = new List<Kill>();
            Deads = new List<DateTime>();
        }
        public int Id { get; set; }

        public string Nome { get; set; }

        public List<Kill> Kills { get; set; }

        public List<DateTime> Deads { get; set; }

    }
}
