using System;
using System.CommandLine;
using System.Linq;
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
        private const string DefaultBreadcrumb = "testhost";

        public static Command Get(string breadcrumb)
            => Get(breadcrumb, new FakeRootCommandOne());

        public static Command GetWithoutGlobalOptions(string breadcrumb)
            => Get(breadcrumb, new FakeRootCommandTwo());

        public static Command GetWithHelpCommand()
            => Get(DefaultBreadcrumb, new FakeRootCommandThree());

        private static Command Get(string breadcrumb, Command rootCommand)
        {
            var breadcrumbParts = breadcrumb.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = rootCommand;
            for (var i = 1; i < breadcrumbParts.Length; i++)
            {
                var breadcrumbPart = breadcrumbParts[i];

                command = command.Children.OfType<Command>()
                    .Single(child => child.Name.Equals(breadcrumbPart, StringComparison.OrdinalIgnoreCase));
            }

            return command;
        }
    }

    public class FakeRootCommandOne : RootCommand
    {
        public FakeRootCommandOne()
            : base("Fake Executable")
        {
            AddCommand(new FakeSubGroupCommand());
            AddCommand(new FakeCommandOne());
            AddGlobalOption(new Option(new[] { "-h", "/h", "--help", "-?", "/?", }, "Show help and usage information."));
        }
    }

    public class FakeRootCommandTwo : RootCommand
    {
        public FakeRootCommandTwo()
            : base("Fake Executable")
        {
            AddCommand(new FakeSubGroupCommand());
        }
    }

    public class FakeRootCommandThree : RootCommand
    {
        public FakeRootCommandThree()
            : base("Fake Executable")
        {
            AddCommand(new FakeHelpCommand());
        }
    }

    public class FakeSubGroupCommand : Command
    {
        public FakeSubGroupCommand()
            : base("sg", "Fake SubGroup")
        {
            AddCommand(new FakeCommandTwo());
            AddCommand(new FakeCommandThree());
            AddCommand(new FakeCommandFour());
        }
    }

    public class FakeCommandOne : Command
    {
        public FakeCommandOne()
            : base("c1", "Fake Command One")
        {
            AddAlias("cone");

            AddArgument(new Argument<int>("foo", "Foo"));
            AddOption(
                new Option<string>(new[] { "--bar", "-b", }, "Bar")
                {
                    IsRequired = true,
                });
            AddOption(new Option<double>("--baz", "Baz"));
        }
    }

    public class FakeCommandTwo : Command
    {
        public FakeCommandTwo()
            : base("c2", "Fake Command Two")
        {
            AddArgument(new Argument<int>("foo", "Foo"));
            AddArgument(new Argument<bool?>("bar", "Bar"));
            AddArgument(new Argument<FakeEnum>("baz", "Baz"));
            AddArgument(new Argument<ParsableSmartEnum<FakeSmartEnum>>("qux", "Qux"));
            AddArgument(
                new Argument<string>("hidden", "Hidden")
                {
                    IsHidden = true,
                });
        }
    }

    public class FakeCommandThree : Command
    {
        public FakeCommandThree()
            : base("c3", "Fake Command Three")
        {
            AddOption(
                new Option<int>(new[] { "--foo", "-f" }, "Foo")
                {
                    IsRequired = true,
                });
            AddOption(new Option<bool>("--bar", "Bar"));
            AddOption(new Option<FakeEnum?>("-b", "Baz"));
            AddOption(new Option<ParsableSmartEnum<FakeSmartEnum>>("--qux", "Qux"));
            AddOption(
                new Option<double>("--hidden", "Hidden")
                {
                    IsHidden = true,
                });
        }
    }

    public class FakeCommandFour : Command
    {
        public FakeCommandFour()
            : base("c4", "Fake Command Four")
        {
            IsHidden = true;

            AddArgument(new Argument<int>("foo", "Foo"));
            AddOption(new Option<int>("--bar", "Bar"));
        }
    }

    public class FakeHelpCommand : Command
    {
        public FakeHelpCommand()
            : base("help", "Help")
        {
        }
    }
}
