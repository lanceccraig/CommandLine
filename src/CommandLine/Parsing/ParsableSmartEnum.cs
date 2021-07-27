using Ardalis.SmartEnum;

namespace LanceC.CommandLine.Parsing
{
    /// <summary>
    /// Represents a <see cref="SmartEnum{TEnum}"/> instance that can be parsed by name from the command line.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    public class ParsableSmartEnum<TEnum> : IParsableSmartEnum<TEnum>
        where TEnum : SmartEnum<TEnum>
    {
        private readonly TEnum? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsableSmartEnum{TEnum}"/> class.
        /// </summary>
        /// <param name="name">The enumeration name.</param>
        public ParsableSmartEnum(string name)
        {
            HasValue = SmartEnum<TEnum>.TryFromName(name, out _value);
            if (!HasValue)
            {
                ErrorMessage = $"'{name}' could not be parsed into an instance of {typeof(TEnum).Name}.";
            }
        }

        /// <summary>
        /// Gets the value that determines whether the enumeration could be parsed.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the error message if the enumeration could not be parsed. Otherwise, returns an empty string.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        /// Gets the enumeration value.
        /// </summary>
        public TEnum? Value => _value;
    }
}
