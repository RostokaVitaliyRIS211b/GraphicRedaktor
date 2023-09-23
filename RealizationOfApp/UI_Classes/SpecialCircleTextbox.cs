using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.UI_Classes
{
    public class SpecialCircleTextbox : Drawable
    {
        public bool isTextVisible = true;
        protected CircleShape circle;
        protected Text text;
        protected Font font;
        public SpecialCircleTextbox()
        {
            circle = new();
            font = new(Directory.GetCurrentDirectory()+"/Impact.ttf");
            text = new(String.Empty, font);
        }
        public SpecialCircleTextbox(CircleShape circle, Text text, Font font)
        {
            this.circle=circle;
            this.text=text;
            this.font=font;
        }
        public SpecialCircleTextbox(in SpecialCircleTextbox circleTextbox)
        {
            circle = new();
            font = circleTextbox.font;
            text = new(circleTextbox.GetString(), font);
            SetCharacterSize(circleTextbox.GetText().CharacterSize);
            SetString(circleTextbox.GetString());
            SetOutlineThicknessText(circleTextbox.GetText().OutlineThickness);
            SetRadius(circleTextbox.GetRadius());
            SetOutlineThicknessCircle(circleTextbox.GetOutlineCircle());
            SetOutlineColorCircle(circleTextbox.GetOutlineColorCircle());
            SetOutlineColorText(circleTextbox.GetText().OutlineColor);
            SetFillColorCircle(circleTextbox.GetFillColorCircle());
            SetFillColorText(circleTextbox.GetText().FillColor);
            SetPosition(circleTextbox.GetPosition().X, circleTextbox.GetPosition().Y);
        }
        public void SetFillColorCircle(Color color)
        {
            circle.FillColor = color;
        }
        public void SetFillColorText(Color color)
        {
            text.FillColor = color;
        }
        public void SetOutlineColorCircle(Color color)
        {
            circle.OutlineColor = color;
        }
        public void SetOutlineColorText(Color color)
        {
            text.OutlineColor = color;
        }
        public void SetOutlineThicknessCircle(float thickness)
        {
            circle.OutlineThickness = thickness;
        }
        public void SetOutlineThicknessText(float thickness)
        {
            text.OutlineThickness = thickness;
        }
        public void SetRadius(float radius)
        {
            circle.Radius = radius;
            circle.Origin = new Vector2f(radius, radius);
        }
        public void SetCharacterSize(uint size)
        {
            text.CharacterSize = size;
            text.Origin = new Vector2f(text.GetGlobalBounds().Width / 2f, text.GetGlobalBounds().Height);
        }
        public void SetString(string text)
        {
            this.text.DisplayedString = text;
            this.text.Origin = new Vector2f(this.text.GetGlobalBounds().Width / 2f, this.text.GetGlobalBounds().Height);
        }
        public string GetString()
        {
            return text.DisplayedString;
        }
        public float GetRadius()
        {
            return circle.Radius;
        }
        public void GetPosition(out float x, out float y)
        {
            x = circle.Position.X;
            y = circle.Position.Y;
        }
        public Vector2f GetPosition()
        {
            return circle.Position;
        }
        public void GetPosition(out Vector2f vector2F)
        {
            vector2F = circle.Position;
        }
        public float GetOutlineCircle()
        {
            return circle.OutlineThickness;
        }
        public Color GetFillColorCircle()
        {
            return circle.FillColor;
        }
        public Color GetOutlineColorCircle()
        {
            return circle.OutlineColor;
        }
        public void SetPosition(Vector2f pos)
        {
            //su
            text.Origin = new Vector2f(text.GetGlobalBounds().Width / 2f, text.GetGlobalBounds().Height);
            circle.Position = pos;
            text.Position = new Vector2f(pos.X,pos.Y-10);
            //su
        }
        public void SetPosition(float x, float y)
        {
            //su
            text.Origin = new Vector2f(text.GetGlobalBounds().Width / 2f, text.GetGlobalBounds().Height);
            circle.Position = new Vector2f(x, y);
            text.Position = new Vector2f(x, y-10);
            //su
        }
        public bool Contains(float x, float y)
        {
            return circle.GetGlobalBounds().Contains(x, y);
        }
        public bool Contains(Vector2f vector2F)
        {
            return circle.GetGlobalBounds().Contains(vector2F.X, vector2F.Y);
        }
        public bool Contains(Vector2i vector2I)
        {
            return circle.GetGlobalBounds().Contains(vector2I.X, vector2I.Y);
        }
        public Text GetText()
        {
            return text;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(circle);
            if(isTextVisible)
            target.Draw(text);
        }
    }
}
