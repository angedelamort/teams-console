using teams_console.Renderer;

namespace teams_console.Components
{
    public class HelpView : BaseComponent
    {
        public override void Render(RenderContext context)
        {
            context.DrawText(Left, Top, "^1 VIEW SPLIT   ^2 VIEW SVR     ^3 VIEW MSG");
        }
    }
}
