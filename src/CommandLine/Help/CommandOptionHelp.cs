using System.Collections.Generic;

namespace LanceC.CommandLine.Help
{
    internal class CommandOptionHelp : CommandArgumentHelp
    {
        public CommandOptionHelp(
            string name,
            string? description,
            bool isHidden,
            bool isRequired,
            IReadOnlyCollection<string> aliases,
            IReadOnlyCollection<string> acceptedValues)
            : base(name, description, isHidden, acceptedValues)
        {
            IsRequired = isRequired;
            Aliases = aliases;
        }

        public bool IsRequired { get; }

        public IReadOnlyCollection<string> Aliases { get; }

        public override string DisplayName => string.Join(' ', Aliases);
    }
}
