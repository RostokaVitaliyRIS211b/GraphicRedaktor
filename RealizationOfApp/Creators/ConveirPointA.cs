using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirPointA : IConveir<Point>
    {
        //TODO Добавить прилипание к сетке по координатам кратным едичиному делению сетки
        public void ProcessObj(Point obj)
        {
            obj.OnMouseMoved+=GetOnMouseMoved(point: obj);
            obj.OnMouseButtonPressed+=GetOnMouseButtonPressed(obj);
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnKeyPressed+=GetOnKeyPressed(obj);
            obj.OnKeyReleased+=GetOnKeyReleased(obj);
        }
        public Action<object?, MouseMoveEventArgs>? GetOnMouseMoved(Point point)
        {
            return (source, e) =>
            {
                if (point.IsCatched)
                {
                    point.Position = new Vector2f(e.X, e.Y);
                }
                point.FillColor = point.Contains(e.X, e.Y) ? Color.Magenta : point.BuffColor;
                if (point.Contains(e.X, e.Y) && source is Application app)
                {
                    Vector2f coordsGrid = Grid.PixelToAnalogCoords(point.Position);
                    app.SetString($"({coordsGrid.X}:{coordsGrid.Y})");
                    app.messageBox.SetPos(100, 30);
                }
            };
        }
        public Action<object?, MouseButtonEventArgs>? GetOnMouseButtonPressed(Point point)
        {
            return (source, e) =>
            {
                point.IsAlive = point.Contains(e.X, e.Y) && source is Application && e.Button==Mouse.Button.Left;
                point.IsCatched = point.IsAlive;
            };
        }
        public Action<object?, MouseButtonEventArgs>? GetOnMouseButtonReleased(Point point)
        {
            return (source, e) =>
            {
                point.IsCatched = false;
            };
        }
        public Action<object?, MouseWheelScrollEventArgs>? GetOnMouseWheelScrolled(Point point)
        {
            return (source, e) => { };
        }
        public Action<object?, KeyEventArgs>? GetOnKeyPressed(Point point)
        {
            return (source, e) =>
            {
                point.IsTextVisible = IsKeyPressed(Key.LAlt);
            };
        }
        public Action<object?, KeyEventArgs>? GetOnKeyReleased(Point point)
        {
            return (source, e) =>
            {
                point.IsTextVisible = false;
            };
        }
    }
}
