namespace teams_console.Commands
{
    [CommandAttribute(Description = "List available resources")]
    public class ListCommand : ICommand
    {
        // receive parameters like list teams ou list channels teams
        public bool IsValid()
        {
            return true;
        }
    }
}
