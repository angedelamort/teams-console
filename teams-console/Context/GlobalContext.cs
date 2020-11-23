using teams_console.Commands;

namespace teams_console.Context
{
    public class GlobalContext : IContext
    {
        // TODO: would be cool to have aliases as well.
        public void ExecuteCommand(ApplicationContext applicationContext, ICommand command)
        {
            switch (command)
            {
                case HelpCommand:
                    Help(applicationContext);
                    break;
                case QuitCommand:
                    break;
                case ConnectCommand connect:
                    Connect(applicationContext, connect);
                    break;
                default:
                    applicationContext.Application.Server.Write(Components.LogType.Error, "Command not supported.");
                    break;
            }
        }

        private void Connect(ApplicationContext applicationContext, ConnectCommand connect)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "connecting to " + connect.Server);

            applicationContext.ServerContext = new ServerContext(connect.Server);

            applicationContext.Application.Server.Write(Components.LogType.Info, "connected!");
        }

        private void Help(ApplicationContext applicationContext)
        {
            CommandFactory.RenderHelp(applicationContext.Application, new[] {
                typeof(ConnectCommand),
                typeof(QuitCommand),
                typeof(HelpCommand)
            });
            //applicationContext.Renderer.ServerInfo("Available commands");
            //applicationContext.Renderer.ServerInfo(" /help               display this help");
            //applicationContext.Renderer.ServerInfo(" /connect {server}   connect to the Azure server using your credentials");
            //applicationContext.Renderer.ServerInfo(" /quit               exit the app");
        }
    }
}
