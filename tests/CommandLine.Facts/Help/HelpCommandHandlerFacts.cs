using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using LanceC.CommandLine.Facts.Testing.Fakes;
using LanceC.CommandLine.Help;
using Xunit;

namespace LanceC.CommandLine.Facts.Help
{
    public class HelpCommandHandlerFacts
    {
        public class TheInvokeAsyncMethod : HelpCommandHandlerFacts
        {
            [Fact]
            public async Task SetsInvocationResultToHelpResult()
            {
                // Arrange
                var invocationContext = default(InvocationContext);
                var parser = new CommandLineBuilder(FakeCommands.GetWithHelpCommand())
                    .UseHost(
                        host =>
                        {
                            invocationContext = host.Properties[typeof(InvocationContext)] as InvocationContext;
                            host.UseCommandHandler<FakeHelpCommand, HelpCommandHandler>();
                        })
                    .Build();

                // Act
                await parser.InvokeAsync("help");

                // Assert
                Assert.NotNull(invocationContext);
                Assert.IsType<HelpResult>(invocationContext!.InvocationResult);
            }
        }
    }
}
