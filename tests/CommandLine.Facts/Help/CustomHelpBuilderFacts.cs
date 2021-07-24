using System.Text;
using LanceC.CommandLine.Facts.Testing.Fakes;
using LanceC.CommandLine.Help;
using Xunit;

namespace LanceC.CommandLine.Facts.Help
{
    public class CustomHelpBuilderFacts
    {
        private readonly FakeConsole _console = new();

        private CustomHelpBuilder CreateSystemUnderTest()
            => new(_console);

        public class TheWriteMethod : CustomHelpBuilderFacts
        {
            [Fact]
            public void WritesSubGroupsAndSubCommandsForGroup()
            {
                // Arrange
                var command = FakeCommands.Get("testhost");
                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine()
                    .AppendLine("Group")
                    .AppendLine("    testhost : Fake Executable")
                    .AppendLine()
                    .AppendLine("Sub Groups:")
                    .AppendLine("    sg                 : Fake SubGroup")
                    .AppendLine()
                    .AppendLine("Commands:")
                    .AppendLine("    c1                 : Fake Command One")
                    .AppendLine()
                    .AppendLine("Global Options:")
                    .AppendLine("    -h /h --help -? /? : Show help and usage information.")
                    .AppendLine()
                    .ToString();

                // Act
                sut.Write(command);
                var actual = _console.OutResult;

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WritesSubCommandsForGroup()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg");
                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine()
                    .AppendLine("Group")
                    .AppendLine("    testhost sg : Fake SubGroup")
                    .AppendLine()
                    .AppendLine("Commands:")
                    .AppendLine("    c2                 : Fake Command Two")
                    .AppendLine("    c3                 : Fake Command Three")
                    .AppendLine()
                    .AppendLine("Global Options:")
                    .AppendLine("    -h /h --help -? /? : Show help and usage information.")
                    .AppendLine()
                    .ToString();

                // Act
                sut.Write(command);
                var actual = _console.OutResult;

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WritesArgumentsAndOptionsForCommand()
            {
                // Arrange
                var command = FakeCommands.Get("testhost c1");
                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine()
                    .AppendLine("Command")
                    .AppendLine("    testhost c1 : Fake Command One")
                    .AppendLine("                  Aliases: cone")
                    .AppendLine()
                    .AppendLine("Arguments:")
                    .AppendLine("    <foo>                         : Foo")
                    .AppendLine()
                    .AppendLine("Options:")
                    .AppendLine("    --bar -b           [Required] : Bar")
                    .AppendLine("    --baz                         : Baz")
                    .AppendLine()
                    .AppendLine("Global Options:")
                    .AppendLine("    -h /h --help -? /?            : Show help and usage information.")
                    .AppendLine()
                    .ToString();

                // Act
                sut.Write(command);
                var actual = _console.OutResult;

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WritesArgumentsForCommand()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg c2");
                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine()
                    .AppendLine("Command")
                    .AppendLine("    testhost sg c2 : Fake Command Two")
                    .AppendLine()
                    .AppendLine("Arguments:")
                    .AppendLine("    <foo>              : Foo")
                    .AppendLine("    <bar>              : Bar")
                    .AppendLine("                         Accepted values: false, true")
                    .AppendLine("    <baz>              : Baz")
                    .AppendLine("                         Accepted values: Foo, Bar, Baz")
                    .AppendLine("    <qux>              : Qux")
                    .AppendLine("                         Accepted values: Foo, Bar, Baz")
                    .AppendLine()
                    .AppendLine("Global Options:")
                    .AppendLine("    -h /h --help -? /? : Show help and usage information.")
                    .AppendLine()
                    .ToString();

                // Act
                sut.Write(command);
                var actual = _console.OutResult;

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WritesOptionsForCommand()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg c3");
                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine()
                    .AppendLine("Command")
                    .AppendLine("    testhost sg c3 : Fake Command Three")
                    .AppendLine()
                    .AppendLine("Options:")
                    .AppendLine("    --foo -f           [Required] : Foo")
                    .AppendLine("    --bar                         : Bar")
                    .AppendLine("                                    Accepted values: false, true")
                    .AppendLine("    -b                            : Baz")
                    .AppendLine("                                    Accepted values: Foo, Bar, Baz")
                    .AppendLine("    --qux                         : Qux")
                    .AppendLine("                                    Accepted values: Foo, Bar, Baz")
                    .AppendLine()
                    .AppendLine("Global Options:")
                    .AppendLine("    -h /h --help -? /?            : Show help and usage information.")
                    .AppendLine()
                    .ToString();

                // Act
                sut.Write(command);
                var actual = _console.OutResult;

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}
