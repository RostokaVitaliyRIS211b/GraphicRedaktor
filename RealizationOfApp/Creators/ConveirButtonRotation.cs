using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Creators
{
    public class ConveirButtonRotation:IConveir<EvButton>
    {
        protected GlobalEventHandler eventHandler;
        protected List<Point> selectePoints = new();
        protected Point? pointOfRotate;
        protected Matrix matrixComplexLeft = new();
        protected Matrix matrixComplexRight = new();
        protected const float ANGLE_PER_TICK = 0.01f;
        bool isSubscribed = false;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnMouseMoved+=OnMouseMoved;
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
                        eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelectPointOfRotate;
                        eventHandler.OnKeyPressed+=KeyEscUnSubscribe;
                        isSubscribed = true;
                        app.isCanResetString = true;
                        app.SetString("Режим: Выбор точки вращения");
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
        void MouseButtonReleased_SelectPointOfRotate(object? source, MouseButtonEventArgs e)
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
                    pointOfRotate = pon;
                    pointOfRotate.FillColor = Color.Red;
                    eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointOfRotate;
                    eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelectPointsToRotate;
                    eventHandler.OnKeyPressed+=KeyEndSelectPoints;
                    app.isCanResetString = true;
                    app.SetString("Режим: Выбор точек для вращения");
                    app.isCanResetString = false;
                }
            }
        }
        void MouseButtonReleased_SelectPointsToRotate(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Left && source is Application app && e.X>200)
            {
                Point? pon = (from elem in app.eventDrawables
                              where elem is Point
                              let u = elem as Point
                              where u.Contains(e.X, e.Y) && u!=pointOfRotate && !selectePoints.Contains(u)
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
                eventHandler.OnKeyPressed-=KeyEndSelectPoints;
                eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointsToRotate;
                eventHandler.OnKeyPressed+=KeyRotate;
                app.isCanResetString = true;
                app.SetString("Вращение");
                app.isCanResetString = false;
                Matrix matrixRotate = new(), matrixTransportBack = new();
                float cos = 0.998476952f, sin = 0.0174524064f;
                matrixRotate.AddLastString(new float[] { cos, sin, 0 });
                matrixRotate.AddLastString(new float[] { -sin, cos, 0 });
                matrixRotate.AddLastString(new float[] { 0, 0, 1 });
                Vector2f analog = Grid.PixelToAnalogCoords(pointOfRotate.Position);
                matrixComplexLeft.AddLastString(new float[] { 1, 0, 0 });
                matrixComplexLeft.AddLastString(new float[] { 0, 1, 0 });
                matrixComplexLeft.AddLastString(new float[] { -analog.X, -analog.Y, 1 });
                matrixTransportBack.AddLastString(new float[] { 1, 0, 0 });
                matrixTransportBack.AddLastString(new float[] { 0, 1, 0 });
                matrixTransportBack.AddLastString(new float[] { analog.X, analog.Y, 1 });
                matrixComplexLeft*=matrixRotate;
                matrixComplexLeft*=matrixTransportBack;
                sin = -sin;
                matrixRotate.Clear();
                matrixRotate.AddLastString(new float[] { cos, sin, 0 });
                matrixRotate.AddLastString(new float[] { -sin, cos, 0 });
                matrixRotate.AddLastString(new float[] { 0, 0, 1 });
                matrixComplexRight.AddLastString(new float[] { 1, 0, 0 });
                matrixComplexRight.AddLastString(new float[] { 0, 1, 0 });
                matrixComplexRight.AddLastString(new float[] { -analog.X, -analog.Y, 1 });
                matrixComplexRight*=matrixRotate;
                matrixComplexRight*=matrixTransportBack;
            }
        }
        void KeyRotate(object? source, KeyEventArgs e)
        {
            if(e.Code==Key.Q && source is Application app)
            {
                foreach(Point p in selectePoints)
                {
                    Vector2f pos = Grid.PixelToAnalogCoords(p.Position);
                    Matrix matrix = new();
                    matrix.AddLastString(new float[] { pos.X,pos.Y,p.PositionKG.Item3 });
                    matrix*=matrixComplexLeft;
                    pos = new(matrix[0, 0]/matrix[0,2], matrix[0, 1]/matrix[0, 2]);
                    pos = Grid.AnalogToPixelCoords(pos);
                    p.PositionKG = (pos.X,pos.Y,1);
                }
            }
            if(e.Code==Key.E && source is Application app2)
            {
                foreach (Point p in selectePoints)
                {
                    Vector2f pos = Grid.PixelToAnalogCoords(p.Position);
                    Matrix matrix = new();
                    matrix.AddLastString(new float[] { pos.X, pos.Y, p.PositionKG.Item3 });
                    matrix*=matrixComplexRight;
                    pos = new(matrix[0, 0]/matrix[0, 2], matrix[0, 1]/matrix[0, 2]);
                    pos = Grid.AnalogToPixelCoords(pos);
                    p.PositionKG = (pos.X, pos.Y, 1);
                }
            }
        }
        void UnSubscribe(Application app)
        {
            eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointOfRotate;
            eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointsToRotate;
            eventHandler.OnKeyPressed-=KeyEndSelectPoints;
            eventHandler.OnKeyPressed-=KeyEscUnSubscribe;
            eventHandler.OnKeyPressed-=KeyRotate;
            isSubscribed = false;
            pointOfRotate = null;
            selectePoints.Clear();
            app.isCanResetString = true;
            app.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
        public void OnMouseMoved(object? source,ICollection<EventDrawableGUI> even,MouseMoveEventArgs e)
        {
            if(pointOfRotate is not null)
                pointOfRotate.FillColor=Color.Red;
            foreach (Point point in selectePoints)
            {
                point.FillColor = Color.Green;
            }
        }

    }
}
