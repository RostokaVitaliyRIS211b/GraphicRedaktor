

namespace RealizationOfApp.Creators
{
    public class GlobalEventHandler:IEventHandler
    {
        public event Action<object?, MouseMoveEventArgs>? OnMouseMoved;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonPressed;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonReleased;
        public event Action<object?, MouseWheelScrollEventArgs>? OnMouseWheelScrolled;
        public event Action<object?, KeyEventArgs>? OnKeyPressed;
        public event Action<object?, KeyEventArgs>? OnKeyReleased;

        public GlobalEventHandler()
        {
            OnKeyPressed+=GlobalEventHandler_OnKeyPressed;
            OnMouseButtonPressed+=GlobalEventHandler_OnMouseButtonPressed;
        }

        private void GlobalEventHandler_OnMouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            if(e.Button==Mouse.Button.Left && IsKeyPressed(Key.P) && source is Application app)
            {
                Point p = new(e.X, e.Y, "");
                ConveirPointA conveir = new();
                app.eventDrawables.Add(p);
                EnterTextPoint enterText = new(new Vector2f(e.X, e.Y), p);
                enterText.IsAlive = true;
                conveir.ProcessObj(p);
                app.eventDrawables.Add(enterText);
                app.messageBox.SetString("Введите имя точки");
            }
            //Console.WriteLine((e.Button==Mouse.Button.Left)+" "+IsKeyPressed(Key.P)+" "+(source is Application));
        }

        protected void GlobalEventHandler_OnKeyPressed(object? source, KeyEventArgs e)
        {
            
        }

        public  void MouseMoved(object? source, MouseMoveEventArgs e)
        {
            OnMouseMoved?.Invoke(source, e);
        }

        public  void MouseButtonPressed(object? source,
            MouseButtonEventArgs e)
        {
            OnMouseButtonPressed?.Invoke(source, e);
        }

        public  void MouseButtonReleased(object? source,
            MouseButtonEventArgs e)
        {
            OnMouseButtonReleased?.Invoke(source, e);
        }

        public  void MouseWheelScrolled(object? source,
            MouseWheelScrollEventArgs e)
        {
            OnMouseWheelScrolled?.Invoke(source, e);
        }

        public  void KeyPressed(object? source, KeyEventArgs e)
        {
            OnKeyPressed?.Invoke(source, e);
        }

        public  void KeyReleased(object? source, KeyEventArgs e)
        {
            OnKeyReleased?.Invoke(source, e);
        }
    }
}
