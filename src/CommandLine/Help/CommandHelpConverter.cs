using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using Ardalis.SmartEnum;
using LanceC.CommandLine.Parsing;

namespace LanceC.CommandLine.Help
{
    internal static class CommandHelpConverter
    {
        public static CommandHelp Convert(ICommand command)
        {
            var subCommands = command.Children.OfType<Command>()
                .ToArray();

            var commandKind = GetCommandKind(subCommands);
            var breadcrumb = GetBreadcrumb(command);

            var subCommandHelps = new List<SubCommandHelp>();
            foreach (var subCommand in subCommands)
            {
                var subCommandHelp = new SubCommandHelp(
                    subCommand.Name,
                    subCommand.Description,
                    GetCommandKind(subCommand.Children.OfType<Command>()),
                    subCommand.IsHidden,
                    GetAliases(subCommand));
                subCommandHelps.Add(subCommandHelp);
            }

            var argumentHelps = new List<CommandArgumentHelp>();
            foreach (var argument in command.Children.OfType<Argument>())
            {
                var argumentHelp = new CommandArgumentHelp(
                    argument.Name,
                    argument.Description,
                    argument.IsHidden,
                    GetAcceptedValues(argument.ArgumentType));
                argumentHelps.Add(argumentHelp);
            }

            var globalOptionHelps = GetGlobalOptionHelps(command);

            var optionHelps = new List<CommandOptionHelp>();
            foreach (var option in command.Children.OfType<IOption>())
            {
                if (globalOptionHelps.Any(optionHelp => optionHelp.Name.Equals(option.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var optionHelp = Convert(option);
                optionHelps.Add(optionHelp);
            }

            var commandHelp = new CommandHelp(
                command.Name,
                command.Description,
                commandKind,
                command.IsHidden,
                breadcrumb,
                GetAliases(command),
                subCommandHelps,
                argumentHelps,
                optionHelps,
                globalOptionHelps);
            return commandHelp;
        }

        private static CommandOptionHelp Convert(IOption option)
            => new(
                option.Name,
                option.Description,
                option.IsHidden,
                option.IsRequired,
                option.Aliases,
                GetAcceptedValues(option.Argument.ValueType));

        private static CommandKind GetCommandKind(IEnumerable<Command> subCommands)
            => subCommands.Any() ? CommandKind.Group : CommandKind.Command;

        [SuppressMessage(
            "Performance",
            "CA1826:Do not use Enumerable methods on indexable collections",
            Justification = "There isn't a great alternative to the `FirstOrDefault` LINQ method.")]
        private static string GetBreadcrumb(ICommand command)
        {
            var stack = new Stack<string>();
            for (ISymbol? currentCommand = command;
                currentCommand != null;
                currentCommand = currentCommand.Parents.FirstOrDefault())
            {
                stack.Push(currentCommand.Name.ToLower());
            }

            var builder = new StringBuilder();
            while (stack.TryPop(out var part))
            {
                if (builder.Length != 0)
                {
                    builder.Append(' ');
                }

                builder.Append(part);
            }

            var breadcrumb = builder.ToString();
            return breadcrumb;
        }

        private static IReadOnlyCollection<string> GetAliases(ICommand command)
            => command.Aliases.Where(alias => !alias.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        private static IReadOnlyCollection<CommandOptionHelp> GetGlobalOptionHelps(ICommand command)
        {
            var rootCommand = command as Command;
            for (var currentCommand = rootCommand;
                currentCommand != null;
                currentCommand = currentCommand.Parents.OfType<Command>()
                    .FirstOrDefault())
            {
                rootCommand = currentCommand;
            }

            var optionHelps = new List<CommandOptionHelp>();
            foreach (var option in rootCommand!.GlobalOptions.OfType<IOption>())
            {
                var optionHelp = Convert(option);
                optionHelps.Add(optionHelp);
            }

            return optionHelps;
        }

        private static IReadOnlyCollection<string> GetAcceptedValues(Type type)
        {
            var innerNullableType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? type.GetGenericArguments()[0]
                : null;
            if (innerNullableType is not null)
            {
                return GetAcceptedValues(innerNullableType);
            }

            if (type.IsEnum)
            {
                return Enum.GetNames(type);
            }

            if (type == typeof(bool))
            {
                return new[] { "false", "true", };
            }

            var parsableEnumerationType = type.GetInterfaces()
                .FirstOrDefault(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IParsableSmartEnum<>));
            if (parsableEnumerationType is not null)
            {
                return GetAcceptedValuesForSmartEnum(parsableEnumerationType);
            }

            return Array.Empty<string>();
        }

        private static IReadOnlyCollection<string> GetAcceptedValuesForSmartEnum(Type parsableEnumerationType)
        {
            var enumerationType = parsableEnumerationType.GenericTypeArguments
                .Single(genericArgumentType =>
                    genericArgumentType.BaseType is not null &&
                    genericArgumentType.BaseType.GetGenericTypeDefinition() == typeof(SmartEnum<>));

            var listProperty = enumerationType
                .GetProperty("List", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var valueProperty = enumerationType.GetProperty("Value", typeof(int));

            var enumerationObjects = listProperty!.GetValue(null) as IEnumerable<object>;
            var enumerations = enumerationObjects!.Select(enumerationObject =>
                new
                {
                    Id = (int)valueProperty!.GetValue(enumerationObject)!,
                    Name = enumerationObject.ToString(),
                });

            return enumerations.OrderBy(enumeration => enumeration.Id)
                .Select(enumeration => enumeration.Name!)
                .ToArray();
        }
    }
}
