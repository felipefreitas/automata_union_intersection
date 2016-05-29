namespace AutomataOperations.Application.Contracts
{
    public interface IFileReaderProvider
    {
        string[] ReadInputFileRequest();
        void PrintAutomataRequests();
    }
}
