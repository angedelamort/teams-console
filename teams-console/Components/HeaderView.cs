using teams_console.Renderer;

namespace teams_console.Components
{
    public class HeaderView : BaseComponent
    {
        private string title;
        private char pen = '#';
        // HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Center;

        public HeaderView(string title)
        {
            Title = title;
        }

        public string Title { 
            get => title; 
            set 
            { 
                title = value;
                Invalidate();
            } 
        }

        public char Pen
        {
            get => pen;
            set
            {
                pen = value;
                Invalidate();
            }
        }

        public override void Render(RenderContext context)
        {
            if (string.IsNullOrEmpty(Title))
            {
                context.DrawHorizontalLine(Left, Top, Width, Height, pen);
            }
            else
            {
                var length = (Width - title.Length) / 2;

                context.DrawHorizontalLine(Left, Top, length, Height, pen);
                context.DrawText(Left + length, Top, Title);
                context.DrawHorizontalLine(Left + length + Title.Length, Top, Width - length - Title.Length, Height, pen);
            }
        }
    }
}
