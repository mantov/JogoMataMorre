using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JogoMataMorre.Entities;

namespace JogoMataMorre.Business
{
    public class LogBO
    {

    //[0]: "23/04/2013 15:34:22 "
    //[1]: " New match 11348965 has started 23/04/2013 15:36:04 "
    //[2]: " Roman killed Nick using M16 23/04/2013 15:36:33 "
    //[3]: " killed Nick by DROWN 23/04/2013 15:39:22 "
    //[4]: " Match 11

        public Match LoadMatch(string[] logLines)
        {
            var match = new Match();
            match.Start = Convert.ToDateTime(logLines[0]);
            match.Number = GetNumber(logLines);



            return new Match();


        }

        private string GetNumber(string[] logLines)
        {
            string begin = "New match";
            string end = "has started";
            var start = logLines[1].IndexOf(begin) + begin.Length;
            var final = logLines[1].IndexOf(end);


            for (int contador = 2; contador < logLines.Length -1; contador++)
            {
                
            }
            return logLines[1].Substring(start, final - start).Trim();
        }

        public void LoadGame()
        {
            var logContent = LoadLog();
            var logLines = logContent.Split('-');
            var newMatch = LoadMatch(logLines);


        }


        public string LoadLog(string path = @"C:\Users\Slave\Source\Repos\JogoMataMorre\JogoMataMorre\Log\Log.Log")
        {
            string result = string.Empty;

            using (var reader =new StreamReader(path))
            {
                result = reader.ReadToEnd();
            }
            
            return result;
        }


    }
}
