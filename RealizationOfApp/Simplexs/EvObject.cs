using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Simplexs
{
    public abstract class EvObject:EventDrawable
    {
        public event Action<object?, MouseMoveEventArgs>? OnMouseMoved;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonPressed;
        public event Action<object?, MouseButtonEventArgs>? OnMouseButtonReleased;
        public event Action<object?, MouseWheelScrollEventArgs>? OnMouseWheelScrolled;
        public event Action<object?, KeyEventArgs>? OnKeyPressed;
        public event Action<object?, KeyEventArgs>? OnKeyReleased;

        public EvObject()
        {
        }

        public override void MouseMoved(object? source , MouseMoveEventArgs e)
        {
            OnMouseMoved?.Invoke(source, e);
        }

        public override void MouseButtonPressed(object? source ,
            MouseButtonEventArgs e)
        {
            OnMouseButtonPressed?.Invoke(source, e);
        }

        public override void MouseButtonReleased(object? source ,
            MouseButtonEventArgs e)
        {
            OnMouseButtonReleased?.Invoke(source, e);
        }

        public override void MouseWheelScrolled(object? source ,
            MouseWheelScrollEventArgs e)
        {
            OnMouseWheelScrolled?.Invoke(source, e);
        }

        public override void KeyPressed(object? source , KeyEventArgs e)
        {
            OnKeyPressed?.Invoke(source, e);
        }

        public override void KeyReleased(object? source , KeyEventArgs e)
        {
            OnKeyReleased?.Invoke(source, e);
        }
    }
}
