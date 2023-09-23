using RealizationOfApp.UI_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RealizationOfApp.Simplexs
{
    public class Point : EvObject
    { 
    
        public static int Counter { get; protected set; }
        protected SpecialCircleTextbox circle = new();
        public bool IsCatched = false;
        public event Action<Point>? IGetRemoved;
        public string Name { get => circle.GetString(); set => circle.SetString(value); }
        public override bool IsNeedToRemove
        {
            get => base.IsNeedToRemove;
            set
            {
                IsNeedToRemove = value;
                if (!IsNeedToRemove)
                    IGetRemoved?.Invoke(this);
            }
        }
        public Point(Vector2f coords, string name)
        {
            ++Counter;
            circle = new(Shablones.GetShablone1());
            circle.SetPosition(coords.X, coords.Y);
            circle.SetString(name);
        }
        public Point (float x,float y,string name)
        {
            ++Counter;
            circle = new(Shablones.GetShablone1());
            circle.SetPosition(x, y);
            circle.SetString(name);
        }
        public bool IsTextVisible { get => circle.isTextVisible; set => circle.isTextVisible=value; }
        public Color BuffColor;
        public Vector2f Position { get => circle.GetPosition(); set => circle.SetPosition(value); }

        public bool Contains(Vector2f vector)
        {
            return circle.Contains(vector);
        }
        public override bool Contains(float x, float y)
        {
            return circle.Contains(x, y);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(circle, states);
        }
    }
}
