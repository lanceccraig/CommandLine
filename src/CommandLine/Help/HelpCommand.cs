using System.CommandLine;

namespace LanceC.CommandLine.Help
{
    /// <summary>
    /// Represents a command that displays help text.
    /// </summary>
    public class HelpCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommand"/> class.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="description">The command description.</param>
        protected HelpCommand(string name, string? description = null)
            : base(name, description)
        {
            Handler = HelpCommandHandler.Create();
        }
    }
}
