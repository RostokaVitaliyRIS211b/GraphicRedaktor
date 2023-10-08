using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirButtonTransfer
    {
        protected GlobalEventHandler eventHandler;
        protected List<Point> selectePoints = new();
        protected Matrix matrixTransfer = new();
        protected int counter = 0;
        protected bool isSubscribed = false;
        protected float? xDelta = null, yDelta = null;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnMouseMoved+=MouseMovedPaintPoints;
            matrixTransfer.AddLastString(new float[] { 1, 0, 0 });
            matrixTransfer.AddLastString(new float[] { 0, 1, 0 });
            matrixTransfer.AddLastString(new float[] { 0, 0, 1 });
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
                        eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelectPointsToTransfer;
                        eventHandler.OnKeyPressed+=KeyEndSelectPoints;
                        eventHandler.OnKeyPressed+=KeyEscUnSubscribe;
                        isSubscribed = true;
                        app.isCanResetString = true;
                        app.SetString("Режим: Выбор точек\nдля перемещения");
                        app.isCanResetString = false;
                    }
                    else
                    {
                        UnSubscribe(app);
                    }

                }
                else if (isSubscribed && source is Application app2)
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
        void MouseButtonReleased_SelectPointsToTransfer(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Right && source is Application app && e.X>200)
            {
                Point? pon = (from elem in app.eventDrawables
                              where elem is Point
                              let u = elem as Point
                              where u.Contains(e.X, e.Y) && !selectePoints.Contains(u)
                              select u).FirstOrDefault();
                if (pon is not null)
                {
                    selectePoints.Add(pon);
                    pon.FillColor = Color.Green;
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
        void KeyEndSelectPoints(object? source, KeyEventArgs e)
        {
            if (e.Code==Key.Enter  && source is Application app)
            {
                app.isCanResetString = true;
                app.SetString("Режим: Введите перем\nещение по оси X");
                app.isCanResetString = false;
                EnterTextTexbox enterTextTexbox = new(new(640, 360));
                enterTextTexbox.identify="X";
                enterTextTexbox.IsAlive = true;
                app.eventDrawables.Add(enterTextTexbox);
                eventHandler.OnKeyPressed-=KeyEndSelectPoints;
                eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointsToTransfer;
                eventHandler.OnKeyPressed+=KeyEnterDelta;
            }
        }
        void KeyEnterDelta(object? source, KeyEventArgs e)
        {
            if (e.Code==Key.Enter && source is Application app)
            {
                EnterTextTexbox? enterTextX = (from elem in app.eventDrawables
                                               where elem is EnterTextTexbox
                                               let u = elem as EnterTextTexbox
                                               where u.identify=="X" && !u.IsAlive
                                               select u).FirstOrDefault();
                EnterTextTexbox? enterTextY = (from elem in app.eventDrawables
                                               where elem is EnterTextTexbox
                                               let u = elem as EnterTextTexbox
                                               where u.identify=="Y" && !u.IsAlive
                                               select u).FirstOrDefault();
                if (enterTextX is not null && xDelta is null)
                {
                    xDelta = float.Parse(enterTextX.textbox.GetString());
                    enterTextX.IsNeedToRemove=true;
                    app.isCanResetString = true;
                    app.SetString("Режим: Введите перем\nещение по оси Y");
                    app.isCanResetString = false;
                    EnterTextTexbox enterTextTexbox = new(new(640, 360));
                    enterTextTexbox.identify="Y";
                    enterTextTexbox.IsAlive = true;
                    app.eventDrawables.Add(enterTextTexbox);
                }
                if (enterTextY is not null && yDelta is null)
                {
                    yDelta = float.Parse(enterTextY.textbox.GetString());
                    enterTextY.IsNeedToRemove=true;
                    matrixTransfer[2, 0] = xDelta ?? 0; matrixTransfer[2, 1] = yDelta ?? 0;
                    foreach (Point p in selectePoints)
                    {
                        Vector2f pos = Grid.PixelToAnalogCoords(p.Position);
                        Matrix matrix = new();
                        matrix.AddLastString(new float[] { pos.X, pos.Y, p.PositionKG.Item3 });
                        matrix*=matrixTransfer;
                        pos = new Vector2f(matrix[0, 0]/matrix[0, 2], matrix[0, 1]/matrix[0, 2]);
                        pos = Grid.AnalogToPixelCoords(pos);
                        p.PositionKG = (pos.X, pos.Y, 1);
                    }
                    UnSubscribe(app);
                }

            }
        }
        void UnSubscribe(Application app)
        {

            EnterTextTexbox? enterTextX = (from elem in app.eventDrawables
                                           where elem is EnterTextTexbox
                                           let u = elem as EnterTextTexbox
                                           where u.identify=="X" && !u.IsAlive
                                           select u).FirstOrDefault();
            EnterTextTexbox? enterTextY = (from elem in app.eventDrawables
                                           where elem is EnterTextTexbox
                                           let u = elem as EnterTextTexbox
                                           where u.identify=="Y" && !u.IsAlive
                                           select u).FirstOrDefault();
            if (enterTextX is not null)
                enterTextX.IsNeedToRemove=true;
            if (enterTextY is not null)
                enterTextY.IsNeedToRemove=true;
            eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointsToTransfer;
            eventHandler.OnKeyPressed-=KeyEndSelectPoints;
            eventHandler.OnKeyPressed-=KeyEscUnSubscribe;
            eventHandler.OnKeyPressed-=KeyEnterDelta;
            isSubscribed = false;
            foreach (Point p in selectePoints)
                p.FillColor=p.BuffColor;
            selectePoints.Clear();
            xDelta = null;
            yDelta = null;
            app.isCanResetString = true;
            app.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
        public void MouseMovedPaintPoints(object? source, ICollection<EventDrawableGUI> even, MouseMoveEventArgs e)
        {
            foreach (Point point in selectePoints)
            {
                point.FillColor = Color.Green;
            }
        }
    }
}
