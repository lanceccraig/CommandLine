using System.CommandLine;

namespace LanceC.CommandLine.Help
{
    /// <summary>
    /// Represents a root command that displays help text.
    /// </summary>
    public abstract class RootHelpCommand : RootCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootHelpCommand"/> class.
        /// </summary>
        /// <param name="description">The command description.</param>
        protected RootHelpCommand(string description = "")
            : base(description)
        {
            Handler = HelpCommandHandler.Create();
        }
    }
}
