using System;
using teams_console.Commands;

namespace teams_console.Context
{
    public class TeamContext : IContext
    {
        public TeamContext(string teamName)
        {
            Name = teamName;
        }

        public string Name { get; }

        public void ExecuteCommand(ApplicationContext applicationContext, ICommand command)
        {
            switch (command)
            {
                case HelpCommand:
                    Help(applicationContext);
                    break;
                case QuitCommand:
                    break;
                case ListCommand:
                    List(applicationContext);
                    break;
                case LeaveCommand:
                    Leave(applicationContext);
                    break;
                case JoinCommand join:
                    Join(applicationContext, join);
                    break;
                default:
                    applicationContext.Application.Server.Write(Components.LogType.Error, "Command not supported.");
                    break;
            }
        }

        private void Leave(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "leaving team " + Name);
            applicationContext.TeamContext = null;
        }

        private void Join(ApplicationContext applicationContext, JoinCommand join)
        {
            applicationContext.ChannelContext = new ChannelContext(join.Text);
            applicationContext.Application.Server.Write(Components.LogType.Info, "joining team " + join.Text);
        }

        private void List(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "Channels:");
            applicationContext.Application.Server.Write(Components.LogType.Info, " 1. General");
            applicationContext.Application.Server.Write(Components.LogType.Info, " 2. Random");
            applicationContext.Application.Server.Write(Components.LogType.Info, "");
        }

        private void Help(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "Available commands");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /list               list all channels");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /join               join a channel");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /leave              leave the current team");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /help               display this help");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /quit               exit the app");
        }
    }
}
