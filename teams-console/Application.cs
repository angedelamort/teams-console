using System;
using System.Threading;
using System.Threading.Tasks;
using teams_console.Commands;
using teams_console.Components;
using teams_console.Context;
using teams_console.Renderer;

namespace teams_console
{
    public class Application
    {
        private IRenderer renderer;
        private ApplicationView application;
        private ApplicationContext context;
        private volatile bool quit = false;

        public Application()
        {
            // TODO: make an event OnWindowResize
            application = new ApplicationView(Console.WindowWidth, Console.WindowHeight);
            renderer = new TextRenderer();
            context = new ApplicationContext(new GlobalContext(), application);

            //application.Input.Text = context.GetPrompt();
        }

        // TODO: tab -> should auto complete.

        public void Run()
        {
            application.Server.Write(LogType.Notice, "Welcome to Teams - console mode");

            /*var quit = false;

            while (!quit)
            {
                renderer.Render(context);

                // TODO: call the Render
                //Console.Write(context.GetPrompt());

                // todo: probably go character by character since we want to auto-complete.
                // also can implement paste on multi line or shift+enter
                var line = Console.ReadLine(); 
                var command = CommandFactory.Create(line);

                if (command.IsValid())
                    context.ExecuteCommand(command);
                else
                    renderer.ServerError("Command doesn't exists. Type /help for available commands.");

                quit = command is QuitCommand;
            }*/
            Task.WaitAll(
                Task.Run(() => InputLoop()),
                Task.Run(() => RenderingLoop()),
                Task.Run(() => ServerLoop())
                );

            application.Server.Write(LogType.Notice, "Thanks for using Teams Console!");
        }

        private void InputLoop()
        {
            application.Input.OnSendCommand += Input_OnSendCommand;
            // TODO: probably better to handle that in another file.
            // TODO: handle multiple lines edit and nav
            //var line = ""; // change for a string builder? or probably just send 1 character at the time to the renderer...
            while (!quit)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    application.OnKeyPress(key);

                    /*
                    if (key.Key == ConsoleKey.Enter)
                    {
                        var command = CommandFactory.Create(line);

                        if (command.IsValid())
                            context.ExecuteCommand(command);
                        else
                            application.Server.Write(LogType.Error, "Command doesn't exists. Type /help for available commands.");

                        quit = command is QuitCommand;
                    }
                    else
                    {
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Control) || key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                        {
                            // TODO
                        }
                        else
                        {

                        }
                        // TODO: render the key press async!
                        // TODO: hadle the different modifiers and keys. hack for now.
                        line += key.KeyChar;
                    }*/
                }

                Thread.Sleep(1);
            }
        }

        private void Input_OnSendCommand(string commandText)
        {
            var command = CommandFactory.Create(commandText);

            if (command.IsValid())
                context.ExecuteCommand(command);
            else
                application.Server.Write(LogType.Error, "Command doesn't exists. Type /help for available commands.");

            quit = command is QuitCommand;
        }

        private void RenderingLoop()
        {
            // TODO: when dirty, re-render what needs to be done.
            // TODO: make a boolean to stop processing keys since we are re-rendering (would mix up the cursor)
            while (!quit)
            {
                renderer.Render(application);
                Thread.Sleep(100);
            }
        }

        private void ServerLoop()
        {
            // TODO: make and receive server calls.
            while (!quit)
            {
                Thread.Sleep(100);
            }
        }
    }
}
