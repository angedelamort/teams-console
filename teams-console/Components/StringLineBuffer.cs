using System;
using System.Collections.Generic;
using System.Drawing;
using teams_console.Utils;

namespace teams_console.Components
{
    /// <summary>
    /// A read-only string text buffer that will wrap the text automatically using words.
    /// Also provide a mapping table for the current position of each character in the text.
    /// </summary>
    public class StringLineBuffer : ICloneable<StringLineBuffer> //(iterable??)
    {
        private readonly List<char[]> textItems = new List<char[]>();
        private List<int[]> positionItems = new List<int[]>();
        private Point[] characterPositionToCoord;

        public StringLineBuffer(string text, int width)
        {
            Width = width;
            Text = text;
            characterPositionToCoord = new Point[(text != null ? text.Length : 0) + 1]; // need one more for the cursor "hello" has 5 letters and 6 cursor postions

            Update();
        }

        /// <summary>
        /// Get the width of the LineBuffer
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Get the text in the buffer
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Get the text in the buffer.
        /// </summary>
        /// <returns>Get the Text</returns>
        public override string ToString()
        {
            return Text;
        }

        public int GetTextPositionFromCoord(int left, int top)
        {
            if (top >= textItems.Count)
                throw new ArgumentOutOfRangeException(nameof(top));
            if (left >= Width)
                throw new ArgumentOutOfRangeException(nameof(left));

            var textLine = textItems[top];
            var posLine = positionItems[top];
            while (left > 0)
            {
                if (textLine[left] != 0)
                    return posLine[left];
                left--;
            }

            return -1;
        }

        public Point GetCoordFromTextPosition(int characterIndex)
        {
            return characterPositionToCoord[characterIndex];
        }

        public char[] GetTextLineBuffer(int index)
        {
            return textItems[index];
        }

        public ReadOnlySpan<int> GetPositionLineBuffer(int index)
        {
            return positionItems[index];
        }

        public int LineCount => textItems.Count;

        public StringLineBuffer Clone()
        {
            return new StringLineBuffer(Text, Width);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        private void AddNewLine()
        {
            textItems.Add(new char[Width]);
            positionItems.Add(new int[Width]);
        }

        private void Update()
        {
            var isWhiteSpaceEol = false;
            AddNewLine();

            if (!string.IsNullOrEmpty(Text))
            {
                var lineIndex = 0;
                var charIndex = 0;
                int i = 0;
                for (i = 0; i < Text.Length; i++)
                {
                    var c = Text[i];
                    var line = textItems[lineIndex];
                    var pos = positionItems[lineIndex];

                    if (c == '\r')
                    {
                        characterPositionToCoord[i] = new Point(charIndex, lineIndex);
                        AddNewLine();
                        lineIndex++;
                        charIndex = 0;
                        isWhiteSpaceEol = false;
                    }
                    else if (charIndex < Width)
                    {
                        characterPositionToCoord[i] = new Point(charIndex, lineIndex);
                        line[charIndex] = c;
                        pos[charIndex] = i;
                        charIndex++;
                        isWhiteSpaceEol = false;
                    }
                    else
                    {
                        // ignore white space at the end of the line.
                        if (!char.IsWhiteSpace(c))
                        {
                            List<char> extraCharacters = new List<char>();
                            List<int> extraPositions = new List<int>();
                            for (var j = Width - 1; j >= 0; j--)
                            {
                                if (char.IsWhiteSpace(line[j]))
                                    break;
                                extraCharacters.Add(line[j]);
                                extraPositions.Add(pos[j]);
                            }

                            AddNewLine();

                            if (isWhiteSpaceEol || extraCharacters.Count == line.Length)
                            {
                                characterPositionToCoord[i] = new Point(0, lineIndex + 1);
                                textItems[lineIndex][0] = c;
                                positionItems[lineIndex][0] = i;
                                charIndex = 1;
                            }
                            else
                            {
                                extraCharacters.Reverse();
                                extraCharacters.Add(c);
                                extraPositions.Reverse();
                                extraPositions.Add(i);

                                Array.Clear(line, line.Length - extraCharacters.Count, extraCharacters.Count);
                                Array.Clear(pos, line.Length - extraCharacters.Count, extraCharacters.Count);

                                var k = 0;
                                for (var j = Width - extraCharacters.Count; j < Width; j++)
                                    characterPositionToCoord[i - extraCharacters.Count] = new Point(k++, lineIndex);

                                extraCharacters.CopyTo(textItems[lineIndex]);
                                extraPositions.CopyTo(positionItems[lineIndex]);
                                charIndex = extraCharacters.Count;
                            }

                            lineIndex++;
                            isWhiteSpaceEol = false;
                        }
                        else
                        {
                            characterPositionToCoord[i] = new Point(charIndex, lineIndex);
                            isWhiteSpaceEol = true;
                        }
                    }
                }

                // CHECK : 
                // 1. a new line index should always have a character
                // 2. a character position should always be a (width - 1)
                characterPositionToCoord[i] = new Point(charIndex, lineIndex);
            }
            else
            {
                characterPositionToCoord[0] = new Point(0, 0);
            }
        }
    }
}
