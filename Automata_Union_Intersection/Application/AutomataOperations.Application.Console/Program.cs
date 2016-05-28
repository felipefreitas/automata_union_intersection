using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataOperations.Application.Console
{
    public class Program
    {
        protected static string AutomataFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_automataFilePath))
                {
                    _automataFilePath = ConfigurationManager.AppSettings["AutomataFilePath"];
                }
                return _automataFilePath;
            }
        }

        private static string _automataFilePath;

        public static void Main(string[] args)
        {
            ReadFile();
            Process();
        }

        private static void ReadFile()
        {
            throw new NotImplementedException();
        }

        private static void Process()
        {
            throw new NotImplementedException();
        }
    }
}
