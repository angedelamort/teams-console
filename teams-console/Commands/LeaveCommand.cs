namespace teams_console.Commands
{
    [CommandAttribute(Description = "Leave a team or a channel")]
    public class LeaveCommand : ICommand
    {
        public bool IsValid()
        {
            return true;
        }
    }
}
