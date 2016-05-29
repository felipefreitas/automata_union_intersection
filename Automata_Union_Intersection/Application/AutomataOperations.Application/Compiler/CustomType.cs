namespace AutomataOperations.Application.Compiler
{
    public class CustomType
    {
        public ECustomType Type { get; set; }
        public string Value { get; set; }

        public CustomType(ECustomType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}