using System.Linq;
using LanceC.CommandLine.Facts.Testing.Fakes;
using LanceC.CommandLine.Help;
using Xunit;

namespace LanceC.CommandLine.Facts.Help
{
    public class CommandHelpConverterFacts
    {
        public class TheConvertMethod
        {
            [Fact]
            public void ConvertsGroupToCommandHelp()
            {
                // Arrange
                var command = FakeCommands.Get("testhost");

                // Act
                var commandHelp = CommandHelpConverter.Convert(command);

                // Assert
                Assert.Equal("testhost", commandHelp.Name);
                Assert.Equal("Fake Executable", commandHelp.Description);
                Assert.Equal(CommandKind.Group, commandHelp.Kind);
                Assert.False(commandHelp.IsHidden);
                Assert.Equal("testhost", commandHelp.Breadcrumb);
                Assert.Empty(commandHelp.Aliases);

                Assert.Equal(2, commandHelp.SubCommands.Count);
                Assert.Single(
                    commandHelp.SubCommands,
                    subCommand =>
                        subCommand.Name == "sg" &&
                        subCommand.Description == "Fake SubGroup" &&
                        subCommand.Kind == CommandKind.Group &&
                        !subCommand.IsHidden &&
                        !subCommand.Aliases.Any());
                Assert.Single(
                    commandHelp.SubCommands,
                    subCommand =>
                        subCommand.Name == "c1" &&
                        subCommand.Description == "Fake Command One" &&
                        subCommand.Kind == CommandKind.Command &&
                        !subCommand.IsHidden &&
                        subCommand.Aliases.Count == 1 &&
                        subCommand.Aliases.Count(alias => alias == "cone") == 1);

                Assert.Empty(commandHelp.Arguments);
                Assert.Empty(commandHelp.Options);

                AssertGlobalOptionsEqual(commandHelp);
            }

            [Fact]
            public void ConvertsSubGroupToCommandHelp()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg");

                // Act
                var commandHelp = CommandHelpConverter.Convert(command);

                // Assert
                Assert.Equal("sg", commandHelp.Name);
                Assert.Equal("Fake SubGroup", commandHelp.Description);
                Assert.Equal(CommandKind.Group, commandHelp.Kind);
                Assert.False(commandHelp.IsHidden);
                Assert.Equal("testhost sg", commandHelp.Breadcrumb);
                Assert.Empty(commandHelp.Aliases);

                Assert.Equal(3, commandHelp.SubCommands.Count);
                Assert.Single(
                    commandHelp.SubCommands,
                    subCommand =>
                        subCommand.Name == "c2" &&
                        subCommand.Description == "Fake Command Two" &&
                        subCommand.Kind == CommandKind.Command &&
                        !subCommand.IsHidden &&
                        !subCommand.Aliases.Any());
                Assert.Single(
                    commandHelp.SubCommands,
                    subCommand =>
                        subCommand.Name == "c3" &&
                        subCommand.Description == "Fake Command Three" &&
                        subCommand.Kind == CommandKind.Command &&
                        !subCommand.IsHidden &&
                        !subCommand.Aliases.Any());
                Assert.Single(
                    commandHelp.SubCommands,
                    subCommand =>
                        subCommand.Name == "c4" &&
                        subCommand.Description == "Fake Command Four" &&
                        subCommand.Kind == CommandKind.Command &&
                        subCommand.IsHidden &&
                        !subCommand.Aliases.Any());

                Assert.Empty(commandHelp.Arguments);
                Assert.Empty(commandHelp.Options);

                AssertGlobalOptionsEqual(commandHelp);
            }

            [Fact]
            public void ConvertsCommandWithArgumentsAndOptionsToCommandHelp()
            {
                // Arrange
                var command = FakeCommands.Get("testhost c1");

                // Act
                var commandHelp = CommandHelpConverter.Convert(command);

                // Assert
                Assert.Equal("c1", commandHelp.Name);
                Assert.Equal("Fake Command One", commandHelp.Description);
                Assert.Equal(CommandKind.Command, commandHelp.Kind);
                Assert.False(commandHelp.IsHidden);
                Assert.Equal("testhost c1", commandHelp.Breadcrumb);

                Assert.Equal(1, commandHelp.Aliases.Count);
                Assert.Single(commandHelp.Aliases, alias => alias == "cone");

                Assert.Empty(commandHelp.SubCommands);

                Assert.Equal(1, commandHelp.Arguments.Count);
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "foo" &&
                        argument.Description == "Foo" &&
                        !argument.IsHidden &&
                        !argument.AcceptedValues.Any());

