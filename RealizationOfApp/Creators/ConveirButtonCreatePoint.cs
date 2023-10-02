using SfmlAppLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirButtonCreatePoint:IConveir<EvButton>
    {
        GlobalEventHandler eventHandler;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
        }
        public Action<object?, ICollection<EventDrawableGUI>, MouseButtonEventArgs>? GetOnMouseButtonReleased(EvButton evButton)
        {
            bool isSubscribed = false;
            return (source, IC, e) =>
            {
                if(evButton.IsAlive && source is Application app)
                {
                      eventHandler = (from elem in app.eventHandlers
                                                       where elem is GlobalEventHandler
                                                       select elem as GlobalEventHandler).First();
                    
                    if (!isSubscribed)
                    {
                        eventHandler.OnMouseButtonPressed+=MousePressedCreatePoint;
                        isSubscribed = true;
                        app.messageBox.SetString("Режим: Создать точку");
                        app.messageBox.SetPos(100, 30);
                    }
                    else
                    {
                        eventHandler.OnMouseButtonPressed-=MousePressedCreatePoint;
                        isSubscribed = false;
                        app.messageBox.SetString("Режим:");
                        app.messageBox.SetPos(100, 30);
                    }
               
                }
                evButton.IsAlive = false;
            };
        }
        void MousePressedCreatePoint(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Left && source is Application app && e.X>200)
            {
                if (app.eventDrawables.Find(x => x is EnterTextPoint) is null)
                {
                    Point p = new(e.X, e.Y, "");
                    app.eventDrawables.Add(p);
                    EnterTextPoint enterText = new(new Vector2f(e.X, e.Y), p);
                    enterText.IsAlive = true;
                    eventHandler.conveirPoint.ProcessObj(p);
                    app.eventDrawables.Add(enterText);
                    app.messageBox.SetString("Введите имя точки");
                    app.messageBox.SetPos(100, 30);
                }
            }
        }
    }
}
