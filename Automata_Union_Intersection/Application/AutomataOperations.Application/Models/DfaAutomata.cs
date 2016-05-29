using System.Collections.Generic;

namespace AutomataOperations.Application.Models
{
    public class DfaAutomata
    {
        public State InitialState { get; set; }
        public IEnumerable<State> States { get; set; }
        public IEnumerable<State> AcceptionStates { get; set; }
    }
}
