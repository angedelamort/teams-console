using System;
using System.Drawing;

namespace teams_console.Components
{
    public class WindowBuffer
    {
        private readonly char[][] windowBuffer;

        public WindowBuffer(StringLineBuffer stringLineBuffer, Size size, Point cursorPosition)
        {
            if (stringLineBuffer.Width != size.Width)
                throw new NotImplementedException();

            Size = size;

            windowBuffer = new char[Size.Height][];
            InitWindowBuffer(stringLineBuffer, cursorPosition);
        }

        public Size Size { get; }

        public char[][] GetBuffer()
        {
            return windowBuffer;
        }

        public Point CursorPosition { get; private set; }

        private void InitWindowBuffer(StringLineBuffer stringLineBuffer, Point cursorPosition)
        {
            var windowLeft = 0; // TODO: change first index if we implement width.
            var windowTop = Math.Max(0, cursorPosition.Y - Size.Height);

            CursorPosition = new Point(
                cursorPosition.X, // TODO: change first index if we implement width.
                cursorPosition.Y - windowTop);

            for (var i = 0; i < windowBuffer.Length; i++)
            {
                windowBuffer[i] = new char[Size.Width];

                if (windowTop < stringLineBuffer.LineCount)
                {
                    var lineBuffer = stringLineBuffer.GetTextLineBuffer(windowTop++);
                    Array.Copy(lineBuffer, windowLeft, windowBuffer[i], 0, Size.Width); 
                }
            }
        }
    }
}
