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

            };
        }
        public Action<object?, MouseButtonEventArgs>? GetOnMouseButtonPressed(Line line)
        {
            return (source, e) =>
            {

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
