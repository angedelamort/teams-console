using System.Collections.Generic;
using teams_console.Components;

namespace teams_console.Renderer
{
    public interface IRenderer
    {
        void Render(IEnumerable<BaseComponent> components);
        void Redraw();
    }
}
