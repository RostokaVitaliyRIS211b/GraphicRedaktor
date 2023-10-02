using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class EnterTextPoint : EventDrawable
    {
        public Textbox textbox = new();
        protected Clock clock = new();
        protected Point point;
        public EnterTextPoint(Vector2f position, Point poin)
        {
            textbox.SetColorText(Color.Black);
            textbox.SetSizeCharacterText(15);
            textbox.SetString("");
            textbox.SetPos(position.X,position.Y-15);
            point = poin;
        }
        public override void KeyPressed(object? source, KeyEventArgs e)
        {
            if (IsAlive && e.Code!=Keyboard.Key.Enter)
            {
                textbox.SetString(SomeFuncs.ConvertToString(textbox.GetString(), e.Code));
            }
            else if (IsAlive && e.Code==Keyboard.Key.Enter && source is Application app)
            {
                IsAlive = false;
                string oldName = point.Name, newName = textbox.GetString()=="" || textbox.GetString() is null ? Point.Counter.ToString() : textbox.GetString();
                if (app.eventDrawables.Find(x => 
                {
                    Point? u = x as Point;
                    return u is Point && u?.Name==newName;
                }) is null)
                {
                    point.Name = newName;
                    app.isCanResetString = true;
                    app.SetString("Режим: Создать точку");
                    app.isCanResetString = false;
                    app.messageBox.SetPos(100, 30);
                }
                else
                {
                    app.isCanResetString = true;
                    app.SetString("Такое имя уже существует");
                    app.isCanResetString = false;
                    app.messageBox.SetPos(100, 30);
                }
                IsNeedToRemove = true;
            }
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            Vector2f t = new(textbox.GetText().GetGlobalBounds().Left+textbox.GetText().GetGlobalBounds().Width+1, textbox.GetText().GetGlobalBounds().Top-2);
            Vector2f d = new(textbox.GetText().GetGlobalBounds().Left+textbox.GetText().GetGlobalBounds().Width+1, textbox.GetText().GetGlobalBounds().Top+15);
            textbox.Draw(target, states);
            if (clock.ElapsedTime.AsSeconds()>1 && clock.ElapsedTime.AsSeconds()<2)
            {
                target.Draw(new Vertex[] { new Vertex(t, Color.Black), new Vertex(d, Color.Black) }, PrimitiveType.Lines, states);
            }
            else if (clock.ElapsedTime.AsSeconds()>2)
                clock.Restart();
            textbox.Draw(target, states);
        }
    }
}
