using System;
using teams_console.Commands;

namespace teams_console.Context
{
    public class ChannelContext : IContext
    {
        public ChannelContext(string channelName)
        {
            Name = channelName;
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
                case LeaveCommand:
                    Leave(applicationContext);
                    break;
                case TextCommand text:
                    Text(applicationContext, text);
                    break;
                // create a ReplyCommand for threads or a join thread?
                default:
                    applicationContext.Application.Server.Write(Components.LogType.Error, "Command not supported.");
                    break;
            }
        }

        private void Leave(ApplicationContext applicationContext)
        {
            applicationContext.Application.Server.Write(Components.LogType.Info, "leaving channel " + Name);
            applicationContext.ChannelContext = null;
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
            applicationContext.Application.Server.Write(Components.LogType.Info, " /leave              leave the current channel");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /help               display this help");
            applicationContext.Application.Server.Write(Components.LogType.Info, " /quit               exit the app");
        }
    }
}
