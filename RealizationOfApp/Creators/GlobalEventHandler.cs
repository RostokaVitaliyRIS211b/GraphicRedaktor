

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
        IConveir<Point> conveirPoint;
        IConveir<Line> conveirLine;

        public GlobalEventHandler(IConveir<Point> conveir1, IConveir<Line> conveir2)
        {
            conveirPoint = conveir1;
            conveirLine = conveir2;
            OnKeyPressed+=GlobalEventHandler_OnKeyPressed;
            OnMouseButtonPressed+=GlobalEventHandler_OnMouseButtonPressed;
        }

        private void GlobalEventHandler_OnMouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Middle && source is Application app2)
            {
                EventDrawable? obj = app2.eventDrawables.First(x => x.Contains(e.X, e.Y) && !x.IsNeedToRemove);
                if(obj is not null)
                    obj.IsNeedToRemove = true;
            }
            if (e.Button==Mouse.Button.Left && IsKeyPressed(Key.P) && source is Application app)
            {
                Point p = new(e.X, e.Y, "");
               
                app.eventDrawables.Add(p);
                EnterTextPoint enterText = new(new Vector2f(e.X, e.Y), p);
                enterText.IsAlive = true;
                conveirPoint.ProcessObj(p);
                app.eventDrawables.Add(enterText);
                app.messageBox.SetString("Введите имя точки");
            }
            if (e.Button==Mouse.Button.Left && IsKeyPressed(Key.L) && source is Application app1)
            {
                Point? pon = (from elem in app1.eventDrawables
                              where elem is Point
                              let u = elem as Point
                              where u.Contains(e.X, e.Y)
                              select u).FirstOrDefault();
                if (pon is not null)
                {
                    Line line = new(pon, new Vector2f(e.X, e.Y));
                    conveirLine.ProcessObj(line);
                    line.IsAlive = true;
                    app1.eventDrawables.Insert(0,line);
                    pon.IsCatched = false;
                    pon.IsAlive = false;
                }
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
