using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class EnterTextTexbox : EventDrawable
    {
        public Textbox textbox = new();
        protected Clock clock = new();
        public EnterTextTexbox(Vector2f position)
        {
            textbox.SetFillColorRect(Color.White);
            textbox.SetSizeRect(200, 50);
            textbox.SetOutlineThicknessRect(3);
            textbox.SetOutlineColorRect(Color.Black);
            textbox.SetColorText(Color.Black);
            textbox.SetSizeCharacterText(15);
            textbox.SetString("");
            textbox.SetPos(position.X, position.Y-15);
        }
        public override void KeyPressed(object? source, KeyEventArgs e)
        {
            float outpar = 0;
            if (IsAlive && e.Code!=Key.Enter)
            {
                textbox.SetString(SomeFuncs.ConvertToString(textbox.GetString(), e.Code));
            }
            else if (IsAlive && e.Code==Key.Enter && source is Application app && Single.TryParse(textbox.GetString(),out outpar))
            {
                IsAlive = false;
            }
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            Vector2f t = new(textbox.GetText().GetGlobalBounds().Left+textbox.GetText().GetGlobalBounds().Width+1, textbox.GetText().GetGlobalBounds().Top-2);
            Vector2f d = new(textbox.GetText().GetGlobalBounds().Left+textbox.GetText().GetGlobalBounds().Width+1, textbox.GetText().GetGlobalBounds().Top+15);
            target.Draw(textbox, states);
            if (clock.ElapsedTime.AsSeconds()>1 && clock.ElapsedTime.AsSeconds()<2)
            {
                target.Draw(new Vertex[] { new Vertex(t, Color.Black), new Vertex(d, Color.Black) }, PrimitiveType.Lines, states);
            }
            else if (clock.ElapsedTime.AsSeconds()>2)
                clock.Restart();
        }
    }
}
