using System;
using System.CommandLine;
using System.CommandLine.Rendering;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Coloring
{
    /// <summary>
    /// Provides extensions for modifying output color in the console.
    /// </summary>
    public static class ConsoleColorExtensions
    {
        private static bool IsNoColorEnabled => Environment.GetEnvironmentVariable("NO_COLOR") is not null;

        /// <summary>
        /// Sets the foreground color of the console.
        /// </summary>
        /// <param name="console">The console to modify.</param>
        /// <param name="color">The color to set.</param>
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

        /// <summary>
        /// Sets the background color of the console.
        /// </summary>
        /// <param name="console">The console to modify.</param>
        /// <param name="color">The color to set.</param>
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

        /// <summary>
        /// Resets the colors of the console.
        /// </summary>
        /// <param name="console">The console to modify.</param>
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
    }
}
