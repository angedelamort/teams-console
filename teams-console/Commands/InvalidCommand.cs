namespace teams_console.Commands
{
    public class InvalidCommand : ICommand
    {
        public bool IsValid()
        {
            return false;
        }
    }
}
