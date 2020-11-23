using System;
using teams_console.Renderer;

namespace teams_console.Components
{
    public abstract class BaseComponent
    {
        private int top;
        private int left;
        private int width;
        private int height;
        private bool visible = true;

        public int Top
        {
            get => top;
            set
            {
                top = value;
                IsDirty = true;
            }
        }

        public int Left
        {
            get => left;
            set
            {
                left = value;
                IsDirty = true;
            }
        }

        public int Width
        {
            get => width;
            set
            {
                width = value;
                IsDirty = true;
                OnSizeChange(width, height); // TODO: not sure it's good to do that on every set, but good for now.
            }
        }

        public int Height
        {
            get => height;
            set
            {
                height = value;
                IsDirty = true;
                OnSizeChange(width, height);
            }
        }

        public bool IsVisible
        {
            get => visible;
            set
            {
                visible = value;
                IsDirty = true;
            }
        }

        public bool IsDirty { get; private set; } = true;

        public void Invalidate() { IsDirty = true; }
        public void Reset() { IsDirty = false; }

        public abstract void Render(RenderContext context);

        public virtual void OnKeyPress(ApplicationView applicationViews, ConsoleKeyInfo key) {}

        // TODO: maybe use events instead.
        protected virtual void OnSizeChange(int width, int height) { }
    }
}
