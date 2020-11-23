using System.Collections.Generic;
using teams_console.Components;

namespace teams_console.Renderer
{
    // How to cancel Console.Read() so we can update the screen without waiting for. Also, I could just reimplement key press
    // https://stackoverflow.com/questions/9479573/how-to-interrupt-console-readline

    // probably better to use the Console.KeyAvailable since it's non blocking
    // https://docs.microsoft.com/en-us/dotnet/api/system.console.keyavailable?view=net-5.0
    public class TextRenderer : IRenderer
    {
        private bool redraw = false;

        public void Render(IEnumerable<BaseComponent> components)
        {
            var renderContext = new RenderContext();
            var restoreCursor = false;

            foreach (var component in components)
            {
                if ((redraw || component.IsDirty) && component.IsVisible)
                {
                    component.Render(renderContext);
                    component.Reset();

                    restoreCursor = true;
                }
            }

            if (restoreCursor)
                renderContext.RestoreCursorPosition();

            redraw = false;
        }

        public void Redraw() => redraw = true;

        /*private TextBuffer ServerWindow { get; set; } = new TextBuffer(Console.WindowWidth);
        private TextBuffer TextWindow { get; set; } = new TextBuffer(Console.WindowWidth);
        private int currentCursorX = 0;
        private int currentCursorY = 0;

        [Flags]
        private enum DirtyFlags
        {
            None    = 0x0000,
            All     = 0xFFFF,

            Header  = 0x0001,
            Server  = 0x0002,
            Message = 0x0004,
            Input   = 0x0008,
            Help    = 0x00010
        }

        private DirtyFlags dirtyStatus = DirtyFlags.All;

        public void Render(ApplicationContext context)
        {
            if (dirtyStatus == DirtyFlags.None)
                return; // quick skip

            // TODO: always set the width (size could change)
            Console.CursorVisible = false;
            currentCursorX = Console.CursorLeft;
            currentCursorY = Console.CursorTop;

            if (dirtyStatus.HasFlag(DirtyFlags.Header))
                RenderHeader();

            if (dirtyStatus.HasFlag(DirtyFlags.Server))
                RenderServerLogs();

            if (dirtyStatus.HasFlag(DirtyFlags.Message))
                RenderMessages();

            if (dirtyStatus.HasFlag(DirtyFlags.Help))
                RenderHelp();

            if (dirtyStatus.HasFlag(DirtyFlags.Input))
                RenderInput(context); // must be the last one

            Console.CursorLeft = currentCursorX;
            Console.CursorTop = currentCursorY;
            Console.CursorVisible = true;
            dirtyStatus = DirtyFlags.None;
        }

        private void RenderHelp()
        {
            // Cannot use DrawText() because it will print the last character and make a new line.
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("^1 VIEW SPLIT   ^2 VIEW SVR     ^3 VIEW MSG");
        }

        private void RenderMessages()
        {
            var messageWindowTop = HeaderWindowHeight + ServerWindowHeight;
            DrawLine('-', messageWindowTop, " Windows Name ");

            var messageWindowBottom = Console.WindowHeight - HelpWindowHeight - InputWindowHeight - 2;
            DrawLine('-', messageWindowBottom, "");
        }

        public void RenderInput(ApplicationContext context)
        {
            // TODO: probably a invalide all vs a new letter?
            var top = Console.WindowHeight - HelpWindowHeight - InputWindowHeight - 1;
            for (int i = 0; i < InputWindowHeight; i++)
                DrawText(0, top++,  ""); // erase everything

            Console.SetCursorPosition(0, top - InputWindowHeight);
            Console.Write(context.GetPrompt());

            currentCursorX = Console.CursorLeft;
            currentCursorY = Console.CursorTop;
        }

        private void RenderServerLogs()
        {
            var recents = ServerWindow.Tail(ServerWindowHeight); // todo, might be have to add an offset as well, for pagination
            for (var i = 0; i < recents.Count; i++)
            {
                var recent = recents[recents.Count - i - 1];
                DrawText(0, HeaderWindowHeight + ServerWindowHeight - i - 1, recent);
            }
        }

        private void RenderHeader()
        {
            DrawLine('#', 0, "> TEAMS CONSOLE 0.1 <");
        }

        private void DrawText(int left, int top, string text)
        {
            Console.CursorTop = top;
            Console.CursorLeft = left;
            Console.Write(text);
            Console.Write(new string(' ', Console.WindowWidth - text.Length));
        }

        private void DrawLine(char character, int top = -1, string text = null)
        {
            Console.CursorLeft = 0;
            if (top >= 0)
                Console.CursorTop = top;

            if (string.IsNullOrEmpty(text))
            {
                Console.Write(new string(character, Console.WindowWidth));
            }
            else
            {
                var length = (Console.WindowWidth - text.Length) / 2;
                Console.Write(new string(character, length));
                Console.Write(text);
                Console.Write(new string(character, Console.WindowWidth - length - text.Length));
            }
        }

        public TextRenderer()
        {
            Console.Clear();
        }

        public void ServerNotice(string message)
        {
            ServerWindow.Add("- " + message);
            dirtyStatus |= DirtyFlags.Server;
        }

        public void ServerInfo(string message)
        {
            ServerWindow.Add("% " + message);
            dirtyStatus |= DirtyFlags.Server;
        }

        public void ServerError(string message)
        {
            ServerWindow.Add("! " + message);
            dirtyStatus |= DirtyFlags.Server;
        }

        private int HeaderWindowHeight => 1;
        private int ServerWindowHeight => 5;
        private int InputWindowHeight => 3;
        private int HelpWindowHeight => 1;*/
    }
}
