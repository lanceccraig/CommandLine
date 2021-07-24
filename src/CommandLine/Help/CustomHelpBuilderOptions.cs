namespace LanceC.CommandLine.Help
{
    public class CustomHelpBuilderOptions
    {
        public CustomHelpBuilderOptions(
            string descriptionSeparator = " : ",
            int indentLength = 4,
            int maximumWidth = 100)
        {
            DescriptionSeparator = descriptionSeparator;
            IndentLength = indentLength;
            MaximumWidth = maximumWidth;
        }

        public string DescriptionSeparator { get; }

        public int IndentLength { get; }

        public int MaximumWidth { get; }
    }
}
