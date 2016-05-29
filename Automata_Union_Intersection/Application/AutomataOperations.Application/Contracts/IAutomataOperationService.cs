using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataOperations.Application.Contracts
{
    public interface IAutomataOperationService
    {
        void UnionAutomata();
        void IntersectionAutomata();
    }
}
