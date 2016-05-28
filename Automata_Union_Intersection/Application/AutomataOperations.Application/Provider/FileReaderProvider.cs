using AutomataOperations.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataOperations.Application.Provider
{
    public class FileReaderProvider : IAutomataFileRequest
    {
        private readonly string _filePath;

        public FileReaderProvider(string path)
        {
            _filePath = path;
        }

        public void PrintAutomataRequests()
        {
            throw new NotImplementedException();
        }

        public string[] ReadInputFileRequest()
        {
            throw new NotImplementedException();
        }
    }
}
