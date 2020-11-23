using System;
using System.Collections.Generic;

namespace teams_console.Renderer
{
    public class TextBuffer
    {
        private List<string> originalText = new List<string>();
        private List<string> wrapedText = new List<string>();

        public delegate void OnChangeHandler();
        public event OnChangeHandler OnTextChange;

        public TextBuffer(int width = 200)
        {
            Width = width;
        }

        public int Width { get; private set; }

        public void Add(string text)
        {
            originalText.Add(text);

            wrapedText.AddRange(WrapText(text));

            OnTextChange?.Invoke();
        }

        public List<string> Tail(int lines)
        {
            var items = new List<string>();
            var start = Math.Max(wrapedText.Count - lines, 0);
            var end = wrapedText.Count;

            for (int i = start; i < end; i++)
                items.Add(wrapedText[i]);

            return items;
        }

        public void ChangeWidth(int width)
        {
            if (Width != width)
            {
                Width = width;
                wrapedText.Clear();
                foreach (var line in originalText)
                    Add(line);
            }
        }

        private IEnumerable<string> WrapText(string text)
        {
            var lines = new List<string>();
            foreach (var line in text.Split('\n'))
            {
                if (line.Length < Width)
                    lines.Add(line);
                else
                    lines.AddRange(LineSplit(line));
            }

            return lines;
        }

        private IEnumerable<string> LineSplit(string line)
        {
            var lines = new List<string>();
            line = line.Replace("\t", "    ").Trim();

            while (line.Length > 0)
            {
                var max = Math.Min(Width, line.Length);
                var index = line.LastIndexOf(' ', max);
                if (index == -1)
                    index = max;

                lines.Add(line.Substring(0, index).Trim());

                line = line.Substring(index).Trim();
            }

            return lines;
        }
    }
}
