using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Simplexs
{
    public class Point3D:EvObject
    {
        public static int Counter { get; protected set; } = 1;
        protected SpecialCircleTextbox circle = new();
        public bool IsCatched = false;
        public event Action<Point3D>? IGetRemoved;
        public Vector3f WorldPosition { get; set; }
        public string Name { get => circle.GetString(); set => circle.SetString(value); }
        protected float inDescreteCoordinate = 1;
        public override bool IsNeedToRemove
        {
            get => base.IsNeedToRemove;
            set
            {
                base.IsNeedToRemove = value;
                if (IsNeedToRemove)
                    IGetRemoved?.Invoke(this);
            }
        }
        public Point3D(Vector3f coords, string name)
        {
            ++Counter;
            circle = new(Shablones.GetShablone1());
            WorldPosition=coords;
            circle.SetPosition(SomeFuncs.Convert3DTo2D(coords));
            circle.SetString(name);
            BuffColor = circle.GetFillColorCircle();
        }
        public Point3D(float x, float y,float z, string name)
        {
            ++Counter;
            WorldPosition = new(x, y, z);
            circle = new(Shablones.GetShablone1());
            circle.SetPosition(SomeFuncs.Convert3DTo2D(WorldPosition));
            circle.SetString(name);
            BuffColor = circle.GetFillColorCircle();
        }
        public bool IsTextVisible { get => circle.isTextVisible; set => circle.isTextVisible=value; }
        public Color BuffColor { get; }
        public Color FillColor { get => circle.GetFillColorCircle(); set => circle.SetFillColorCircle(value); }
        public Vector2f Position { get => circle.GetPosition();}

        public bool Contains(Vector2f vector)
        {
            return circle.Contains(vector);
        }
        public override void Move(float deltaX, float deltaY)
        {
            
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
