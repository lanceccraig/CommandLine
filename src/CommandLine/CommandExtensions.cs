using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LanceC.CommandLine
{
    public static class CommandExtensions
    {
        public static Command UseHandler<THandler>(this Command command)
            where THandler : ICommandHandler
        {
            Guard.Against.Null(command, nameof(command));

            command.Handler = CommandHandler.Create<IHost, InvocationContext>(
                (host, context) =>
                {
                    var handler = host.Services.GetRequiredService<THandler>();
                    return handler.InvokeAsync(context);
                });

            return command;
        }

        public static Command UseHelpHandler(this Command command)
            => command.UseHandler<HelpCommandHandler>();

        private class HelpCommandHandler : ICommandHandler
        {
            public Task<int> InvokeAsync(InvocationContext context)
            {
                Guard.Against.MissingInvocationContext(context);

                context.InvocationResult = new HelpResult();
                return Task.FromResult<int>(CommandCode.Success);
            }
        }
    }
}
