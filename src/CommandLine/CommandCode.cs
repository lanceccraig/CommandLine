using Ardalis.SmartEnum;

namespace LanceC.CommandLine
{
    public class CommandCode : SmartEnum<CommandCode>
    {
        public static readonly CommandCode Success = new("Success", 0);

        public static readonly CommandCode Error = new("Error", 1);

        private CommandCode(string name, int value)
            : base(name, value)
        {
        }
    }
}
