using System.Collections.Generic;

namespace LanceC.CommandLine.Help
{
    internal class SubCommandHelp
    {
        public SubCommandHelp(string name, string? description, CommandKind kind, bool isHidden, IReadOnlyCollection<string> aliases)
        {
            Name = name;
            Description = description;
            Kind = kind;
            IsHidden = isHidden;
            Aliases = aliases;
        }

        public string Name { get; }

        public string? Description { get; }

        public CommandKind Kind { get; }

        public bool IsHidden { get; }

        public IReadOnlyCollection<string> Aliases { get; }
    }
}
