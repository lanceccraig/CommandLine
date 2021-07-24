using Ardalis.SmartEnum;

namespace LanceC.CommandLine.Help
{
    internal class CommandKind : SmartEnum<CommandKind>
    {
        public static readonly CommandKind Group = new("Group", 1);

        public static readonly CommandKind Command = new("Command", 2);

        private CommandKind(string name, int id)
            : base(name, id)
        {
        }
    }
}
