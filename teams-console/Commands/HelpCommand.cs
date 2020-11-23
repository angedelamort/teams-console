namespace teams_console.Commands
{
    [CommandAttribute(Description = "Show this help")]
    public class HelpCommand : ICommand
    {
        public HelpCommand()
        {
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
