namespace teams_console.Commands
{
    [CommandAttribute(Description = "Connect to the specified server", Parameters = new[] {"server"})]
    public class ConnectCommand : ICommand
    {
        public ConnectCommand(string text)
        {
            Server = text;
        }

        public string Server { get; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Server);
        }
    }
}
