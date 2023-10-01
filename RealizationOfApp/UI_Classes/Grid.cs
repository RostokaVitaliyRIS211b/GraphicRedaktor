using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class Grid : EvObject
    {
        //TODO добавить масштабируемость как в геобебре
        public List<Line> horizonLines = new();
        public List<Line> vertLines = new();
        public List<Textbox> verTextboxes = new();
        public List<Textbox> horTextboxes = new();
        Arrow arrowX, arrowY;
        public static Vector2f CurrentCenter { get; protected set; }
        public uint CurrentWidth, CurrentHeight, LeftBoard;
        public static float LengthOneDividePX { get; protected set; } = 50;
        public static float LengthOneDivideAnalog { get; protected set; } = 1;
        public Grid(Vector2f currentCenter, uint currentWidth, uint currentHeight, uint leftBoard = 0)
        {
            CurrentCenter = currentCenter;
            CurrentWidth = currentWidth;
            CurrentHeight = currentHeight;
            LeftBoard = leftBoard;
            int countOfX = (int)(currentWidth/LengthOneDividePX)+1;
            int countofY = (int)(currentHeight/LengthOneDividePX)+1;
            Point point1 = new(new(currentCenter.X, 0), "");
            Point point2 = new(new(currentCenter.X, currentHeight), "");
            vertLines.Add(new(point1, point2));
            point1 = new(new(0, currentCenter.Y), "");
            point2 = new(new(currentWidth, currentCenter.Y), "");
            horizonLines.Add(new(point1, point2));
            for (int i = 1; i<=countOfX/2; ++i)
            {
                point1 = new(new(currentCenter.X+LengthOneDividePX*i, 0), "");
                point2 = new(new(currentCenter.X+LengthOneDividePX*i, currentHeight), "");
                vertLines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countOfX/2; ++i)
            {
                point1 = new(new(currentCenter.X-LengthOneDividePX*i, 0), "");
                point2 = new(new(currentCenter.X-LengthOneDividePX*i, currentHeight), "");
                vertLines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countofY/2; ++i)
            {
                point1 = new(new(0, currentCenter.Y-LengthOneDividePX*i), "");
                point2 = new(new(currentWidth, currentCenter.Y-LengthOneDividePX*i), "");
                horizonLines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            for (int i = 1; i<=countofY/2; ++i)
            {
                point1 = new(new(0, currentCenter.Y+LengthOneDividePX*i), "");
                point2 = new(new(currentWidth, currentCenter.Y+LengthOneDividePX*i), "");
                horizonLines.Add(new Line(point1, point2) { LineColor = new Color(204, 204, 204) });
            }
            Textbox textbox = new();
            textbox.SetFillColorText(Color.Black);
            textbox.SetSizeCharacterText(14);
            float tyu = 0;
            textbox.SetString(tyu.ToString());
            textbox.SetPos(currentCenter.X-16, currentCenter.Y-16);
            horTextboxes.Add(new(textbox));
            Vector2f position = new(currentCenter.X, currentCenter.Y-16);
            for (int i = 0; i<countOfX/2; ++i)
            {
                tyu+=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.X+=LengthOneDividePX;
                textbox.SetPos(position);
                horTextboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X, currentCenter.Y-16);
            tyu = 0;
            for (int i = 0; i<countOfX/2; ++i)
            {
                tyu-=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.X-=LengthOneDividePX;
                textbox.SetPos(position);
                horTextboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X+16, currentCenter.Y);
            tyu = 0;
            for (int i = 0; i<countofY/2; ++i)
            {
                tyu+=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.Y-=LengthOneDividePX;
                textbox.SetPos(position);
                verTextboxes.Add(new Textbox(textbox));
            }
            position = new(currentCenter.X+16, currentCenter.Y);
            tyu = 0;
            for (int i = 0; i<countofY/2; ++i)
            {
                tyu-=LengthOneDivideAnalog;
                textbox.SetString(tyu.ToString());
                position.Y+=LengthOneDividePX;
                textbox.SetPos(position);
                verTextboxes.Add(new Textbox(textbox));
            }
            arrowX = new(new Vector2f(currentWidth, currentCenter.Y), 1);
            arrowY = new Arrow(new Vector2f(currentCenter.X, 0), 0);
        }
        public static Vector2f PixelToAnalogCoords(Vector2f coords) => new Vector2f((coords.X-CurrentCenter.X)/LengthOneDividePX, (coords.Y-CurrentCenter.Y)/LengthOneDividePX);
        public override void Move(float deltaX, float deltaY)
        {
            arrowX.Position = new Vector2f(arrowX.Position.X, arrowX.Position.Y+deltaY);
            arrowY.Position = new Vector2f(arrowY.Position.X+deltaX, arrowY.Position.Y);
            for (int i = 0; i<vertLines.Count; ++i)
            {
                vertLines[i].Point1.Position = new(vertLines[i].Point1.Position.X+deltaX, vertLines[i].Point1.Position.Y);
                vertLines[i].Point2.Position = new(vertLines[i].Point2.Position.X+deltaX, vertLines[i].Point2.Position.Y);
            }
            for (int i = 0; i<horizonLines.Count; ++i)
            {
                horizonLines[i].Point1.Position = new(horizonLines[i].Point1.Position.X, horizonLines[i].Point1.Position.Y+deltaY);
                horizonLines[i].Point2.Position = new(horizonLines[i].Point2.Position.X, horizonLines[i].Point2.Position.Y+deltaY);
            }
            for (int i = 0; i<verTextboxes.Count; ++i)
            {
                Vector2f pos = verTextboxes[i].GetPosition();
                verTextboxes[i].SetPos(pos.X+deltaX, pos.Y+deltaY);
            }
            for (int i = 0; i<horTextboxes.Count; ++i)
            {
                Vector2f pos = horTextboxes[i].GetPosition();
                horTextboxes[i].SetPos(pos.X+deltaX, pos.Y+deltaY);
            }
            #region correctLines
            if (deltaX<0)
            {
                Line? line = (from lin in vertLines
                              where lin.LineColor!=Color.Black && lin.Point1.Position.X<=LeftBoard
                              select lin).FirstOrDefault();

                Line? rightestLine = vertLines.FindAll(x=>x.LineColor!=Color.Black).MaxBy(x => x.Point1.Position.X);

                if (line is not null && rightestLine is not null)
                {
                    line.Point1.Position = new(rightestLine.Point1.Position.X+LengthOneDividePX, line.Point1.Position.Y);
                    line.Point2.Position = new(rightestLine.Point2.Position.X+LengthOneDividePX, line.Point2.Position.Y);
                }
            }
            else
            {
                Line? line = (from lin in vertLines
                              where lin.LineColor!=Color.Black && lin.Point1.Position.X>=CurrentWidth
                              select lin).FirstOrDefault();
                
                Line? leftestLine = vertLines.FindAll(x => x.LineColor!=Color.Black).MinBy(x => x.Point1.Position.X);

                if (line is not null && leftestLine is not null)
                {
                    line.Point1.Position = new(leftestLine.Point1.Position.X-LengthOneDividePX, line.Point1.Position.Y);
                    line.Point2.Position = new(leftestLine.Point2.Position.X-LengthOneDividePX, line.Point2.Position.Y);
                }
            }
            if (deltaY<0)
            {
                Line? line = (from lin in horizonLines
                              where lin.LineColor!=Color.Black && lin.Point1.Position.Y<=0
                              select lin).FirstOrDefault();

                Line? lowestLine = horizonLines.FindAll(x => x.LineColor!=Color.Black).MaxBy(x => x.Point1.Position.Y);

                if (line is not null && lowestLine is not null)
                {
                    line.Point1.Position = new(line.Point1.Position.X, lowestLine.Point1.Position.Y+LengthOneDividePX);
                    line.Point2.Position = new(line.Point2.Position.X, lowestLine.Point2.Position.Y+LengthOneDividePX);
                }
            }
            else
            {
                Line? line = (from lin in horizonLines
                              where lin.LineColor!=Color.Black && lin.Point1.Position.Y>=CurrentHeight
                              select lin).FirstOrDefault();

                Line? highestLine = horizonLines.FindAll(x => x.LineColor!=Color.Black).MinBy(x => x.Point1.Position.Y);

                if (line is not null && highestLine is not null)
                {
                    line.Point1.Position = new(line.Point1.Position.X, highestLine.Point1.Position.Y-LengthOneDividePX);
                    line.Point2.Position = new(line.Point2.Position.X, highestLine.Point2.Position.Y-LengthOneDividePX);
                }
            }
            #endregion
            #region correctText
            if (deltaX>0)
            {
                Textbox? textbox = (from textb in horTextboxes
                                    where textb.GetPosition().X>CurrentWidth
                                    select textb).FirstOrDefault();
                Textbox? leftTest = horTextboxes.MinBy(x => x.GetPosition().X);
                if (textbox is not null && leftTest is not null)
                {
                    textbox.SetPos(leftTest.GetPosition().X-LengthOneDividePX, textbox.GetPosition().Y);
                    textbox.SetString($"{Int32.Parse(leftTest.GetString())-1}");
                }
                if (verTextboxes[0].GetPosition().X>CurrentWidth-40)
                    verTextboxes.ForEach(x => x.SetPos(CurrentWidth-40, x.GetPosition().Y)); 
                else if(vertLines[0].Point1.Position.X>=verTextboxes[0].GetPosition().X+16)
                {
                    verTextboxes.ForEach(x => x.SetPos(vertLines[0].Point1.Position.X+16, x.GetPosition().Y));
                }
                else if(vertLines[0].Point1.Position.X<LeftBoard+24)
                    verTextboxes.ForEach(x => x.SetPos(LeftBoard+40, x.GetPosition().Y));
            }
            else
            {
                Textbox? textbox = (from textb in horTextboxes
                                    where textb.GetPosition().X<LeftBoard
                                    select textb).FirstOrDefault();
                Textbox? rightTest = horTextboxes.MaxBy(x => x.GetPosition().X);
                if (textbox is not null && rightTest is not null)
                {
                    textbox.SetPos(rightTest.GetPosition().X+LengthOneDividePX, textbox.GetPosition().Y);
                    textbox.SetString($"{Int32.Parse(rightTest.GetString())+1}");
                }
                if (verTextboxes[0].GetPosition().X<LeftBoard+40)
                    verTextboxes.ForEach(x => x.SetPos(LeftBoard+40, x.GetPosition().Y));
                else if (vertLines[0].Point1.Position.X<=verTextboxes[0].GetPosition().X-16 )
                {
                    verTextboxes.ForEach(x => x.SetPos(vertLines[0].Point1.Position.X+16, x.GetPosition().Y));
                }
                else if (vertLines[0].Point1.Position.X>CurrentWidth-74)
                    verTextboxes.ForEach(x => x.SetPos(CurrentWidth-40, x.GetPosition().Y));
            }
            if(deltaY>0)
            {
                Textbox? textbox = (from textb in verTextboxes
                                    where textb.GetPosition().Y>CurrentHeight
                                    select textb).FirstOrDefault();
                Textbox? highest = verTextboxes.MinBy(x => x.GetPosition().Y);
                if (textbox is not null && highest is not null)
                {
                    textbox.SetPos(textbox.GetPosition().X, highest.GetPosition().Y-LengthOneDividePX);
                    textbox.SetString($"{Int32.Parse(highest.GetString())+1}");
                    if(textbox.GetString()=="0")
                    {
                        textbox.SetPos(textbox.GetPosition().X, highest.GetPosition().Y-LengthOneDividePX*2);
                        textbox.SetString("1");
                    }
                }
                if (horTextboxes[0].GetPosition().Y>CurrentHeight-40)
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, CurrentHeight-40));
                else if(horizonLines[0].Point1.Position.Y>40)
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, horizonLines[0].Point1.Position.Y-16));
                else if (horizonLines[0].Point1.Position.Y<40)
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, 40));
            }
            else
            {
                Textbox? textbox = (from textb in verTextboxes
                                    where textb.GetPosition().Y<0
                                    select textb).FirstOrDefault();
                Textbox? lowest = verTextboxes.MaxBy(x => x.GetPosition().Y);
                if (textbox is not null && lowest is not null)
                {
                    textbox.SetPos(textbox.GetPosition().X, lowest.GetPosition().Y+LengthOneDividePX);
                    textbox.SetString($"{Int32.Parse(lowest.GetString())-1}");
                    if (textbox.GetString()=="0")
                    {
                        textbox.SetPos(textbox.GetPosition().X, lowest.GetPosition().Y+LengthOneDividePX*2);
                        textbox.SetString("-1");
                    }
                }
                if (horTextboxes[0].GetPosition().Y<40 )
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, 40));
                else if (horizonLines[0].Point1.Position.Y<=CurrentHeight-24)
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, horizonLines[0].Point1.Position.Y-16));
                else if (horizonLines[0].Point1.Position.Y>CurrentHeight-24)
                    horTextboxes.ForEach(x => x.SetPos(x.GetPosition().X, CurrentHeight-40));
            }
            #endregion
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 1; i<horizonLines.Count; ++i)
                target.Draw(horizonLines[i], states);
            for (int i = 1; i<vertLines.Count; ++i)
                target.Draw(vertLines[i], states);
            target.Draw(horizonLines[0], states);
            target.Draw(vertLines[0], states);
            for (int i = 0; i<verTextboxes.Count; ++i)
                target.Draw(verTextboxes[i], states);
            for (int i = 0; i<horTextboxes.Count; ++i)
                target.Draw(horTextboxes[i], states);

            target.Draw(arrowX);
            target.Draw(arrowY);
        }
    }
}
