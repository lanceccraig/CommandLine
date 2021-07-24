using System.CommandLine;
using System.CommandLine.IO;
using System.Text;

namespace LanceC.CommandLine.Facts.Testing.Fakes
{
    public class FakeConsole : IConsole
    {
        private readonly StringBuilder _outBuilder = new();
        private readonly StringBuilder _errorBuilder = new();

        public FakeConsole()
        {
            Out = new FakeConsoleStandardStreamWriter(_outBuilder);
            Error = new FakeConsoleStandardStreamWriter(_errorBuilder);
        }

        public IStandardStreamWriter Out { get; }

        public bool IsOutputRedirected => true;

        public IStandardStreamWriter Error { get; }

        public bool IsErrorRedirected => true;

        public bool IsInputRedirected => true;

        public string OutResult => _outBuilder.ToString();

        public string ErrorResult => _errorBuilder.ToString();
    }

    public class FakeConsoleStandardStreamWriter : IStandardStreamWriter
    {
        private readonly StringBuilder _builder;

        public FakeConsoleStandardStreamWriter(StringBuilder builder)
        {
            _builder = builder;
        }

        public void Write(string value)
            => _builder.Append(value);
    }
}
