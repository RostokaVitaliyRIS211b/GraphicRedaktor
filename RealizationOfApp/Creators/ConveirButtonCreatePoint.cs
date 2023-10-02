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
        bool isSubscribed = false;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
        }
        public Action<object?, ICollection<EventDrawableGUI>, MouseButtonEventArgs>? GetOnMouseButtonReleased(EvButton evButton)
        {
           
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
                        eventHandler.OnKeyPressed+=KeyEscUnSubscribe;
                        isSubscribed = true;
                        app.messageBox.SetString("Режим: Создать точку");
                        app.messageBox.SetPos(100, 30);
                    }
                    else
                    {
                        UnSubscribe(app);
                    }
               
                }
                else if(isSubscribed && source is Application app2)
                {
                    EvButton? evCatched = (from elem in IC
                                           where elem is EvButton
                                           let u = elem as EvButton
                                           where u.textbox.Contains(e.X,e.Y)
                                           select u).FirstOrDefault();
                    if(evCatched is not null)
                    UnSubscribe(app2);
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
        void KeyEscUnSubscribe(object? source, KeyEventArgs e)
        {
            if (e.Code==Key.Escape  && source is Application app)
            {
                UnSubscribe(app);
            }
        }
        void UnSubscribe(Application app)
        {
            eventHandler.OnMouseButtonPressed-=MousePressedCreatePoint;
            eventHandler.OnKeyPressed-=KeyEscUnSubscribe;
            isSubscribed = false;
            app.messageBox.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
    }
}
