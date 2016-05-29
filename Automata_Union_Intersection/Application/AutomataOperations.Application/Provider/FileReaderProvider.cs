using AutomataOperations.Application.Contracts;
using System;
using System.IO;

namespace AutomataOperations.Application.Provider
{
    public class FileReaderProvider : IFileReaderProvider
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
            return ReadFileAsArray();
        }

        private string [] ReadFileAsArray()
        {
            var fileAsArray = File.ReadAllLines(_filePath);
            return fileAsArray;
        }
    }
}
