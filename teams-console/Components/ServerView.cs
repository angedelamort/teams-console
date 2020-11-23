using System;
using teams_console.Renderer;

namespace teams_console.Components
{
    public class ServerView : BaseComponent
    {
        public ServerView()
        {
            Text = new TextBuffer(Width);
            Text.OnTextChange += Text_OnTextChange;
        }

        private void Text_OnTextChange()
        {
            Invalidate();
        }

        public TextBuffer Text { get; }

        public override void Render(RenderContext context)
        {
            var recents = Text.Tail(Height);

            for (var i = 0; i < recents.Count; i++)
            {
                var recent = recents[recents.Count - i - 1];
                var top = Top + Height - i - 1;
                context.DrawText(Left, top, recent);
                context.DrawHorizontalLine(recent.Length, top, Width - Left - recent.Length, 1, ' ');
            }
        }

        public void Write(LogType type, string message)
        {
            switch (type)
            {
                case LogType.Info:
                    Text.Add("% " + message);
                    break;
                case LogType.Notice:
                    Text.Add("- " + message);
                    break;
                case LogType.Error:
                    Text.Add("!! " + message);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnSizeChange(int width, int height)
        {
            Text.ChangeWidth(width);
        }
    }
}
