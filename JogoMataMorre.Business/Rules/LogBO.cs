using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JogoMataMorre.Business.Entities;
using Match = JogoMataMorre.Business.Entities.Match;

namespace JogoMataMorre.Business
{
    public class LogBO
    {

        //[0]: "23/04/2013 15:34:22 "
        //[1]: " New match 11348965 has started 23/04/2013 15:36:04 "
        //[2]: " Roman killed Nick using M16 23/04/2013 15:36:33 "
        //[3]: " killed Nick by DROWN 23/04/2013 15:39:22 "
        //[4]: " Match 11348965 has ended

        private const char _lineSeparator = '-';
        private readonly string[] _macthesSeparator = new string[] { "ended" };

        private static string DateTimeRegexPattern
        {
            get { return @"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}"; }
        }

        //public List<Match> LoadMatches(string[] logLines)
        //{
        //}

        public Match LoadMatch(string[] logLines)
        {
            var match = new Match();

            match.Start = GetStart(logLines);
            match.Number = GetNumber(logLines);
            match.Players = GetPlayers(logLines, match);
            match.End = GetEnd(logLines, match);

            return match;
        }

        public List<PlayerMatch> GetPlayers(string[] logLines, Match match)
        {
            var players = match.Players ?? new List<PlayerMatch>();

            for (int count = 1; count < logLines.Length - 1; count++)
            {
                if (ValidKill(logLines[count], match.Number))
                {
                    var playerKiller = AddKiller(logLines, count);
                    var playerVictim = AddVictim(logLines, count);

                    players = AddPlayer(players, playerKiller);
                    players = AddPlayer(players, playerVictim);
                }
            }

            return players;
        }

        private static bool ValidKill(string logLine, string matchNumber)
        {
            var notAutokill = !AutoKill(logLine);
            var matchKey = string.Format("MATCH {0}", matchNumber);
            var notAMatchRegister = !logLine.ToUpper().Contains(matchKey);

            return notAutokill && notAMatchRegister;
        }

        public PlayerMatch AddVictim(string[] loglines, int count)
        {
            var victim = new PlayerMatch();
            var line = loglines[count];
            var dateKill = GetDate(loglines[count - 1]);

            if (dateKill != null)
            {
                victim.Deads.Add(dateKill.Value);
                victim.Name = getVictimName(loglines[count]);
            }

            return victim;
        }

        private PlayerMatch AddKiller(string[] loglines, int count)
        {
            var killer = new PlayerMatch();
            var line = loglines[count];
            var kill = new Kill();
            var dateKill = GetDate(loglines[count - 1]);

            killer.Name = getKillerName(loglines[count]);

            if (dateKill != null)
            {
                kill.Time = dateKill.Value;
                kill.Victim = getVictimName(loglines[count]);
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


        private string getVictimName(string line)
        {
            //Roman killed Nick using M16 23 / 04 / 2013 15:36:33 "
            var keyWordBegin = "killed";
            var numberBegin = line.IndexOf(keyWordBegin) + keyWordBegin.Length;
            var keyWordEnd = "using";
            var sizeName = line.IndexOf(keyWordEnd) - numberBegin;
            var name = line.Substring(numberBegin, sizeName).Trim();

            return name;
        }

        public List<PlayerMatch> AddPlayer(List<PlayerMatch> players, PlayerMatch playerMatch)
        {
            if (players.Count == 0)
            {
                players.Add(playerMatch);

                return players;
            }

            if (players.FirstOrDefault(p => p.Name == playerMatch.Name) != null)
            {
                players.FirstOrDefault(p => p.Name == playerMatch.Name).Kills.AddRange(playerMatch.Kills);
                players.FirstOrDefault(p => p.Name == playerMatch.Name).Deads.AddRange(playerMatch.Deads);

                return players;
            }

            players.Add(playerMatch);

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

        private static DateTime GetEnd(string[] logLines, Match match)
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

        private static DateTime? GetDate(string logLine)
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

        public string[] LoadMatches()
        {
            var logContent = LoadLog();
            var matches = logContent.Split(_macthesSeparator, StringSplitOptions.RemoveEmptyEntries);

            return matches;
        }

        

        public List<Match> LoadGameLog()
        {
            var matches = LoadMatches();
            var gamelist = new List<Match>();
            foreach (var match in matches)
            {
                var game = match + _macthesSeparator.First();
                var logLines = game.Split(_lineSeparator).Where(l => !string.IsNullOrEmpty(l.Trim())).ToArray();
                var newMatch = LoadMatch(logLines);

                gamelist.Add(newMatch);
            }

            GetGameScores(gamelist);

            return gamelist;
        }

        public void GetGameScores(List<Match> games)
        {
            foreach (var game in games)
            {
                GetAwards(game);
            }
        }

        private static void GetAwards(Match game)
        {
            var players = game.Players.OrderByDescending(p => p.Kills.Count).ToList();

            game.Players = players;

            foreach (var player in game.Players)
            {
                if (player.Kills.Count > 0)
                {
                    GetFavoriteGun(player);
                }

                if (player.Kills.Count >= 5)
                {
                    GetOverkill(player);
                }
            }
        }

        private static void GetOverkill(PlayerMatch player)
        {
            int differenceForOverkill = 4;

            for (int counter = differenceForOverkill; counter <= player.Kills.Count - 1; counter++)
            {
                var fifthKillTime = player.Kills[counter].Time;
                var firstKillTime = player.Kills[counter - differenceForOverkill].Time;
                var lessFiveMinutesKill = (fifthKillTime - firstKillTime).Minutes <= 5;

                if (lessFiveMinutesKill)
                {
                    player.Awards.Add(string.Format("Overkiller"));

                    return;
                }
            }
        }

        private static void GetFavoriteGun(PlayerMatch player)
        {
            var favoriteGun = player.Kills.GroupBy(s => s.Gun)
                .OrderByDescending(s => s.Count())
                .FirstOrDefault().Key;

            player.Awards.Add(string.Format("Favorite gun: {0}", favoriteGun));
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
