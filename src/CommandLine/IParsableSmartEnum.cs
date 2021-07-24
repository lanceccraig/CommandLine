using Ardalis.SmartEnum;

namespace LanceC.CommandLine
{
    /// <summary>
    /// Defines an enumeration value that can be parsed from the command line.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    public interface IParsableSmartEnum<TEnum>
        where TEnum : SmartEnum<TEnum>
    {
        /// <summary>
        /// Gets the value that determines whether the enumeration was parsed successfully.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gets the error message for a parsing failure. If parsing succeeded, returns an empty string.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets the parsed enumeration value.
        /// </summary>
        TEnum? Value { get; }
    }
}
