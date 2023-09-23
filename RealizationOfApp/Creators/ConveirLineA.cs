using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirLineA : IConveir<Line>
    {
        public void ProcessObj(Line obj)
        {
            obj.OnMouseMoved+=GetOnMouseMoved(obj);
            obj.OnMouseButtonPressed+=GetOnMouseButtonPressed(obj);
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnKeyPressed+=GetOnKeyPressed(obj);
            obj.OnKeyReleased+=GetOnKeyReleased(obj);

        }
        public Action<object?, MouseMoveEventArgs>? GetOnMouseMoved(Line line)
        {
            return (source, e) =>
            {
                if (line.IsCatched && line.IsAlive)
                    line.Point2 = new(new Vector2f(e.X, e.Y), "");
            };
        }
        public Action<object?, MouseButtonEventArgs>? GetOnMouseButtonPressed(Line line)
        {
            return (source, e) =>
            {
                if(e.Button == Mouse.Button.Left && line.IsCatched && line.IsAlive && source is Application app)
                {
                    Point? p = (from elem in app.eventDrawables
                                where elem is Point
                                let u = elem as Point
                                where u.Contains(e.X,e.Y) && line.Point1!=u && line.Point2!=u
                                select u).FirstOrDefault();
                    if(p is not null)
                    {
                        line.Point2 = p;
                        p.IGetRemoved+=x => line.IsNeedToRemove = x==line.Point1 ||  x==line.Point2;
                        line.IsAlive = false;
                        line.IsCatched = false;
                    }
                }
            };
        }
        public Action<object?, MouseButtonEventArgs>? GetOnMouseButtonReleased(Line line)
        {
            return (source, e) =>
            {

            };
        }
        public Action<object?, MouseWheelScrollEventArgs>? GetOnMouseWheelScrolled(Line line)
        {
            return (source, e) => { };
        }
        public Action<object?, KeyEventArgs>? GetOnKeyPressed(Line line)
        {
            return (source, e) =>
            {
                line.IsNeedToRemove=line.IsAlive && e.Code==Key.Escape;
            };
        }
        public Action<object?, KeyEventArgs>? GetOnKeyReleased(Line line)
        {
            return (source, e) =>
            {

            };
        }
    }
}
