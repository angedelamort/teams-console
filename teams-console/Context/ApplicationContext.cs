using System.Diagnostics;
using teams_console.Commands;
using teams_console.Components;

namespace teams_console.Context
{
    public class ApplicationContext
    {
        private readonly GlobalContext globalContext;
        private ServerContext serverContext;
        private TeamContext teamContext;
        private ChannelContext channelContext;
        private ThreadContext threadContext;

        public ApplicationContext(GlobalContext globalContext, ApplicationView application)
        {
            this.globalContext = globalContext;
            Application = application;
            Debug.Assert(globalContext != null);
        }

        public ApplicationView Application { get; }

        // probably move to the InputView...
        public string GetPrompt()
        {
            if (threadContext != null)
                return $"{serverContext.Name}.{teamContext.Name}@{channelContext.Name}+{threadContext.Name}> ";

            if (channelContext != null)
                return $"{serverContext.Name}.{teamContext.Name}@{channelContext.Name}> ";

            if (teamContext != null)
                return $"{serverContext.Name}.{teamContext.Name}> ";

            if (serverContext != null)
                return $"{serverContext.Name}> ";

            return "> ";
        }

        public void ExecuteCommand(ICommand command)
        {
            GetActiveContext().ExecuteCommand(this, command);
        }

        private IContext GetActiveContext()
        {
            if (threadContext != null)
                return threadContext;

            if (channelContext != null)
                return channelContext;

            if (teamContext != null)
                return teamContext;

            if (serverContext != null)
                return serverContext;

            return globalContext;
        }

        public ServerContext ServerContext
        {
            get { return serverContext; }
            set
            {
                if (value != serverContext)
                {
                    Debug.Assert(globalContext != null);

                    serverContext = value;
                    TeamContext = null;
                }
            }
        }

        public TeamContext TeamContext
        {
            get { return teamContext; }
            set
            {
                if (value != teamContext)
                {
                    Debug.Assert(serverContext != null);
                    Debug.Assert(globalContext != null);

                    teamContext = value;
                    ChannelContext = null;
                }
            }
        }

        public ChannelContext ChannelContext
        {
            get { return channelContext; }
            set
            {
                if (value != channelContext)
                {
                    Debug.Assert(teamContext != null);
                    Debug.Assert(serverContext != null);
                    Debug.Assert(globalContext != null);

                    channelContext = value;
                    ThreadContext = null;
                }
            }
        }

        public ThreadContext ThreadContext { 
            get 
            { 
                return threadContext; 
            } 
            set
            {
                if (value != threadContext)
                {
                    Debug.Assert(channelContext != null);
                    Debug.Assert(teamContext != null);
                    Debug.Assert(serverContext != null);
                    Debug.Assert(globalContext != null);

                    threadContext = value;
                }
            } 
        }
    }
}
