using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class Grid:EvObject
    {
        public List<Line> lines = new();
        public List<Textbox> textboxes = new();
        Arrow arrowX,arrowY;
        protected Vector2f currentCenter;
          
        protected float LengthOneDividePX { get; set; } = 50;
        protected float LengthOneDivideAnalog { get; set; } = 1;
        public Grid(Vector2f currentCenter,uint currentWidth,uint currentHeight)
        {
            this.currentCenter = currentCenter;
            int countOfX = (int)(currentWidth/LengthOneDividePX)+1;
            int countofY = (int)(currentHeight/LengthOneDividePX)+1;
            Point point1 = new(new(currentCenter.X,0),"");
            Point point2 = new (new(currentCenter.X, currentHeight), "");
            lines.Add(new(point1,point2));
            point1 = new(new(0, currentCenter.Y), "");
            point2 = new(new(currentWidth, currentCenter.Y), "");
            lines.Add(new(point1, point2));
            for (int i=1;i<=countOfX/2;++i)
            {
                point1 = new(new(currentCenter.X+LengthOneDividePX*i, 0), "");
                point2 = new(new(currentCenter.X+LengthOneDividePX*i, currentHeight), "");
                lines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countOfX/2; ++i)
            {
                point1 = new(new(currentCenter.X-LengthOneDividePX*i, 0), "");
                point2 = new(new(currentCenter.X-LengthOneDividePX*i, currentHeight), "");
                lines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countofY/2; ++i)
            {
                point1 = new(new(0, currentCenter.Y-LengthOneDividePX*i), "");
                point2 = new(new(currentWidth, currentCenter.Y-LengthOneDividePX*i), "");
                lines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countofY/2; ++i)
            {
                point1 = new(new(0, currentCenter.Y+LengthOneDividePX*i), "");
                point2 = new(new(currentWidth, currentCenter.Y+LengthOneDividePX*i), "");
                lines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            Textbox textbox = new();
            textbox.SetFillColorText(Color.Black);
            textbox.SetSizeCharacterText(14);
            float tyu = 0;
            textbox.SetString(tyu.ToString());
            textbox.SetPos(currentCenter.X-16,currentCenter.Y+16);
            textboxes.Add(new(textbox));
            Vector2f position = new(currentCenter.X, currentCenter.Y-16);
            for(int i=0;i<countOfX/2;++i)
            {
                tyu+=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.X+=LengthOneDividePX;
                textbox.SetPos(position);
                textboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X, currentCenter.Y-16);
            tyu = 0;
            for (int i = 0; i<countOfX/2; ++i)
            {
                tyu-=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.X-=LengthOneDividePX;
                textbox.SetPos(position);
                textboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X+16, currentCenter.Y);
            tyu = 0;
            for (int i = 0; i<countofY/2; ++i)
            {
                tyu+=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.Y-=LengthOneDividePX;
                textbox.SetPos(position);
                textboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X+16, currentCenter.Y);
            tyu = 0;
            for (int i = 0; i<countofY/2; ++i)
            {
                tyu-=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.Y+=LengthOneDividePX;
                textbox.SetPos(position);
                textboxes.Add(new Textbox(textbox));
            }
            arrowX = new(new Vector2f(currentWidth, currentCenter.Y),1);
            arrowY = new Arrow(new Vector2f(currentCenter.X, 0),0);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 0; i<lines.Count; ++i)
                target.Draw(lines[i],states);

            for (int i = 0; i<textboxes.Count; ++i)
                target.Draw(textboxes[i],states);

            target.Draw(arrowX);
            target.Draw(arrowY);
        }
    }
}
