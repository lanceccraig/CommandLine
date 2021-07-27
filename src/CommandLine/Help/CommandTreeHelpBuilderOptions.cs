namespace LanceC.CommandLine.Help
{
    /// <summary>
    /// Provides help text generation builder options for a command tree.
    /// </summary>
    public class CommandTreeHelpBuilderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTreeHelpBuilderOptions"/> class.
        /// </summary>
        /// <param name="descriptionSeparator">The separator between the name and description of an element.</param>
        /// <param name="indentLength">The number of spaces to indent sections by.</param>
        /// <param name="maximumWidth">The maximum width to write to the console.</param>
        public CommandTreeHelpBuilderOptions(
            string descriptionSeparator = " : ",
            int indentLength = 4,
            int maximumWidth = 100)
        {
            DescriptionSeparator = descriptionSeparator;
            IndentLength = indentLength;
            MaximumWidth = maximumWidth;
        }

        /// <summary>
        /// Gets the separator between the name and description of an element.
        /// </summary>
        public string DescriptionSeparator { get; }

        /// <summary>
        /// Gets the number of spaces to indent sections by.
        /// </summary>
        public int IndentLength { get; }

        /// <summary>
        /// Gets the maximum width to write to the console.
        /// </summary>
        public int MaximumWidth { get; }
    }
}
