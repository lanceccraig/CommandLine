using System.Collections.Generic;

namespace LanceC.CommandLine.Help
{
    internal class CommandArgumentHelp
    {
        public CommandArgumentHelp(string name, string? description, bool isHidden, IReadOnlyCollection<string> acceptedValues)
        {
            Name = name;
            Description = description;
            IsHidden = isHidden;
            AcceptedValues = acceptedValues;
        }

        public string Name { get; }

        public string? Description { get; }

        public bool IsHidden { get; }

        public IReadOnlyCollection<string> AcceptedValues { get; }

        public virtual string DisplayName => $"<{Name}>";
    }
}
