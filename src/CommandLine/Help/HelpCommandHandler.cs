using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Help
{
    /// <summary>
    /// Provides a handler for commands that only display help text.
    /// </summary>
    public class HelpCommandHandler : ICommandHandler
    {
        /// <summary>
        /// Invokes the command handler.
        /// </summary>
        /// <param name="context">The command line invocation context.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation,
        /// containing the command code.
        /// </returns>
        public Task<int> InvokeAsync(InvocationContext context)
        {
            Guard.Against.MissingInvocationContext(context);

            context.InvocationResult = new HelpResult();

            return Task.FromResult<int>(CommandCode.Success);
        }

        internal static ICommandHandler Create()
            => CommandHandler.Create<InvocationContext>(context => new HelpCommandHandler().InvokeAsync(context));
    }
}
