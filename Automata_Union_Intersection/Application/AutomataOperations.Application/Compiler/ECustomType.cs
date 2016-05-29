namespace AutomataOperations.Application.Compiler
{
    public enum ECustomType
    {
        Undefined,
        Char,
        AlphabetSimbol,
        AutomataName,
        State,
        Transition, //(A, 0) = B
        Arrow,
        ParenthesesBegin,
        BracketBegin,
        EndOfAutomata,
        EndOfFile
    }
}