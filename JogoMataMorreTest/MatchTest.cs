using System;
using JogoMataMorre.Business;
using NUnit.Framework;
using JogoMataMorre.Entities;

namespace JogoMataMorre.Test
{
    [TestFixture]
    public class MatchTest
    {
        #region Private Structure

        private LogBO _logBO;
        private string _logFileContent;
        private string[] _logLines;

        private void LogBOFactory()
        {
            _logBO = new LogBO();
        }

        private void LoadPrivateLog()
        {
            _logFileContent = _logBO.LoadLog();
        }

        private void SplitLog()
        {
            _logLines = _logFileContent.Split('-');
        }
        #endregion

        [SetUp]
        public void Initial()
        {
            LogBOFactory();
            LoadPrivateLog();
            SplitLog();
        }

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
        public void LoadMatch()
        {
            var logBO = new LogBO();
            logBO.LoadMatch(_logLines);
        } 
    }
}