                Assert.Equal(2, commandHelp.Options.Count);
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "bar" &&
                        option.Description == "Bar" &&
                        !option.IsHidden &&
                        option.IsRequired &&
                        option.Aliases.Count == 2 &&
                        option.Aliases.Count(alias => alias == "--bar") == 1 &&
                        option.Aliases.Count(alias => alias == "-b") == 1 &&
                        !option.AcceptedValues.Any());
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "baz" &&
                        option.Description == "Baz" &&
                        !option.IsHidden &&
                        !option.IsRequired &&
                        option.Aliases.Count == 1 &&
                        option.Aliases.Count(alias => alias == "--baz") == 1 &&
                        !option.AcceptedValues.Any());

                AssertGlobalOptionsEqual(commandHelp);
            }

            [Fact]
            public void ConvertsCommandWithArgumentsToCommandHelp()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg c2");

                // Act
                var commandHelp = CommandHelpConverter.Convert(command);

                // Assert
                Assert.Equal("c2", commandHelp.Name);
                Assert.Equal("Fake Command Two", commandHelp.Description);
                Assert.Equal(CommandKind.Command, commandHelp.Kind);
                Assert.False(commandHelp.IsHidden);
                Assert.Equal("testhost sg c2", commandHelp.Breadcrumb);

                Assert.Empty(commandHelp.Aliases);
                Assert.Empty(commandHelp.SubCommands);

                Assert.Equal(5, commandHelp.Arguments.Count);
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "foo" &&
                        argument.Description == "Foo" &&
                        !argument.IsHidden &&
                        !argument.AcceptedValues.Any());
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "bar" &&
                        argument.Description == "Bar" &&
                        !argument.IsHidden &&
                        argument.AcceptedValues.Count == 2 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "false") == 1 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "true") == 1);
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "baz" &&
                        argument.Description == "Baz" &&
                        !argument.IsHidden &&
                        argument.AcceptedValues.Count == 3 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Foo") == 1 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Bar") == 1 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Baz") == 1);
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "qux" &&
                        argument.Description == "Qux" &&
                        !argument.IsHidden &&
                        argument.AcceptedValues.Count == 3 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Foo") == 1 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Bar") == 1 &&
                        argument.AcceptedValues.Count(acceptedValue => acceptedValue == "Baz") == 1);
                Assert.Single(
                    commandHelp.Arguments,
                    argument =>
                        argument.Name == "hidden" &&
                        argument.Description == "Hidden" &&
                        argument.IsHidden &&
                        !argument.AcceptedValues.Any());

                Assert.Empty(commandHelp.Options);

                AssertGlobalOptionsEqual(commandHelp);
            }

            [Fact]
            public void ConvertsCommandWithOptionsToCommandHelp()
            {
                // Arrange
                var command = FakeCommands.Get("testhost sg c3");

                // Act
                var commandHelp = CommandHelpConverter.Convert(command);

                // Assert
                Assert.Equal("c3", commandHelp.Name);
                Assert.Equal("Fake Command Three", commandHelp.Description);
                Assert.Equal(CommandKind.Command, commandHelp.Kind);
                Assert.False(commandHelp.IsHidden);
                Assert.Equal("testhost sg c3", commandHelp.Breadcrumb);

                Assert.Empty(commandHelp.Aliases);
                Assert.Empty(commandHelp.SubCommands);
                Assert.Empty(commandHelp.Arguments);

                Assert.Equal(5, commandHelp.Options.Count);
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "foo" &&
                        option.Description == "Foo" &&
                        !option.IsHidden &&
                        option.IsRequired &&
                        option.Aliases.Count == 2 &&
                        option.Aliases.Count(alias => alias == "--foo") == 1 &&
                        option.Aliases.Count(alias => alias == "-f") == 1 &&
                        !option.AcceptedValues.Any());
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "bar" &&
                        option.Description == "Bar" &&
                        !option.IsHidden &&
                        !option.IsRequired &&
                        option.Aliases.Count == 1 &&
                        option.Aliases.Count(alias => alias == "--bar") == 1 &&
                        option.AcceptedValues.Count == 2 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "false") == 1 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "true") == 1);
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "b" &&
                        option.Description == "Baz" &&
                        !option.IsHidden &&
                        !option.IsRequired &&
                        option.Aliases.Count == 1 &&
                        option.Aliases.Count(alias => alias == "-b") == 1 &&
                        option.AcceptedValues.Count == 3 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Foo") == 1 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Bar") == 1 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Baz") == 1);
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "qux" &&
                        option.Description == "Qux" &&
                        !option.IsHidden &&
                        !option.IsRequired &&
                        option.Aliases.Count == 1 &&
                        option.Aliases.Count(alias => alias == "--qux") == 1 &&
                        option.AcceptedValues.Count == 3 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Foo") == 1 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Bar") == 1 &&
                        option.AcceptedValues.Count(acceptedValue => acceptedValue == "Baz") == 1);
                Assert.Single(
                    commandHelp.Options,
                    option =>
                        option.Name == "hidden" &&
                        option.Description == "Hidden" &&
                        option.IsHidden &&
                        !option.IsRequired &&
                        option.Aliases.Count == 1 &&
                        option.Aliases.Count(alias => alias == "--hidden") == 1 &&
                        !option.AcceptedValues.Any());

                AssertGlobalOptionsEqual(commandHelp);
            }

            private static void AssertGlobalOptionsEqual(CommandHelp commandHelp)
            {
                Assert.Equal(1, commandHelp.GlobalOptions.Count);
                Assert.Single(
                    commandHelp.GlobalOptions,
                    globalOption =>
                        globalOption.Name == "help" &&
                        globalOption.Description == "Show help and usage information." &&
                        !globalOption.IsHidden &&
                        !globalOption.IsRequired &&
                        globalOption.Aliases.Count == 5 &&
                        globalOption.Aliases.Count(alias => alias == "-h") == 1 &&
                        globalOption.Aliases.Count(alias => alias == "/h") == 1 &&
                        globalOption.Aliases.Count(alias => alias == "--help") == 1 &&
                        globalOption.Aliases.Count(alias => alias == "-?") == 1 &&
                        globalOption.Aliases.Count(alias => alias == "/?") == 1 &&
                        !globalOption.AcceptedValues.Any());
            }
        }
    }
}
