using System;
using System.Collections;
using System.Collections.Generic;

namespace teams_console.Components
{
    public class ApplicationView : IEnumerable<BaseComponent>
    {
        private List<BaseComponent> components = new List<BaseComponent>();
        private int width;
        private int height;
        private ViewType viewType = ViewType.SplitView;

        public ViewType ViewType 
        {
            get => viewType;
            set
            {
                if (viewType != value)
                {
                    viewType = value;
                    Resize(width, height);
                }
            }
        }

        public HeaderView Header { get; } = new HeaderView("> TEAMS CONSOLE 0.1 <");
        public ServerView Server { get; } = new ServerView();
        public MessageView Message { get; } = new MessageView();
        public InputView Input { get; } = new InputView();
        public HelpView Help { get; } = new HelpView();

        public BaseComponent Focus { get; set; }

        public ApplicationView(int width, int height)
        {
            Resize(width, height);

            components.Add(Header);
            components.Add(Server);
            components.Add(Message);
            components.Add(Help);
            components.Add(Input);

            Focus = Input;
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;

            switch (ViewType)
            {
                case ViewType.SplitView:
                    SetSplitView();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void SetSplitView()
        {
            // Header
            Header.Width = width;
            Header.Height = 1;
            Header.Left = 0;
            Header.Top = 0;

            // Server
            Server.Width = width;
            Server.Height = 6;
            Server.Left = 0;
            Server.Top = Header.Top + Header.Height;

            // Help
            Help.Width = width;
            Help.Height = 1;
            Help.Left = 0;
            Help.Top = height - Help.Height;

            // Input
            Input.Width = width;
            Input.Height = 4;
            Input.Left = 0;
            Input.Top = Help.Top - Input.Height;

            // Message (special dependency for height since it will fill)
            Message.Left = 0;
            Message.Top = Server.Top + Server.Height;
            Message.Width = width;
            Message.Height = Input.Top - Message.Top - 1; // TODO: not sure about the -1.
        }

        public void OnKeyPress(ConsoleKeyInfo key)
        {
            if (Focus != null)
                Focus.OnKeyPress(this, key);
        }

        public IEnumerator<BaseComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }
    }
}
