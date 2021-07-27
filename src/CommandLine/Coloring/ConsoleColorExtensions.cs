using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.CommandLine.Rendering;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Coloring
{
    public static class ConsoleColorExtensions
    {
        private static bool IsNoColorEnabled => Environment.GetEnvironmentVariable("NO_COLOR") is not null;

        public static void SetForegroundColor(this IConsole console, ConsoleColor color)
        {
            Guard.Against.Null(console, nameof(console));

            if (IsNoColorEnabled)
            {
                return;
            }

            if (console is ITerminal terminal)
            {
                terminal.ForegroundColor = color;
                return;
            }

            try
            {
                Console.ForegroundColor = color;
            }
            catch (PlatformNotSupportedException)
            {
            }
        }

        public static void SetBackgroundColor(this IConsole console, ConsoleColor color)
        {
            Guard.Against.Null(console, nameof(console));

            if (IsNoColorEnabled)
            {
                return;
            }

            if (console is ITerminal terminal)
            {
                terminal.BackgroundColor = color;
                return;
            }

            try
            {
                Console.BackgroundColor = color;
            }
            catch (PlatformNotSupportedException)
            {
            }
        }

        public static void ResetColor(this IConsole console)
        {
            Guard.Against.Null(console, nameof(console));

            if (IsNoColorEnabled)
            {
                return;
            }

            if (console is ITerminal terminal)
            {
                terminal.ResetColor();
                return;
            }

            try
            {
                Console.ResetColor();
            }
            catch (PlatformNotSupportedException)
            {
            }
        }

        public static void Write(this IConsole console, ConsoleColor color, string value)
        {
            Guard.Against.Null(console, nameof(console));

            console.SetForegroundColor(color);
            console.Out.Write(value);
            console.ResetColor();
        }

        public static void WriteLine(this IConsole console, ConsoleColor color, string value)
        {
            Guard.Against.Null(console, nameof(console));

            console.SetForegroundColor(color);
            console.Out.WriteLine(value);
            console.ResetColor();
        }
    }
}
