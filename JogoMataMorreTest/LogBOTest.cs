using System;
using System.IO;
using System.Linq;
using JogoMataMorre.Business.Entities;
using JogoMataMorre.Business;

using NUnit.Framework;
using NUnit.Framework.Internal;

namespace JogoMataMorreTest
{
    [TestFixture]
    public class LogBOTest
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
        [Repeat(1000)]
        public void TakeNumber()
        {
            string number = new Random().Next(0, int.MaxValue).ToString();
            var line = new string[2];

            line[0] = string.Empty;
            line[1] = string.Format(" New match {0} has started 23/04/2013 15:36:04 ", number);

            var result = _logBO.GetNumber(line);

            Assert.That(number.Trim(), Is.EqualTo(result.Trim()));
        }

        [Test]
        public void TakeDate()
        {
            var line = new string[3];

            line[0] = string.Empty;
            line[1] = " Roman killed Nick using M16 23/04/2013 15:36:33 ";
            line[2] = string.Empty;

            DateTime dateExpected = Convert.ToDateTime("23/04/2013 15:36:33");

            DateTime result;
            DateTime.TryParse("23/04/2013 15:36:33", out result);

            Assert.That(dateExpected, Is.EqualTo(result));
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
