using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class Arrow : EventDrawable
    {
        public Vector2f Position { get; set; }
        protected float CosAngle;
        public Arrow(Vector2f position,float cosAngle)
        {
            Position = position;
            CosAngle = cosAngle;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            float posXMiddle = Position.X;
            float posYMiddle = Position.Y;
            bool DifferenceY = CosAngle==1;
            float SinAngle = (float)Math.Sqrt(1 - CosAngle * CosAngle);
            Vertex vertexArrMid = new(new(posXMiddle, posYMiddle), Color.Black);
            Vertex verUp = new(new(posXMiddle - 20 * CosAngle, posYMiddle + (DifferenceY ? -20 * SinAngle : +20 * SinAngle)), Color.Black);
            Vertex verUp2 = new(new(verUp.Position.X + (!DifferenceY ? -10 * SinAngle : +10 * SinAngle), verUp.Position.Y - 10 * CosAngle), Color.Black);
            Vertex verUp3 = new(new(verUp.Position.X - (!DifferenceY ? -10 * SinAngle : +10 * SinAngle), verUp.Position.Y + 10 * CosAngle), Color.Black);
            Vertex[] vertices = new Vertex[3] { verUp2, verUp3, vertexArrMid };
            target.Draw(vertices, PrimitiveType.Triangles, states);
        }
    }
}
