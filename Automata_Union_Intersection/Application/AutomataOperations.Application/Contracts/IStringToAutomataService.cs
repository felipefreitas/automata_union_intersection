using AutomataOperations.Application.Models;
using System.Collections.Generic;

namespace AutomataOperations.Application.Contracts
{
    public interface IStringToAutomataService
    {
        IEnumerable<DfaAutomata> ProcessString(string [] toProcess);
    }
}
