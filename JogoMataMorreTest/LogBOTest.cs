using System;
using System.IO;
using JogoMataMorre.Entities;
using JogoMataMorre.Business;

using NUnit.Framework;

namespace JogoMataMorreTest
{
    [TestFixture]
    public class LogBOTest
    {
        private LogBO _log;

        [SetUp]
        public void Initial()
        {
            _log = new LogBO();
        }

        [Test]
        public void LogBOExists()
        {
            var logBO = new LogBO();
        }

        [Test]
        public void FilesExists()
        {

            string log = _log.LoadLog("blabla");

            Assert.Throws<FileNotFoundException>(
                () => throw new FileNotFoundException());
        }

        [Test]
        public void LoadLogRuns()
        {
            string log = _log.LoadLog();

            Assert.IsNotEmpty(log);
        }

    }
}
