using System;
using JogoMataMorre.Business;
using NUnit.Framework;
using JogoMataMorre.Entities;

namespace JogoMataMorre.Test
{
    [TestFixture]
    public class MatchTest
    {
        [Test]
        public void MatchExiste()
        {
            var match = new Match();

        }

        [Test]
        public void OnlyOneMatch()
        {

            var logLines = new string[3];

            logLines[0] = "13/01/2016 18:30:30";
            logLines[1] = " New match 11348965 has started 23/04/2013 15:36:04 ";
            logLines[2] = " New match 11348965 has started 23/04/2013 15:36:04 ";

            var isNewMatch = false;
            var timeMatch = new DateTime();

            foreach (var logLine in logLines)
            {
                isNewMatch = DateTime.TryParse(logLine, out timeMatch) || isNewMatch;
                isNewMatch = logLine.Contains("New match") || isNewMatch;
            }
          
        }

        [Test]
        public void Match()
        {
            var logBO = new LogBO();
            logBO.LoadGame();
        } 
    }
}
