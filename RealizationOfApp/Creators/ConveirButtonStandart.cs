using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirButtonStandart:IConveir<EvButton>
    {
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonPressed+=GetOnMouseButtonPressed(obj);
            obj.OnMouseMoved+=GetOnMouseMoved(obj);
        }
        public Action<object?, ICollection<EventDrawableGUI>, MouseMoveEventArgs>? GetOnMouseMoved(EvButton evButton)
        {
            Color buffColor=evButton.textbox.GetFillRectColor();
            return (source, IC, e) =>
            {
                evButton.textbox.SetFillColorRect(evButton.textbox.Contains(e.X, e.Y)?Color.Magenta:buffColor);
            };
        }
        public Action<object?, ICollection<EventDrawableGUI>, MouseButtonEventArgs>? GetOnMouseButtonPressed(EvButton evButton)
        {
            return (source,IC, e) =>
            {
                evButton.IsAlive = evButton.textbox.Contains(e.X, e.Y);
            };
        }
    }
}
