using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;

namespace LanceC.CommandLine
{
    /// <summary>
    /// Represents a command return code.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandCode : SmartEnum<CommandCode>
    {
        /// <summary>
        /// Specifies that the execution was successful.
        /// </summary>
        public static readonly CommandCode Success = new("Success", 0);

        /// <summary>
        /// Specifies that the execution resulted in an error.
        /// </summary>
        public static readonly CommandCode Error = new("Error", 1);

        private CommandCode(string name, int value)
            : base(name, value)
        {
        }
    }
}
