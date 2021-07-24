using System;
using System.CommandLine;
using System.Linq;
using Ardalis.SmartEnum;
using LanceC.CommandLine.Parsing;

namespace LanceC.CommandLine.Facts.Testing.Fakes
{
    public enum FakeEnum
    {
        Foo,
        Bar,
        Baz,
    }

    public static class FakeCommands
    {
        public static Command Get(string breadcrumb)
        {
            var breadcrumbParts = breadcrumb.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = (Command)FakeRootCommand.Instance;
            for (var i = 1; i < breadcrumbParts.Length; i++)
            {
                var breadcrumbPart = breadcrumbParts[i];

                command = command.Children.OfType<Command>()
                    .Single(child => child.Name.Equals(breadcrumbPart, StringComparison.OrdinalIgnoreCase));
            }

            return command;
        }
    }

    // Command instances must be properties (rather than static readonly fields) to allow for parallel test execution.
    public static class FakeRootCommand
    {
        public static RootCommand Instance
        {
            get
            {
                var command = new RootCommand("Fake Executable")
                    {
                        FakeSubGroupCommand.Instance,
                        FakeCommandOne.Instance,
                    };

                command.AddGlobalOption(
                    new Option(new[] { "-h", "/h", "--help", "-?", "/?", }, "Show help and usage information."));

                return command;
            }
        }
    }

    public static class FakeSubGroupCommand
    {
        public static Command Instance
            => new("sg", "Fake SubGroup")
            {
                FakeCommandTwo.Instance,
                FakeCommandThree.Instance,
                FakeCommandFour.Instance,
            };
    }

    public static class FakeCommandOne
    {
        public static Command Instance
        {
            get
            {
                var command = new Command("c1", "Fake Command One")
                {
                    new Argument<int>("foo", "Foo"),
                    new Option<string>(new[] { "--bar", "-b", }, "Bar")
                    {
                        IsRequired = true,
                    },
                    new Option<double>(new[] { "--baz", }, "Baz"),
                };

                command.AddAlias("cone");

                return command;
            }
        }
    }

    public static class FakeCommandTwo
    {
        public static Command Instance
            => new("c2", "Fake Command Two")
            {
                new Argument<int>("foo", "Foo"),
                new Argument<bool>("bar", "Bar"),
                new Argument<FakeEnum>("baz", "Baz"),
                new Argument<FakeParsableSmartEnum>("qux", "Qux"),
                new Argument<string>("hidden", "Hidden")
                {
                    IsHidden = true,
                },
            };
    }

    public static class FakeCommandThree
    {
        public static Command Instance
            => new("c3", "Fake Command Three")
            {
                new Option<int>(new[] { "--foo", "-f" }, "Foo")
                {
                    IsRequired = true,
                },
                new Option<bool>(new[] { "--bar", }, "Bar"),
                new Option<FakeEnum>(new[] { "-b", }, "Baz"),
                new Option<FakeParsableSmartEnum>(new[] { "--qux", }, "Qux"),
                new Option<double>(new[] { "--hidden", }, "Hidden")
                {
                    IsHidden = true,
                },
            };
    }

    public static class FakeCommandFour
    {
        public static Command Instance
        {
            get
            {
                var command = new Command("c4", "Fake Command Four")
                {
                    new Argument<int>("foo", "Foo"),
                    new Option<int>(new[] { "--bar", }, "Bar"),
                };

                command.IsHidden = true;

                return command;
            }
        }
    }

    public class FakeParsableSmartEnum : ParsableSmartEnum<FakeSmartEnum>
    {
        public FakeParsableSmartEnum(string name)
            : base(name)
        {
        }
    }

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
