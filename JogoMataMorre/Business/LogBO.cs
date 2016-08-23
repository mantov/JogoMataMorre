using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoMataMorre.Business
{
    public class LogBO
    {
        public string LoadLog(string path =  @"Log\log.log")
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
