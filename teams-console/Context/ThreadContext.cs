using teams_console.Commands;

namespace teams_console.Context
{
    public class ThreadContext : IContext
    {
        public ThreadContext()
        {
        }

        public string Name { get; }

        public void ExecuteCommand(ApplicationContext applicationContext, ICommand command)
        {
            /*switch (command)
            {
                case HelpCommand:
                    Help();
                    return true;
                case QuitCommand:
                    return false;
                case TextCommand text:
                    Text(text);
                    return true;
                // create a ReplyCommand for threads or a join thread?
                default:
                    Console.WriteLine("Command not supported.");
                    return true;
            }*/
        }

        private void Text(ApplicationContext applicationContext, TextCommand text)
        {
            //Console.WriteLine();
            // TODO: send the data (and don't write anything)
        }

        private void Help(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "Available commands");
            applicationContext.Application.Server.Write(Components.LogType.Info, " text                start a new thread in the channel");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /help               display this help");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /quit               exit the app");
        }
    }
}
