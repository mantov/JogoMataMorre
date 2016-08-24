using System;
using System.IO;
using System.Linq;
using JogoMataMorre.Entities;
using JogoMataMorre.Business;

using NUnit.Framework;
using NUnit.Framework.Internal;

namespace JogoMataMorreTest
{
    [TestFixture]
    public class LogBOTest
    {
        #region Private Structure

        private LogBO _log;
        private string _logFileContent;
        private string[] _logLines;

        private void LogBOFactory()
        {
            _log = new LogBO();
        }

        private void LoadPrivateLog()
        {
            _logFileContent = _log.LoadLog();
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
        public void LogBOExists()
        {
            var logBO = new LogBO();
        }

        [Test]
        public void LoadLogRuns()
        {
            Assert.IsNotEmpty(_logFileContent);
        }

        [Test]
        public void HaveData()
        {
            Assert.GreaterOrEqual(_logLines.Length,2);
        }

        [Test]
        public void FirstLineIsDate()
        {
            DateTime firstDate;
            var logLines = _logLines;
            bool isDAte = DateTime.TryParse(logLines[0], out firstDate);

            Assert.IsTrue(isDAte);
        }


    }
}
