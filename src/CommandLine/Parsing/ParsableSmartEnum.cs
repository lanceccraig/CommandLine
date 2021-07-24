using Ardalis.SmartEnum;

namespace LanceC.CommandLine.Parsing
{
    public class ParsableSmartEnum<TEnum> : IParsableSmartEnum<TEnum>
        where TEnum : SmartEnum<TEnum>
    {
        private readonly TEnum? _value;

        public ParsableSmartEnum(string name)
        {
            HasValue = SmartEnum<TEnum>.TryFromName(name, out _value);
            if (!HasValue)
            {
                ErrorMessage = $"'{name}' could not be parsed into an instance of {typeof(TEnum).Name}.";
            }
        }

        public bool HasValue { get; }

        public string ErrorMessage { get; } = string.Empty;

        public TEnum? Value => _value;
    }
}
