namespace teams_console.Commands
{
    [CommandAttribute(Alias = "exit", Description = "Exit the application")]
    public class QuitCommand : ICommand
    {
        public bool IsValid()
        {
            return true;
        }
    }
}
