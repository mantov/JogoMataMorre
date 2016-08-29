using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace JogoMataMorre.Entities
{
    public class Kill
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string Gun { get; set; }

        public Player Victim { get; set; }

    }
}
