using System.Text;
using LanceC.CommandLine.Help;
using Xunit;

namespace LanceC.CommandLine.Facts.Help
{
    public class IndentedDescriptionFormatterFacts
    {
        private static IndentedDescriptionFormatter CreateSystemUnderTest() => new(1, new CommandTreeHelpBuilderOptions());

        public class TheFormatMethod : IndentedDescriptionFormatterFacts
        {
            [Fact]
            public void ReturnsEmptyStringWhenInputIsNull()
            {
                // Arrange
                var input = default(string?);
                var sut = CreateSystemUnderTest();

                // Act
                var actual = sut.Format(input);

                // Assert
                Assert.Equal(string.Empty, actual);
            }

            [Fact]
            public void ReturnsEmptyStringWhenInputIsEmpty()
            {
                // Arrange
                var input = string.Empty;
                var sut = CreateSystemUnderTest();

                // Act
                var actual = sut.Format(input);

                // Assert
                Assert.Equal(string.Empty, actual);
            }

            [Fact]
            public void WrapsInputAtMaximumLengthOnSpaces()
            {
                // Arrange
                var input = new StringBuilder("Lorem ipsum dolor sit amet, ea persius recteque disputando mea, id doctus omittantur ")
                    .Append("quo, in duis blandit sea. Pro cu eripuit qualisque referrentur, no virtute habemus scribentur sea. Sed ")
                    .Append("ei exerci ceteros, per ferri concludaturque in. Duo ea oratio scriptorem, cu eum mollis convenire. Ei ")
                    .Append("est possit scripta accusam.")
                    .ToString();

                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine(" : Lorem ipsum dolor sit amet, ea persius recteque disputando mea, id doctus omittantur quo, in")
                    .AppendLine("        duis blandit sea. Pro cu eripuit qualisque referrentur, no virtute habemus scribentur sea.")
                    .AppendLine("        Sed ei exerci ceteros, per ferri concludaturque in. Duo ea oratio scriptorem, cu eum mollis")
                    .Append("        convenire. Ei est possit scripta accusam.")
                    .ToString();

                // Act
                var actual = sut.Format(input);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void PreservesExistingLineBreaks()
            {
                // Arrange
                var input = "Foo\r\nBar\nBaz";

                var sut = CreateSystemUnderTest();

                var expected = new StringBuilder()
                    .AppendLine(" : Foo")
                    .AppendLine("        Bar")
                    .Append("        Baz")
                    .ToString();

                // Act
                var actual = sut.Format(input);

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}
