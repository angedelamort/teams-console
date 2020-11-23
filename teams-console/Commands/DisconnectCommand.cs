namespace teams_console.Commands
{
    [CommandAttribute(Description = "Disconnect from the server")]
    public class DisconnectCommand : ICommand
    {
        public DisconnectCommand() { }

        public bool IsValid() => true;
    }
}
