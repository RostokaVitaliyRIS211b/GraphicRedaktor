

namespace RealizationOfApp.Creators
{
    public class ConveirButtonMirroring:IConveir<EvButton>
    {
        protected GlobalEventHandler eventHandler;
        protected List<Point> selectePoints = new();
        protected List<Vector2f> oldPositions = new();
        protected Line? LineOfRotate;
        protected Matrix matrixMirroring = new();
        protected int counter = 0;
        bool isSubscribed = false;
        public void ProcessObj(EvButton obj)
        {
            obj.OnMouseButtonReleased+=GetOnMouseButtonReleased(obj);
            obj.OnMouseMoved+=MouseMovedPaintPoints;
            matrixMirroring.AddLastString(new float[] { 1, 0, 0 });
            matrixMirroring.AddLastString(new float[] { 0, 1, 0 });
            matrixMirroring.AddLastString(new float[] { 0, 0, 1 });
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
                        eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelecLineOfMirroring;
                        eventHandler.OnKeyPressed+=KeyEscUnSubscribe;
                        isSubscribed = true;
                        app.isCanResetString = true;
                        app.SetString("Режим: Выбор прямой\nдля зеркалирования");
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
        void MouseButtonReleased_SelecLineOfMirroring(object? source, MouseButtonEventArgs e)
        {
            if (e.Button==Mouse.Button.Right && source is Application app && e.X>200)
            {
                Line? pon = (from elem in app.eventDrawables
                              where elem is  Line
                             let u = elem as Line
                             where u.Contains(e.X, e.Y)
                              select u).FirstOrDefault();
                if (pon is not null)
                {
                    LineOfRotate = pon;
                    eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelecLineOfMirroring;
                    eventHandler.OnMouseButtonReleased+=MouseButtonReleased_SelectPointsToMirroring;
                    eventHandler.OnKeyPressed+=KeyEndSelectPoints;
                    app.isCanResetString = true;
                    app.SetString("Режим: Выбор точек\nдля зеркалирования");
                    app.isCanResetString = false;
                }
            }
        }
        void MouseButtonReleased_SelectPointsToMirroring(object? source, MouseButtonEventArgs e)
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
                (float, float, float) coefs = LineOfRotate.GetEqualenceInGrid();
                float y = -coefs.Item3/coefs.Item2,angle = -MathF.Atan(-coefs.Item1/coefs.Item2);
                if(coefs.Item2==0)
                {
                    float x = -coefs.Item3/coefs.Item1;
                    matrixMirroring[0, 0]=-1; matrixMirroring[0, 1]=0;
                    matrixMirroring[1, 0]=0; matrixMirroring[1, 1]=1;
                    matrixMirroring[2, 0]=2*x; matrixMirroring[2, 1]=0;
                }
                else if(coefs.Item1==0)
                {
                    matrixMirroring[0, 0]=1; matrixMirroring[0, 1]=0;
                    matrixMirroring[1, 0]=0; matrixMirroring[1, 1]=-1;
                    matrixMirroring[2, 0]=0; matrixMirroring[2, 1]=2*y;
                }
                else
                {
                    float cos1 = MathF.Cos(angle), sin1 = MathF.Sin(angle), cos2 = MathF.Cos(-angle), sin2 = MathF.Sin(-angle);
                    //matrixMirroring.AddLastString(new float[] { cos1*cos2+sin1*sin2, cos1*sin2-sin1*cos2, 0 });
                    //matrixMirroring.AddLastString(new float[] { cos1*sin2-sin1*cos2, -sin1*sin2-cos1*cos2, 0 });
                    //matrixMirroring.AddLastString(new float[] { y*sin1*cos2-y*cos1*sin2, y*sin1*sin2+y*cos1*cos2 + y, 1 });
                    //Matrix CurrentMatrix = new();
                    //CurrentMatrix.AddLastString(new float[] { cos1, sin1, 0 });
                    //CurrentMatrix.AddLastString(new float[] { -sin1, cos1, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, 0, 1 });
                    //matrixMirroring*=CurrentMatrix;
                    //CurrentMatrix.Clear();
                    //CurrentMatrix.AddLastString(new float[] { 1, 0, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, -1, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, 0, 1 });
                    //matrixMirroring*=CurrentMatrix;
                    //CurrentMatrix.Clear();
                    //CurrentMatrix.AddLastString(new float[] { cos2, sin2, 0 });
                    //CurrentMatrix.AddLastString(new float[] { -sin2, cos2, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, 0, 1 });
                    //matrixMirroring*=CurrentMatrix;
                    //CurrentMatrix.Clear();
                    //CurrentMatrix.AddLastString(new float[] { 1, 0, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, 1, 0 });
                    //CurrentMatrix.AddLastString(new float[] { 0, y, 1 });
                    //matrixMirroring*=CurrentMatrix;
                    matrixMirroring[0, 0]=cos1*cos2+sin1*sin2; matrixMirroring[0, 1]=cos1*sin2-sin1*cos2;
                    matrixMirroring[1, 0]=cos1*sin2-sin1*cos2; matrixMirroring[1, 1]=-sin1*sin2-cos1*cos2;
                    matrixMirroring[2, 0]=y*sin1*cos2-y*cos1*sin2; matrixMirroring[2, 1]=y*sin1*sin2+y*cos1*cos2 + y;
                }
                foreach(Point p in selectePoints)
                {
                    Vector2f pos = Grid.PixelToAnalogCoords(p.Position);
                    Matrix matrix = new();
                    matrix.AddLastString(new float[] {pos.X,pos.Y,p.PositionKG.Item3 });
                    matrix*=matrixMirroring;
                    pos = new Vector2f(matrix[0, 0]/matrix[0, 2], matrix[0, 1]/matrix[0,2]);
                    pos = Grid.AnalogToPixelCoords(pos);
                    p.PositionKG = (pos.X,pos.Y,1);
                }
                UnSubscribe(app);
            }
        }
        
        void UnSubscribe(Application app)
        {
            eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelecLineOfMirroring;
            eventHandler.OnMouseButtonReleased-=MouseButtonReleased_SelectPointsToMirroring;
            eventHandler.OnKeyPressed-=KeyEndSelectPoints;
            eventHandler.OnKeyPressed-=KeyEscUnSubscribe;
            isSubscribed = false;
            if (LineOfRotate is not null)
                LineOfRotate.LineColor=Color.Black;
            LineOfRotate = null;
            foreach (Point p in selectePoints)
                p.FillColor=p.BuffColor;
            selectePoints.Clear();
            app.isCanResetString = true;
            app.SetString("Режим:");
            app.messageBox.SetPos(100, 30);
        }
        public void MouseMovedPaintPoints(object? source, ICollection<EventDrawableGUI> even, MouseMoveEventArgs e)
        {
            if (LineOfRotate is not null)
                LineOfRotate.LineColor=Color.Yellow;
            foreach (Point point in selectePoints)
            {
                point.FillColor = Color.Green;
            }
        }
    }
}
