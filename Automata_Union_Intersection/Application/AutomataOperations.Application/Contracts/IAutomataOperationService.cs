using AutomataOperations.Application.Models;

namespace AutomataOperations.Application.Contracts
{
    public interface IAutomataOperationService
    {
        DfaAutomata UnionAutomata();
        DfaAutomata IntersectionAutomata();
    }
}
