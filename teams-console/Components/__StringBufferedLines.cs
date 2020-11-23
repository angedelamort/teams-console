using System;
using System.Collections.Generic;
using teams_console.Utils;

namespace teams_console.Components
{
    // TODO: REMOVE ME!!
    public class __StringBufferedLines : ICloneable<__StringBufferedLines>
    {
        private string text = string.Empty;
        private int textCursorIndex = 0;
        private int cursorX = 0;
        private int cursorY = 0;
        //private int textSelectionStart;
        //private int textSelectionEnd;

        private object lockObject = new object();

        public __StringBufferedLines() { }

        private __StringBufferedLines(__StringBufferedLines inputTextBuffer)
        {
            text = inputTextBuffer.text;
            textCursorIndex = inputTextBuffer.textCursorIndex;
            //cursorX = inputTextBuffer.cursorX;
            //cursorY = inputTextBuffer.cursorY;
            //textSelectionStart = inputTextBuffer.textSelectionStart;
            //textSelectionEnd = inputTextBuffer.textSelectionEnd;
        }

        //public ReadOnlySpan<char> GetLineBuffer(int index) => new ReadOnlySpan<char>(lines[index]);

        public int TextCursorX
        {
            get { return cursorX; }
        }

        public int TextCursorY
        {
            get { return cursorY; }
        }

        // SOLUTION: 
        // - remove cursor stuff
        // - add a PositionAfterModification (text only)
        // - render is const and fill a RenderResult with 2 arrays (letters + position)
        // - The handler call PositionAfterModification and do the cursor movement
        // - Easier to handle selection and other operation on a constant array.
        // - Can create a function that transform LettersToPos and vice versa.



        public void MoveCharacterUp()
        {
            // redo the rendering? and set the character up?
            //if ()
            // x might be smaller
            // finding x might be hard
            // setting textCursorIndex might be challenging

            /*if (textCursorIndex > 0)
                textCursorIndex--;*/
        }
        /*
        public void MoveCharacterDown()
        {
            if (textCursorIndex > 0)
                textCursorIndex--;
        }

        public void MoveCharacterLeft()
        {
            lock (lockObject)
            {
                if (textCursorIndex > 0)
                    textCursorIndex--;
            }
        }

        public void MoveCharacterRight()
        {
            lock (lockObject)
            {
                if (textCursorIndex < text.Length)
                    textCursorIndex++;
            }
        }*/

        public override string ToString() => text;

        public void Insert(char character)
        {
            lock (lockObject)
            {
                text = text.Insert(textCursorIndex, character.ToString());
                textCursorIndex++;
            }
        }

        public void Clear()
        {
            lock (lockObject)
            {
                text = string.Empty;
                textCursorIndex = 0;
                //cursorX = 0;
                //cursorY = 0;
                //textSelectionStart = 0;
                //textSelectionEnd = 0;
            }
        }

        public void Insert(string str)
        {
            lock (lockObject)
            {
                text = text.Insert(textCursorIndex, str);
                textCursorIndex += str.Length;
            }
        }

        /// <summary>
        /// Delete 1 character (either backwards or right next to the cursor.
        /// </summary>
        /// <param name="isRight"></param>
        public bool Delete(bool isRight)
        {
            lock (lockObject)
            {
                if (isRight)
                {
                    if (text.Length > 0 && textCursorIndex < text.Length)
                    {
                        text = text.Remove(textCursorIndex, 1);
                        return true;
                    }
                }
                else
                {
                    if (textCursorIndex > 0)
                    {
                        text = text.Remove(--textCursorIndex, 1);
                        return true;
                    }
                }

                return false;
            }  
        }

        /*public int DeleteSelection()
        {
            lock (lockObject)
            {
                if (HasSelection())
                {
                    var start = Math.Min(textSelectionStart, textSelectionEnd);
                    var length = Math.Abs(textSelectionStart - textSelectionEnd);
                    text = text.Remove(start, length);
                    return start;
                }

                return -1;
            }
        }

        public bool HasSelection()
        {
            return textSelectionEnd != textSelectionStart;
        }*/

        public char[][] GetRenderedText(int width, int height)
        {
            var lines = new List<char[]> { new char[width] };
            var isWhiteSpaceEol = false;

            if (text.Length > 0)
            {
                var lineIndex = 0;
                var charIndex = 0;
                for (var i = 0; i < text.Length; i++)
                {
                    var c = text[i];
                    var line = lines[lineIndex];

                    if (c == '\r')
                    {
                        lineIndex++;
                        charIndex = 0;
                        var newLine = new char[width];
                        lines.Add(newLine);
                        isWhiteSpaceEol = false;
                    }
                    else if (charIndex < width)
                    {
                        line[charIndex] = c;
                        charIndex++;
                        isWhiteSpaceEol = false;
                    }
                    else
                    {
                        // ignore white space at the end of the line.
                        if (!char.IsWhiteSpace(c))
                        {
                            List<char> characters = new List<char>();
                            for (var j = line.Length - 1; j >= 0; j--)
                            {
                                if (char.IsWhiteSpace(line[j]))
                                    break;
                                characters.Add(line[j]);
                            }

                            var newLine = new char[width];
                            lineIndex++;

                            if (isWhiteSpaceEol || characters.Count == line.Length)
                            {
                                newLine[0] = c;
                                charIndex = 1;
                            }
                            else
                            {
                                characters.Reverse();
                                characters.Add(c);
                                Array.Clear(line, line.Length - characters.Count, characters.Count);
                                
                                characters.CopyTo(newLine);
                                charIndex = characters.Count;
                            }

                            isWhiteSpaceEol = false;
                            lines.Add(newLine);
                        }
                        else
                        {
                            isWhiteSpaceEol = true;
                        }
                    }

                    if (textCursorIndex == i + 1) // need to be in front of the char.
                    {
                        cursorX = charIndex;
                        cursorY = lineIndex;
                    }
                }

                Array.Clear(lines[lineIndex], charIndex, width - charIndex);
            }
            else
            {
                Array.Clear(lines[0], 0, width); // when deleting all the text, ensure the line is clear.
                cursorX = 0;
                cursorY = 0;
            }

            // TODO: update the windows based on the cursorY.

            // add missing lines to fill the buffer
            while (lines.Count < height)
                lines.Add(new char[width]);

            return lines.ToArray();
        }

        public __StringBufferedLines Clone()
        {
            lock (lockObject)
            {
                return new __StringBufferedLines(this);
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
 