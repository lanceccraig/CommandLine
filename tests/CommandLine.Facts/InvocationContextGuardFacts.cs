using System;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Xunit;

namespace LanceC.CommandLine.Facts
{
    public class InvocationContextGuardFacts
    {
        public class TheMissingInvocationContextMethod : InvocationContextGuardFacts
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenContextIsNull()
            {
                // Arrange
                var context = default(InvocationContext?);

                // Act
                var exception = Record.Exception(() => Guard.Against.MissingInvocationContext(context!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public async Task DoesNotThrowExceptionWhenContextIsNotNull()
            {
                // Arrange
                var context = default(InvocationContext);

                var parser = new CommandLineBuilder()
                    .UseHost(
                        host =>
                        {
                            context = host.Properties[typeof(InvocationContext)] as InvocationContext;
                        })
                    .Build();
                await parser.InvokeAsync(Array.Empty<string>());

                // Act
                var exception = Record.Exception(() => Guard.Against.MissingInvocationContext(context!));

                // Assert
                Assert.Null(exception);
            }
        }
    }
}
