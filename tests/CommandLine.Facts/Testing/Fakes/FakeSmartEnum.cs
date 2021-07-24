using Ardalis.SmartEnum;

namespace LanceC.CommandLine.Facts.Testing.Fakes
{
    public class FakeSmartEnum : SmartEnum<FakeSmartEnum>
    {
        public static readonly FakeSmartEnum Foo = new("Foo", 1);

        public static readonly FakeSmartEnum Baz = new("Baz", 3);

        public static readonly FakeSmartEnum Bar = new("Bar", 2);

        private FakeSmartEnum(string name, int value)
            : base(name, value)
        {
        }
    }
}
