using System;

namespace teams_console.Renderer
{
    // TODO: when writing text, should call DrawLine if we want to clear the data.
    public class RenderContext
    {
        public RenderContext()
        {
            SaveCursorPosition();
        }

        public void DrawHorizontalLine(int left, int top, int width, int height, char character)
        {
            Console.CursorTop = top;
            for (var i = 0; i < height; i++)
            {
                Console.CursorLeft = left;
                Console.Write(new string(character, width));
                top++;
            }
            
        }

        public void DrawHeader(int top, int width, char character, string text)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = top;

            if (string.IsNullOrEmpty(text))
            {
                Console.Write(new string(character, width));
            }
            else
            {
                var length = (Console.WindowWidth - text.Length) / 2;
                Console.Write(new string(character, length));
                Console.Write(text);
                Console.Write(new string(character, width - length - text.Length));
            }
        }

        public void DrawText(int left, int top, string text)
        {
            Console.CursorTop = top;
            Console.CursorLeft = left;
            Console.Write(text);
        }

        public void DrawCharacter(int left, int top, char c)
        {
            Console.CursorTop = top;
            Console.CursorLeft = left;
            Console.Write(c);
        }

        public void SaveCursorPosition()
        {
            CursorLeft = Console.CursorLeft;
            CursorTop = Console.CursorTop;
        }

        public void SaveCursorPosition(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }

        public void RestoreCursorPosition()
        {
            Console.CursorLeft = Math.Min(CursorLeft, Console.WindowWidth - 1);
            Console.CursorTop = CursorTop;
        }

        public int MaxWidth => Console.WindowWidth;

        public int CursorLeft { get; private set; }
        public int CursorTop { get; private set; }
    }
}
