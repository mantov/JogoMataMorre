using System;
using System.IO;
using JogoMataMorre.Entities;
using JogoMataMorre.Business;

using NUnit.Framework;
using NUnit.Framework.Internal;

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

        //private void LoadNoFile()
        //{
        //    string log = _log.LoadLog("blabla");

        //}

        //[Test]
        //public void FilesNoExists()
        //{

        //    string log = _log.LoadLog("blabla");

        //    Assert.Throws<FileNotFoundException>(LoadNoFile(),throw new FileNotFoundException());{
        //}

        [Test]
        public void LoadLogRuns()
        {
            string log = _log.LoadLog();

            Assert.IsNotEmpty(log);
        }

        [Test]
        public void HaveData()
        {
            string log = _log.LoadLog();
            var points = log.Split('-');

            Assert.GreaterOrEqual(points.Length,2);
        }
    }
}
