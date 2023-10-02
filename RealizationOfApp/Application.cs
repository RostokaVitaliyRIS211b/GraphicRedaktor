
using System.Net.Http.Headers;

namespace RealizationOfApp
{
    public class Application
    {
        public List<EventDrawable> eventDrawables = new();
        public List<IEventHandler> eventHandlers = new();
        public uint CurrentWidth = 1280, CurrentHeight = 720;
        public Textbox messageBox = new();
        RectangleShape field;
        //public View view = new();
        public RenderWindow window;
        public Vector2i mousePosLast;
        public Application()
        {
            messageBox.SetColorText(Color.Black);
            messageBox.SetSizeCharacterText(16);
            messageBox.SetPos(100, 30);
            messageBox.SetString("Режим:");
            GlobalEventHandler handler = new(new ConveirPointA(),new ConveirLineA());
            eventHandlers.Add(handler);
            //view.Reset(new FloatRect(0, 0, CurrentWidth, CurrentHeight));
            Grid grid = new(new Vector2f((CurrentWidth+200)/2,CurrentHeight/2), CurrentWidth, CurrentHeight,200);
            eventDrawables.Add(grid);
            field = new();
            field.FillColor=new(236, 253, 230);
            field.Size = new Vector2f(200, CurrentHeight);
            field.Position = new(0,0);
            field.OutlineColor = Color.Black;
            field.OutlineThickness = 2;
            window = new RenderWindow(new VideoMode(CurrentWidth, CurrentHeight), "Graphic redaktor");
            window.SetFramerateLimit(60);
            eventDrawables.Add(new GUI(new GUIFactoryA()));
            Subscribe();
            window.Closed+=Closed;
        }
        public void StartApp()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                DeleteObjects();
                window.Clear(new(236, 253, 230));
                foreach (EventDrawable eventDrawable in eventDrawables.Where(x=>x is not GUI))
                    window.Draw(eventDrawable);
                window.Draw(field);
                window.Draw(eventDrawables.Where(x => x is GUI).First());
                window.Draw(messageBox);
                mousePosLast = Mouse.GetPosition(window);
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
            for (int i = 0; i< eventHandlers.Count; ++i)
            {
                eventHandlers[i].MouseMoved(this, e);
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