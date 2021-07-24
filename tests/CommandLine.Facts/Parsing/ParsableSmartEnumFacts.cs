using LanceC.CommandLine.Facts.Testing.Fakes;
using LanceC.CommandLine.Parsing;
using Xunit;

namespace LanceC.CommandLine.Facts.Parsing
{
    public class ParsableSmartEnumFacts
    {
        public class TheConstructor : ParsableSmartEnumFacts
        {
            [Fact]
            public void ParsesEnumerationFromName()
            {
                // Arrange
                var expectedValue = FakeSmartEnum.Foo;

                // Act
                var parsableSmartEnum = new ParsableSmartEnum<FakeSmartEnum>(expectedValue.Name);

                // Assert
                Assert.True(parsableSmartEnum.HasValue);
                Assert.Equal(string.Empty, parsableSmartEnum.ErrorMessage);
                Assert.Equal(expectedValue, parsableSmartEnum.Value);
            }

            [Fact]
            public void DoesNotParseEnumerationForUnrecognizedName()
            {
                // Arrange
                var name = "Invalid";

                // Act
                var parsableSmartEnum = new ParsableSmartEnum<FakeSmartEnum>(name);

                // Assert
                Assert.False(parsableSmartEnum.HasValue);
                Assert.NotEqual(string.Empty, parsableSmartEnum.ErrorMessage);
                Assert.Null(parsableSmartEnum.Value);
            }
        }
    }
}
