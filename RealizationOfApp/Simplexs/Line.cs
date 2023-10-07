

namespace RealizationOfApp.Simplexs
{
    public class Line : EvObject
    {
        //TODO добавить Contains
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Color LineColor { get; set; } = Color.Black;
        public bool IsCatched { get; set; }
        public Line(Point point1,Vector2f MouseCoords)
        {
            Point1 = point1;
            point1.IGetRemoved+=x => this.IsNeedToRemove = x==Point1 || x==Point2;
            Point2 = new(MouseCoords,"");
            IsCatched = true;
        }
        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
            point1.IGetRemoved+=x => this.IsNeedToRemove = x==Point1 || x==Point2;
            point2.IGetRemoved+=x => this.IsNeedToRemove = x==Point1 || x==Point2;
        }
        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(Point1.Position.X-Point2.Position.X, 2)+Math.Pow(Point1.Position.Y-Point2.Position.Y, 2));
        }
        public float Dlina(Vector2f point1,Vector2f point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X-point2.X, 2)+Math.Pow(point1.Y-point2.Y, 2));
        }
        public Vertex[] ToArr()
        {
            return new Vertex[2] { new Vertex(Point1.Position, LineColor), new Vertex(Point2.Position, LineColor) };
        }
        public (float, float, float) GetEquvalence() => (Point1.Position.Y-Point2.Position.Y, Point2.Position.X-Point1.Position.X,
            Point1.Position.X*Point2.Position.Y-Point2.Position.X*Point1.Position.Y);
        public (float,float,float) GetEqualenceInGrid()
        {
            Vector2f point1 = Grid.PixelToAnalogCoords(Point1.Position);
            Vector2f point2 = Grid.PixelToAnalogCoords(Point2.Position);
            return (point1.Y-point2.Y, point2.X-point1.X, point1.X*point2.Y-point2.X*point1.Y);
        }
        public override bool Contains(float x, float y)
        {
            const double MAX_LENGTH_TO_POINT = 15;
            (float, float, float) coef = GetEquvalence();
            float dlina1 = Dlina(Point1.Position, new(x, y)), dlina2 = Dlina(Point2.Position, new(x, y));
            float dlinaLine = Length();
            return (Math.Abs(coef.Item1*x+coef.Item2*y+coef.Item3)/Math.Sqrt(coef.Item1*coef.Item1+coef.Item2*coef.Item2))<=MAX_LENGTH_TO_POINT && dlina1<dlinaLine-20 && dlina2<dlinaLine-20;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ToArr(), PrimitiveType.Lines, states);
        }
    }
}
