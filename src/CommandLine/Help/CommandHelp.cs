using System.Collections.Generic;

namespace LanceC.CommandLine.Help
{
    internal class CommandHelp : SubCommandHelp
    {
        public CommandHelp(
            string name,
            string? description,
            CommandKind kind,
            bool isHidden,
            string breadcrumb,
            IReadOnlyCollection<string> aliases,
            IReadOnlyCollection<SubCommandHelp> subCommands,
            IReadOnlyCollection<CommandArgumentHelp> arguments,
            IReadOnlyCollection<CommandOptionHelp> options,
            IReadOnlyCollection<CommandOptionHelp> globalOptions)
            : base(name, description, kind, isHidden, aliases)
        {
            Breadcrumb = breadcrumb;
            SubCommands = subCommands;
            Arguments = arguments;
            Options = options;
            GlobalOptions = globalOptions;
        }

        public string Breadcrumb { get; }

        public IReadOnlyCollection<SubCommandHelp> SubCommands { get; }

        public IReadOnlyCollection<CommandArgumentHelp> Arguments { get; }

        public IReadOnlyCollection<CommandOptionHelp> Options { get; }

        public IReadOnlyCollection<CommandOptionHelp> GlobalOptions { get; }
    }
}
