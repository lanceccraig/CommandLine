using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Help
{
    public class HelpCommandHandler : ICommandHandler
    {
        public Task<int> InvokeAsync(InvocationContext context)
        {
            Guard.Against.MissingInvocationContext(context);

            context.InvocationResult = new HelpResult();

            return Task.FromResult<int>(CommandCode.Success);
        }
    }
}
