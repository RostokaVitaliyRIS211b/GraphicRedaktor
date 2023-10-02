using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirButtonCreateLine:IConveir<EvButton>
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
                if (evButton.IsAlive && source is Application app)
                {
                    eventHandler = (from elem in app.eventHandlers
                                    where elem is GlobalEventHandler
                                    select elem as GlobalEventHandler).First();

                    if (!isSubscribed)
                    {
                        eventHandler.OnMouseButtonPressed+=MousePressedCreateLine;
                        eventHandler.OnKeyPressed+=KeyEscUnSubscribe;
                        isSubscribed = true;
                        app.messageBox.SetString("Режим: Создать Прямую");
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
                                           where u.textbox.Contains(e.X, e.Y)
                                           select u).FirstOrDefault();
                    if (evCatched is not null)
                        UnSubscribe(app2);
                }
                evButton.IsAlive = false;
            };
        }
        void MousePressedCreateLine(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Left && source is Application app && e.X>200)
            {
                Point? pon = (from elem in app.eventDrawables
                              where elem is Point
                              let u = elem as Point
                              where u.Contains(e.X, e.Y)
                              select u).FirstOrDefault();
                if (pon is not null)
                {
                    Line line = new(pon, new Vector2f(e.X, e.Y));
                    eventHandler.conveirLine.ProcessObj(line);
                    line.IsAlive = true;
                    app.eventDrawables.Insert(0, line);
                    pon.IsCatched = false;
                    pon.IsAlive = false;
                }
            }
        }
        void KeyEscUnSubscribe(object? source, KeyEventArgs e)
        {
            if(e.Code==Key.Escape  && source is Application app)
            {
                UnSubscribe(app);
            }
        }
        void UnSubscribe(Application app)
        {
            eventHandler.OnMouseButtonPressed-=MousePressedCreateLine;
            eventHandler.OnKeyPressed-=KeyEscUnSubscribe;
            isSubscribed = false;
            app.messageBox.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
    }
}
