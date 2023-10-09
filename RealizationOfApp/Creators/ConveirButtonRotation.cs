namespace RealizationOfApp.Creators
{
    public class ConveirButtonRotation : IConveir<EvButton>
    {
        protected GlobalEventHandler eventHandler;
        protected List<Point> selectePoints = new();
        protected List<Vector2f> oldPositions = new();
        protected Point? pointOfRotate;
        protected Matrix matrixComplexLeft = new();
        protected int counter = 0;
        bool isSubscribed = false;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnMouseMoved+=MouseMovedPaintPoints;
            matrixComplexLeft.AddLastString(new float[] { 1, 0, 0 });
            matrixComplexLeft.AddLastString(new float[] { 0, 1, 0 });
            matrixComplexLeft.AddLastString(new float[] { 0, 0, 1 });
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
                        app.SetString("Режим: Выбор точки\n вращения");
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
            if (e.Button==Mouse.Button.Right && source is Application app && e.X>200)
            {
                Point? pon = (from elem in app.eventDrawables
                              where elem is Point
                              let u = elem as Point
                              where u.Contains(e.X, e.Y)
                              select u).FirstOrDefault();
                if (pon is not null)
                {
                    pointOfRotate = pon;
                    eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointOfRotate;
                    eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelectPointsToRotate;
                    eventHandler.OnKeyPressed+=KeyEndSelectPoints;
                    app.isCanResetString = true;
                    app.SetString("Режим: Выбор точек\n для вращения");
                    app.isCanResetString = false;
                }
            }
        }
        void MouseButtonReleased_SelectPointsToRotate(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Right && source is Application app && e.X>200)
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
                    oldPositions.Add(pon.Position);
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
                counter=0;
            }
        }
        void KeyRotate(object? source, KeyEventArgs e)
        {
            if (e.Code==Key.Q || e.Code==Key.E && source is Application app)
            {
                counter = e.Code==Key.Q?counter+1:counter-1;
                counter%=360;
                UpdateMatrixs();
                for (int i = 0; i<selectePoints.Count; ++i)
                {
                    Vector2f pos = Grid.PixelToAnalogCoords(oldPositions[i]);
                    Matrix matrix = new();
                    matrix.AddLastString(new float[] { pos.X, pos.Y, selectePoints[i].PositionKG.Item3 });
                    matrix*=matrixComplexLeft;
                    pos = new(matrix[0, 0]/matrix[0, 2], matrix[0, 1]/matrix[0, 2]);
                    pos = Grid.AnalogToPixelCoords(pos);
                    selectePoints[i].PositionKG = (pos.X, pos.Y, 1);
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
            if (pointOfRotate is not null)
                pointOfRotate.FillColor=pointOfRotate.BuffColor;
            pointOfRotate = null;
            foreach (Point p in selectePoints)
                p.FillColor=p.BuffColor;
            selectePoints.Clear();
            oldPositions.Clear();
            counter=0;
            app.isCanResetString = true;
            app.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
        public void MouseMovedPaintPoints(object? source, ICollection<EventDrawableGUI> even, MouseMoveEventArgs e)
        {
            if (pointOfRotate is not null)
                pointOfRotate.FillColor=Color.Yellow;
            foreach (Point point in selectePoints)
            {
                point.FillColor = Color.Green;
            }
        }
        void UpdateMatrixs()
        {
            float cos = MathF.Cos(counter*(MathF.PI/180)), sin = MathF.Sin(counter*(MathF.PI/180));
            Vector2f analog = Grid.PixelToAnalogCoords(pointOfRotate.Position);
            matrixComplexLeft[0, 0] = cos; matrixComplexLeft[0, 1]=sin;
            matrixComplexLeft[1, 0] = -sin; matrixComplexLeft[1, 1]=cos;
            matrixComplexLeft[2, 0] = -analog.X*cos+sin*analog.Y+analog.X;
            matrixComplexLeft[2, 1] = -analog.Y*cos-analog.X*sin+analog.Y;
        }
    }
}
