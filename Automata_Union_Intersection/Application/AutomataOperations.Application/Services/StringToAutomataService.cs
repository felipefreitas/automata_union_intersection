using AutomataOperations.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomataOperations.Application.Models;

namespace AutomataOperations.Application.Services
{
    public class StringToAutomataService : IStringToAutomataService
    {
        public IEnumerable<DfaAutomata> ProcessString(string [] toProcess)
        {
            throw new NotImplementedException();
        }
    }
}
