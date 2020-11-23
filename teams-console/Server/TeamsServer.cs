using System;
using teams_console.Context;

namespace teams_console.Server
{
    public class TeamsServer : IServer
    {
        public TeamsServer(ApplicationContext context)
        {
            Context = context;
        }

        private ApplicationContext Context { get; }

        public void Send(string message)
        {
            //Context.Renderer.Client("todo");
            // TODO: call the renderer when needed
        }
    }
}
