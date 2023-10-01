﻿

namespace RealizationOfApp.Simplexs
{
    public class Line : EvObject
    {
        //TODO добавить вычисление уранения по двум точкам
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
        public float Dlina()
        {
            return (float)Math.Sqrt(Math.Pow(Point1.Position.X-Point2.Position.X, 2)+Math.Pow(Point1.Position.Y-Point2.Position.Y, 2));
        }
        public Vertex[] ToArr()
        {
            return new Vertex[2] { new Vertex(Point1.Position, LineColor), new Vertex(Point2.Position, LineColor) };
        }
        public (float, float, float) GetEquvalence() => (Point2.Position.X-Point1.Position.X,Point1.Position.Y-Point2.Position.Y,
            Point1.Position.X*Point2.Position.Y-Point2.Position.X*Point1.Position.Y);
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ToArr(), PrimitiveType.Lines, states);
        }
    }
}
