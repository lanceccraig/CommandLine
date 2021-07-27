using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.IO;
using System.Linq;
using System.Text;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Help
{
    /// <summary>
    /// Provides help text generation builder for a command tree.
    /// </summary>
    public class CommandTreeHelpBuilder : IHelpBuilder
    {
        private const string RequiredTag = "[Required]";
        private const string AcceptedValuesLabel = "Accepted values";
        private const string AliasesLabel = "Aliases";

        private readonly IConsole _console;
        private readonly CommandTreeHelpBuilderOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTreeHelpBuilder"/> class.
        /// </summary>
        /// <param name="console">The console to write help text to.</param>
        /// <param name="options">The help text generation options.</param>
        public CommandTreeHelpBuilder(IConsole console, CommandTreeHelpBuilderOptions? options = default)
        {
            _console = console;
            _options = options ?? new CommandTreeHelpBuilderOptions();
        }

        private string Indent => new(' ', _options.IndentLength);

        /// <summary>
        /// Writes the help text for a command.
        /// </summary>
        /// <param name="command">The command to generate help text for.</param>
        public void Write(ICommand command)
        {
            Guard.Against.Null(command, nameof(command));

            var commandHelp = CommandHelpConverter.Convert(command);
            if (commandHelp.IsHidden)
            {
                return;
            }

            WriteSynopsis(commandHelp);
            WriteGroupBody(commandHelp);
            WriteCommandBody(commandHelp);
        }

        private static string GetFullDescription(string? description, string valuesLabel, IReadOnlyCollection<string> values)
        {
            var builder = new StringBuilder(description);
            if (values.Any())
            {
                builder.AppendLine()
                    .Append(valuesLabel)
                    .Append(": ")
                    .Append(string.Join(", ", values));
            }

            var rawDescription = builder.ToString();
            return rawDescription;
        }

        private void WriteSynopsis(CommandHelp commandHelp)
        {
            var format = GetFormat(commandHelp.Breadcrumb.Length);
            var descriptionFormatter = new IndentedDescriptionFormatter(commandHelp.Breadcrumb.Length, _options);

            var fullDescription = GetFullDescription(commandHelp.Description, AliasesLabel, commandHelp.Aliases);
            var formattedDescription = descriptionFormatter.Format(fullDescription);
            var message = string.Format(format, commandHelp.Breadcrumb, formattedDescription);

            _console.Out.WriteLine();
            _console.Out.WriteLine(commandHelp.Kind.Name);
            _console.Out.WriteLine(message);
            _console.Out.WriteLine();
        }

        private void WriteGroupBody(CommandHelp commandHelp)
        {
            if (commandHelp.Kind != CommandKind.Group)
            {
                return;
            }

            var subGroups = commandHelp.SubCommands.Where(subCommand => !subCommand.IsHidden)
                .Where(subCommand => subCommand.Kind == CommandKind.Group)
                .ToArray();
            var subCommands = commandHelp.SubCommands.Where(subCommand => !subCommand.IsHidden)
                .Where(subCommand => subCommand.Kind == CommandKind.Command)
                .ToArray();
            var globalOptions = commandHelp.GlobalOptions.Where(globalOption => !globalOption.IsHidden)
                .ToArray();

            var maximumColumnWidths = new[]
            {
                subGroups.Any() ? subGroups.Max(subGroup => subGroup.Name.Length) : 0,
                subCommands.Any() ? subCommands.Max(subCommand => subCommand.Name.Length) : 0,
                globalOptions.Any() ? globalOptions.Max(globalOption => globalOption.DisplayName.Length) : 0,
            };
            var firstColumnWidth = maximumColumnWidths.Max();

            var format = GetFormat(firstColumnWidth);
            var descriptionFormatter = new IndentedDescriptionFormatter(firstColumnWidth, _options);

            if (subGroups.Any())
            {
                _console.Out.WriteLine("Sub Groups:");
                foreach (var subGroup in subGroups)
                {
                    var formattedDescription = descriptionFormatter.Format(subGroup.Description);
                    var message = string.Format(format, subGroup.Name, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }

            if (subCommands.Any())
            {
                _console.Out.WriteLine("Commands:");
                foreach (var subCommand in subCommands)
                {
                    var formattedDescription = descriptionFormatter.Format(subCommand.Description);
                    var message = string.Format(format, subCommand.Name, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }

            if (globalOptions.Any())
            {
                _console.Out.WriteLine("Global Options:");
                foreach (var globalOption in globalOptions)
                {
                    var fullDescription = GetFullDescription(
                        globalOption.Description,
                        AcceptedValuesLabel,
                        globalOption.AcceptedValues);
                    var formattedDescription = descriptionFormatter.Format(fullDescription);
                    var message = string.Format(format, globalOption.DisplayName, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }
        }

        private void WriteCommandBody(CommandHelp commandHelp)
        {
            if (commandHelp.Kind != CommandKind.Command)
            {
                return;
            }

            var arguments = commandHelp.Arguments.Where(argument => !argument.IsHidden)
                .ToArray();
            var options = commandHelp.Options.Where(option => !option.IsHidden)
                .ToArray();
            var globalOptions = commandHelp.GlobalOptions.Where(globalOption => !globalOption.IsHidden)
                .ToArray();

            var hasRequiredOptions = options.Any(option => option.IsRequired);

            var maximumColumnWidths = new[]
            {
                arguments.Any() ? arguments.Max(argument => argument.DisplayName.Length) : 0,
                options.Any() ? options.Max(option => option.DisplayName.Length) : 0,
                globalOptions.Any() ? globalOptions.Max(globalOption => globalOption.DisplayName.Length) : 0,
            };
            var firstColumnWidth = maximumColumnWidths.Max();
            var secondColumnWidth = hasRequiredOptions ? RequiredTag.Length : 0;

            var format = GetFormat(firstColumnWidth, secondColumnWidth);
            var additionalIndentLength = hasRequiredOptions ? 1 : 0;
            var descriptionFormatter = new IndentedDescriptionFormatter(
                firstColumnWidth + additionalIndentLength + secondColumnWidth,
                _options);

            if (arguments.Any())
            {
                _console.Out.WriteLine("Arguments:");
                foreach (var argument in arguments)
                {
                    var fullDescription = GetFullDescription(argument.Description, AcceptedValuesLabel, argument.AcceptedValues);
                    var formattedDescription = descriptionFormatter.Format(fullDescription);
                    var message = string.Format(format, argument.DisplayName, string.Empty, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }

            if (options.Any())
            {
                _console.Out.WriteLine("Options:");
                foreach (var option in options)
                {
                    var fullDescription = GetFullDescription(option.Description, AcceptedValuesLabel, option.AcceptedValues);
                    var formattedDescription = descriptionFormatter.Format(fullDescription);
                    var tags = option.IsRequired ? RequiredTag : string.Empty;
                    var message = string.Format(format, option.DisplayName, tags, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }

            if (globalOptions.Any())
            {
                _console.Out.WriteLine("Global Options:");
                foreach (var globalOption in globalOptions)
                {
                    var fullDescription = GetFullDescription(
                        globalOption.Description,
                        AcceptedValuesLabel,
                        globalOption.AcceptedValues);
                    var formattedDescription = descriptionFormatter.Format(fullDescription);
                    var message = string.Format(format, globalOption.DisplayName, string.Empty, formattedDescription);
                    _console.Out.WriteLine(message);
                }

                _console.Out.WriteLine();
            }
        }

        private string GetFormat(params int[] columnWidths)
        {
            var firstColumnWidth = columnWidths[0];
            var formatBuilder = new StringBuilder($"{Indent}{{0, -{firstColumnWidth}}}");

            int index;
            for (index = 1; index < columnWidths.Length; index++)
            {
                var columnWidth = columnWidths[index];
                if (columnWidth == 0)
                {
                    formatBuilder.Append($"{{{index}}}");
                }
                else
                {
                    formatBuilder.Append($" {{{index}, {columnWidth}}}");
                }
            }

            formatBuilder.Append($"{{{index}}}");

            var format = formatBuilder.ToString();
            return format;
        }
    }
}
