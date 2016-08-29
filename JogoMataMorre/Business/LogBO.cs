using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JogoMataMorre.Entities;
using Match = JogoMataMorre.Entities.Match;

namespace JogoMataMorre.Business
{
    public class LogBO
    {

        //[0]: "23/04/2013 15:34:22 "
        //[1]: " New match 11348965 has started 23/04/2013 15:36:04 "
        //[2]: " Roman killed Nick using M16 23/04/2013 15:36:33 "
        //[3]: " killed Nick by DROWN 23/04/2013 15:39:22 "
        //[4]: " Match 11348965 has ended

        private static string DateTimeRegexPattern
        {
            get { return @"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}"; }
        }
        
        public Match LoadMatch(string[] logLines)
        {
            var match = new Match();

            match.Start = GetStart(logLines);
            match.Number = GetNumber(logLines);
            match.Players = GetPlayers(logLines, match);
            match.End = GetEnd(logLines, match);

            return new Match();
        }

        public List<Player> GetPlayers(string[] logLines, Match match)
        {
            var players = match.Players??new List<Player>();

            for (int count = 1; count < logLines.Length -1; count++)
            {
                if (ValidKill(logLines[count],match.Number))
                {
                    var playerKiller = AddKiller(logLines, count);
                    var playerVictim = AddVictim(logLines, count);

                    players = AddPlayer(players,playerKiller);
                    players = AddPlayer(players, playerVictim);
                }
            }

            return  players;
        }

        private static bool ValidKill(string logLine, string matchNumber)
        {
            var notAutokill = !AutoKill(logLine);
            var matchKey = string.Format("MATCH {0}", matchNumber);
            var notAMatchRegister = !logLine.ToUpper().Contains(matchKey);

            return notAutokill && notAMatchRegister;
        }

        public Player AddVictim(string[] loglines, int count)
        {
            var killer = new Player();
            var line = loglines[count];
            var kill = new Kill();
            var dateKill = GetDate(loglines[count - 1]);

            if (dateKill != null)
            {
                kill.Time = dateKill.Value;
            }

            kill.Gun = getGun(line);

            return new Player();
        }

        private Player AddKiller(string[] loglines, int count)
        {
            var killer = new Player();
            var line = loglines[count];
            var kill = new Kill();
            var dateKill = GetDate(loglines[count - 1]);

            killer.Nome = getKillerName(loglines[count]);

            if (dateKill != null)
            {
                kill.Time = dateKill.Value;
            }

            kill.Gun = getGun(line);
            killer.Kills.Add(kill);

            return killer;
        }

        private string getGun(string line)
        {

            var regex = new Regex(DateTimeRegexPattern);
            string lineWithoutDate = regex.Replace(line, string.Empty);
            var keyWordString = "using";
            var stringGunBegin = lineWithoutDate.IndexOf(keyWordString, StringComparison.Ordinal) + keyWordString.Length;
            var stringGunEnd = lineWithoutDate.Length - stringGunBegin;
            var gun = lineWithoutDate.Substring(stringGunBegin, stringGunEnd).Trim();

            return gun;
        }

        private string getKillerName(string line)
        {
            var keyWord = "killed";
            var name = line.Substring(0, line.IndexOf(keyWord)).Trim();

            return name;
        }

        public List<Player> AddPlayer(List<Player> players, Player player)
        {
            if (players.Count == 0) 
            {
                players.Add(player);
                return players;
            }

           players.FirstOrDefault(p => p.Nome == player.Nome) == null?
           players.FirstOrDefault(p => p.Nome == player.Nome).Kills.AddRange(player.Kills);

            return players;      
        }

        private static bool AutoKill(string logLine)
        {
            return logLine.Contains("by");
        }

        private static DateTime GetStart(string[] logLines)
        {
            DateTime dateStart;
            if (DateTime.TryParse(logLines[0], out dateStart))
            {
                return Convert.ToDateTime(logLines[0]);
            }
            throw new Exception("No start detected.");
        }

        public DateTime GetEnd(string[] logLines, Match match)
        {
            //string endMatch = string.Format("Match {0} has ended" ,match.Number);
            var penultimate = logLines.Length - 2;
            DateTime? dateEnd = GetDate(logLines[penultimate]);

            if (dateEnd != null)
            {
                return dateEnd.Value;
            }

            throw new Exception("No end detected.");
        }

        private DateTime? GetDate(string logLine)
        {
            var regex = new Regex(DateTimeRegexPattern);
            string dateString = regex.Match(logLine).Value;
            DateTime dateConverted;

            DateTime.TryParse(dateString, out dateConverted);

            return dateConverted;
        }

        public string GetNumber(string[] logLines)
        {
            string newMatch = "New match";
            string hasStarted = "has started";
            var start = logLines[1].IndexOf(newMatch, StringComparison.Ordinal) + newMatch.Length;
            var final = logLines[1].IndexOf(hasStarted, StringComparison.Ordinal);

            return logLines[1].Substring(start, final - start).Trim();
        }

        public void LoadGame()
        {
            var logContent = LoadLog();
            var logLines = logContent.Split('-');
            var newMatch = LoadMatch(logLines);

        }

        //public string LoadLog(string path = @"C:\Users\Slave\Source\Repos\JogoMataMorre\JogoMataMorre\Log\Log.Log")
        public string LoadLog(string path = @"c:\Temp\Log.Log")
        {
            string result = string.Empty;

            using (var reader = new StreamReader(path))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
        
    }
}
