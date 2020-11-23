using System;
using teams_console.Commands;

namespace teams_console.Context
{
    public class ServerContext : IContext
    {
        public string Name { get; }

        public ServerContext(string name)
        {
            Name = name;
        }

        public void ExecuteCommand(ApplicationContext applicationContext, ICommand command)
        {
            switch (command)
            {
                case HelpCommand:
                    Help(applicationContext);
                    break;
                case ListCommand:
                    List(applicationContext);
                    break;
                case JoinCommand join:
                    Join(applicationContext, join);
                    break;
                case QuitCommand:
                    break;
                case DisconnectCommand:
                    Disconnect(applicationContext);
                    break;
                default:
                    applicationContext.Application.Server.Write(Components.LogType.Error, "Command not supported.");
                    break;
            }
        }

        private void Join(ApplicationContext applicationContext, JoinCommand join)
        {
            applicationContext.TeamContext = new TeamContext(join.Text);
            applicationContext.Application.Server.Write(Components.LogType.Info, "joining team " + join.Text);
        }

        private void List(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "Teams:");
            applicationContext.Application.Server.Write(Components.LogType.Info, "  1. IT");
            applicationContext.Application.Server.Write(Components.LogType.Info, "  2. Fun");
            applicationContext.Application.Server.Write(Components.LogType.Info, "");
        }

        private void Disconnect(ApplicationContext applicationContext)
        {
            applicationContext.ServerContext = null;
            applicationContext.Application.Server.Write(Components.LogType.Info, $"Disconnected from {Name}");
        }

        private void Help(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "Available commands");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /list               List all available teams");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /join               join a Team");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /disconnect         disconnect from the server");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /help               display this help");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /quit               exit the app");
        }
    }
}
