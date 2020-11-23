using System;
using System.Collections.Generic;
using System.Drawing;
using teams_console.Renderer;

namespace teams_console.Components
{
    // tests: TODO: Unit tests! or else it didn't happen.
    // 1. continuous line
    //  1.1. should go to next line
    //  1.2. backspace should go back to line 1
    // 2. word before end
    //  2.1 word wrap if we continue typing
    //  2.2 word doesn't wrap with extra space and wrap on new character.

    // todo array keys
    // TODO: 
    // * implement the selection
    // * implement the copy
    public class InputView : BaseComponent
    {
        // 6- add an auto-complete
        private List<string> previousCommands = new List<string>(); // todo: maybe add a maximum.

        //char[][] previousLines = null;
        //StringBufferedLines inputTextBuffer = new StringBufferedLines();
        private string Text { get => text; set { text = value; Invalidate(); } }
        private int TextCursorPosition { get => textCursorPosition; set { textCursorPosition = value; Invalidate(); } }
        private StringLineBuffer previousLineBuffer;
        private WindowBuffer previousWindowBuffer;

        private Dictionary<ConsoleKeyInfo, Action<ApplicationView>> keyMapping;
        private string text = string.Empty;
        private int textCursorPosition;

        static InputView()
        {
            // cancel the ctrl+c
            Console.CancelKeyPress += (s, e) =>
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                    e.Cancel = true;
            };
        }

        public delegate void KeyEventHandler(string command);

        public event KeyEventHandler OnSendCommand;

        public override void Render(RenderContext context)
        {
            var lineBuffer = new StringLineBuffer(Text, Width);
            var windowBuffer = new WindowBuffer(lineBuffer, new Size(Width, Height), lineBuffer.GetCoordFromTextPosition(TextCursorPosition));

            var buffer = windowBuffer.GetBuffer();
            var previousBuffer = previousWindowBuffer != null ? previousWindowBuffer.GetBuffer() : null;

            for (int i = 0; i < buffer.Length; i++)
            {
                var line = buffer[i];
                var previousLine = previousBuffer != null ? previousBuffer[i] : null;
                for (var j = 0; j < line.Length; j++)
                {
                    if (previousLine == null || line[j] != previousLine[j])
                        context.DrawCharacter(Left + j, Top + i, line[j] == 0 ? ' ' : line[j]);
                }
            }

            context.SaveCursorPosition(windowBuffer.CursorPosition.X + Left, windowBuffer.CursorPosition.Y + Top);
            previousLineBuffer = lineBuffer;
            previousWindowBuffer = windowBuffer;
        }

        public override void OnKeyPress(ApplicationView applicationViews, ConsoleKeyInfo keyInfo)
        {
            if (keyMapping.TryGetValue(keyInfo, out var value))
            {
                value(applicationViews);
                Invalidate();
            }
            else if (!keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt) &&
                !char.IsControl(keyInfo.KeyChar))
            {
                var str = keyInfo.KeyChar.ToString();
                Text = Text.Insert(TextCursorPosition, str);
                TextCursorPosition += str.Length;
            }
        }

        public InputView()
        {
            keyMapping = new Dictionary<ConsoleKeyInfo, Action<ApplicationView>>
            {
                // Enter / new line
                { new ConsoleKeyInfo((char)ConsoleKey.Enter, ConsoleKey.Enter, false, false, false), v => SendMessage(v) },
                { new ConsoleKeyInfo((char)ConsoleKey.Enter, ConsoleKey.Enter, true, false, false), v => NewLine(v) },
                { new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, true), v => NewLine(v) },

                // Deletion
                { new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), v => Delete(v, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false), v => Delete(v, true) },

                // Movement using cursor
                { new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false), v => Move(v, -1, 0, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, true), v => MoveWord(v, -1, 0, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, true, false, false), v => Move(v, -1, 0, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, true, false, true), v => MoveWord(v, -1, 0, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false), v => Move(v, 1, 0, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, true), v => MoveWord(v, 1, 0, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, true, false, false), v => Move(v, 1, 0, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, true, false, true), v => MoveWord(v, 1, 0, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false), v => Move(v, 0, -1, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, true), v => MoveWord(v, 0, -1, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, false, false), v => Move(v, 0, -1, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, false, true), v => MoveWord(v, 0, -1, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false), v => Move(v, 0, 1, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, true), v => MoveWord(v, 0, 1, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, true, false, false), v => Move(v, 0, 1, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, true, false, true), v => MoveWord(v, 0, 1, true) },

                { new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false), v => StartOfLine(v, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, true), v => StartOfText(v, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.Home, true, false, false), v => StartOfLine(v, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.Home, true, false, true), v => StartOfText(v, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false), v => EndOfLine(v, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, true), v => EndOfText(v, false) },
                { new ConsoleKeyInfo('\0', ConsoleKey.End, true, false, false), v => EndOfLine(v, true) },
                { new ConsoleKeyInfo('\0', ConsoleKey.End, true, false, true), v => EndOfText(v, true) },

                { new ConsoleKeyInfo('\0', ConsoleKey.PageUp, false, false, false), v => PreviousPage(v) },
                { new ConsoleKeyInfo('\0', ConsoleKey.PageDown, false, false, false), v => NextPage(v) },
                { new ConsoleKeyInfo('\0', ConsoleKey.PageDown, false, false, true), v => LastPage(v) },

                // Copy/Paste
                { new ConsoleKeyInfo((char)ConsoleKey.C, ConsoleKey.C, false, false, true), v => Copy(v) },
                { new ConsoleKeyInfo((char)ConsoleKey.X, ConsoleKey.X, false, false, true), v => Cut(v) },
                // seems to be in the console...
                //{ new ConsoleKeyInfo((char)ConsoleKey.V, ConsoleKey.V, false, false, true), v => Paste(v) }
            };
        }

        private void Delete(ApplicationView v, bool isRight)
        {
            if (text.Length > 0)
            {
                if (isRight)
                {
                    if (TextCursorPosition < Text.Length)
                        Text = Text.Remove(TextCursorPosition, 1);
                }
                else
                {
                    if (TextCursorPosition > 0)
                        Text = Text.Remove(--TextCursorPosition, 1);
                }
            }
        }

        private void Paste(ApplicationView v)
        {
        }

        private void Cut(ApplicationView v)
        {
        }

        private void Copy(ApplicationView v)
        {
        }

        private void LastPage(ApplicationView v)
        {
        }

        private void NextPage(ApplicationView v)
        {
        }

        private void PreviousPage(ApplicationView v)
        {
        }

        private void StartOfText(ApplicationView view, bool isSelection)
        {
        }

        private void EndOfText(ApplicationView view, bool isSelection)
        {
        }

        private void EndOfLine(ApplicationView view, bool isSelection)
        {
        }

        private void StartOfLine(ApplicationView view, bool isSelection)
        {
        }

        private void NewLine(ApplicationView v)
        {
            Text += '\r';
        }

        private void MoveWord(ApplicationView view, int x, int y, bool isSelection)
        {

        }

        private void Move(ApplicationView view, int x, int y, bool isSelection)
        {
            if (x != 0)
            {
                /*if (x < 0)
                    inputTextBuffer.MoveCharacterLeft();
                else
                    inputTextBuffer.MoveCharacterRight();*/
            }
        }

        private void SendMessage(ApplicationView applicationView)
        {
            previousCommands.Add(Text);
            OnSendCommand?.Invoke(Text);
            Text = string.Empty;
        }
    }
}
