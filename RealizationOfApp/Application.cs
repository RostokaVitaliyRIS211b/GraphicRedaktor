
namespace RealizationOfApp
{
    public class Application
    {
        public List<EventDrawable> eventDrawables = new();
        public List<IEventHandler> eventHandlers = new();
        public uint CurrentWidth = 1280, CurrentHeight = 720;
        public Textbox messageBox = new();
        public View view = new();
        public RenderWindow window;
        public Application()
        {
            messageBox.SetColorText(Color.Black);
            messageBox.SetSizeCharacterText(16);
            messageBox.SetPos(CurrentWidth/2, CurrentHeight-30);
            messageBox.SetString("");
            GlobalEventHandler handler = new(new ConveirPointA(),new ConveirLineA());
            eventHandlers.Add(handler);
            //view.Reset(new FloatRect(0, 0, CurrentWidth, CurrentHeight));
            Grid grid = new(new Vector2f(CurrentWidth/2,CurrentHeight/2), CurrentWidth, CurrentHeight);
            eventDrawables.Add(grid);
            window = new RenderWindow(new VideoMode(CurrentWidth, CurrentHeight), "graphic redaktor");
            window.SetFramerateLimit(60);
            Subscribe();
            window.Closed+=Closed;
        }
        public void StartApp()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                DeleteObjects();
                //window.SetView(view);
                window.Clear(new(236, 253, 230));
                foreach (EventDrawable eventDrawable in eventDrawables)
                    window.Draw(eventDrawable);
                window.Draw(messageBox);
                window.Display();
            }
        }
        public void Subscribe()
        {
            window.MouseMoved+=MouseMoved;
            window.KeyPressed+=KeyPressed;
            window.KeyReleased+=KeyReleased;
            window.MouseButtonReleased+=MouseButtonReleased;
            window.MouseButtonPressed+=MouseButtonPressed;
            window.MouseWheelScrolled+=MouseWheelScrolled;
        }
        public void Unsubscribe()
        {
            window.MouseMoved-=MouseMoved;
            window.KeyPressed-=KeyPressed;
            window.KeyReleased-=KeyReleased;
            window.MouseButtonReleased-=MouseButtonReleased;
            window.MouseButtonPressed-=MouseButtonPressed;
            window.MouseWheelScrolled-=MouseWheelScrolled;
        }

        public void MouseMoved(object? source, MouseMoveEventArgs e)
        {
            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].MouseMoved(this, e);
            }
        }
        public void MouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            for (int i = 0; i< eventHandlers.Count; ++i)
            {
                eventHandlers[i].MouseButtonPressed(this, e);
            }
            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].MouseButtonPressed(this, e);
            }
        }
        public void MouseButtonReleased(object? source, MouseButtonEventArgs e)
        {
            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].MouseButtonReleased(this, e);
            }
        }
        public void KeyPressed(object? source, KeyEventArgs e)
        {
            for (int i = 0; i< eventHandlers.Count; ++i)
            {
                eventHandlers[i].KeyPressed(this, e);
            }
            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].KeyPressed(this, e);
            }
        }
        public void KeyReleased(object? source, KeyEventArgs e)
        {
            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].KeyReleased(this, e);
            }
        }
        public void MouseWheelScrolled(object? source, MouseWheelScrollEventArgs e)
        {

            for (int i = 0; i<eventDrawables.Count; ++i)
            {
                eventDrawables[i].MouseWheelScrolled(this, e);
            }
        }
        public void DeleteObjects()
        {
            eventDrawables.RemoveAll(x => x.IsNeedToRemove);
        }
        public void Closed(object? source, EventArgs e)
        {
            if (source is Window c)
            {
                c.Close();
            }
        }
    }
}