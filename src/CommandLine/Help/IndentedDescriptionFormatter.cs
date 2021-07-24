using System;
using System.Linq;
using System.Text;
using Ardalis.GuardClauses;

namespace LanceC.CommandLine.Help
{
    internal class IndentedDescriptionFormatter
    {
        private readonly int _firstColumnWidth;
        private readonly string _firstLineSeparator;
        private readonly int _maxLineLength;

        private readonly int _indentSize;
        private readonly string _paddedLine;
        private bool _usedSeparator;

        public IndentedDescriptionFormatter(int firstColumnWidth, CustomHelpBuilderOptions options)
        {
            Guard.Against.Null(options, nameof(options));

            _firstColumnWidth = options.IndentLength + firstColumnWidth;
            _firstLineSeparator = options.DescriptionSeparator;
            _maxLineLength = options.MaximumWidth;

            _indentSize = _firstColumnWidth + _firstLineSeparator.Length;
            _paddedLine = Environment.NewLine + new string(' ', _indentSize);
        }

        public string Format(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var lines = input
                .Split(new[] { "\n", "\r\n" }, StringSplitOptions.None)
                .Select(WrapInput)
                .ToArray();

            _usedSeparator = false;

            var result = string.Join(_paddedLine, lines);
            return result;
        }

        private string WrapInput(string original)
        {
            var builder = new StringBuilder();

            if (!_usedSeparator)
            {
                builder.Append(_firstLineSeparator);
                _usedSeparator = true;
            }

            var lineLength = _indentSize;

            foreach (var token in original.Split(new[] { ' ', }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (lineLength == _indentSize)
                {
                }
                else if (lineLength + 1 + token.Length > _maxLineLength)
                {
                    builder.Append(_paddedLine);
                    lineLength = _indentSize;
                }
                else
                {
                    builder.Append(' ');
                    lineLength++;
                }

                builder.Append(token);
                lineLength += token.Length;
            }

            return builder.ToString();
        }
    }
}
