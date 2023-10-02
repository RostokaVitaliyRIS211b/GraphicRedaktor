

namespace RealizationOfApp.Creators
{
    public class GlobalEventHandler : IEventHandler
    {
        public event Action<object?, MouseMoveEventArgs>? OnMouseMoved;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonPressed;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonReleased;
        public event Action<object?, MouseWheelScrollEventArgs>? OnMouseWheelScrolled;
        public event Action<object?, KeyEventArgs>? OnKeyPressed;
        public event Action<object?, KeyEventArgs>? OnKeyReleased;
        public IConveir<Point> conveirPoint;
        public IConveir<Line> conveirLine;
        public GlobalEventHandler(IConveir<Point> conveir1, IConveir<Line> conveir2)
        {
            conveirPoint = conveir1;
            conveirLine = conveir2;
            OnKeyPressed+=GlobalEventHandler_OnKeyPressed;
            OnMouseButtonPressed+=GlobalEventHandler_OnMouseButtonPressed;
            OnMouseMoved+=GlobalEventHandler_OnMouseMoved;
        }

        protected void GlobalEventHandler_OnMouseMoved(object? source, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && source is Application app4)
            {
                EventDrawable? contains = (from elem in app4.eventDrawables
                                           where elem.Contains(e.X, e.Y)
                                           select elem).FirstOrDefault();
                if (contains is null)
                {
                    for (int i = 0; i<app4.eventDrawables.Count; ++i)
                    {
                        app4.eventDrawables[i].Move(e.X-app4.mousePosLast.X, e.Y-app4.mousePosLast.Y);
                    }
                    //Console.WriteLine($"{e.X-app4.mousePosLast.X},  {e.Y-app4.mousePosLast.Y}");
                }
            }
        }

        protected void GlobalEventHandler_OnMouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Middle && source is Application app2)
            {
                EventDrawable? obj = app2.eventDrawables.First(x => x.Contains(e.X, e.Y) && !x.IsNeedToRemove);
                if (obj is not null)
                    obj.IsNeedToRemove = true;
            }
            //Console.WriteLine((e.Button==Mouse.Button.Left)+" "+IsKeyPressed(Key.P)+" "+(source is Application));
        }

        protected void GlobalEventHandler_OnKeyPressed(object? source, KeyEventArgs e)
        {
            if (e.Code==Key.D && source is Application app)
            {

            }
        }

        public void MouseMoved(object? source, MouseMoveEventArgs e)
        {
            OnMouseMoved?.Invoke(source, e);
        }

        public void MouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            OnMouseButtonPressed?.Invoke(source, e);
        }

        public void MouseButtonReleased(object? source, MouseButtonEventArgs e)
        {
            OnMouseButtonReleased?.Invoke(source, e);
        }

        public void MouseWheelScrolled(object? source, MouseWheelScrollEventArgs e)
        {
            OnMouseWheelScrolled?.Invoke(source, e);
        }

        public void KeyPressed(object? source, KeyEventArgs e)
        {
            OnKeyPressed?.Invoke(source, e);
        }

        public void KeyReleased(object? source, KeyEventArgs e)
        {
            OnKeyReleased?.Invoke(source, e);
        }
    }
}
