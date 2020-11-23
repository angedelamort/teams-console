namespace teams_console.Commands
{
    public class TextCommand : ICommand
    {
        public TextCommand(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public bool IsValid()
        {
            return true;
        }
    }
}
