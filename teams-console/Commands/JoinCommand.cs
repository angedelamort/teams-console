namespace teams_console.Commands
{
    [CommandAttribute(Description = "Join a team or a channel", Parameters = new[] { "id" })]
    public class JoinCommand : ICommand
    {
        public JoinCommand(string text)
        {
            Text = text;
        }

        public string Text { get; }

        // receive parameters like list teams ou list channels teams
        public bool IsValid()
        {
            return true;
        }
    }
}
