using teams_console.Commands;

namespace teams_console.Context
{
    public interface IContext
    {
        void ExecuteCommand(ApplicationContext applicationContext, ICommand command);
    }
}
