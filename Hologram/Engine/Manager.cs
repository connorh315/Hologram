using Hologram.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine
{
    public abstract class Manager
    {
        public int X = 0;
        public int Y = 0;
        public void SetPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            RebuildMatrix();
            OnResize();
        }

        public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);

        public bool Focused { get; private set; }
        public void SetFocus(bool focused)
        {
            Focused = focused;
        }

        public bool HasFocus() => Parent.Hovered == this;

        public MainWindow Parent { get; private set; }
        public void SetParent(MainWindow window)
        {
            Parent = window;
        }

        public abstract void Update(double deltaTime);

        public abstract void Draw();

        protected abstract void RebuildMatrix();

        public abstract void OnMouseEnter(Vector2 mouse);

        public abstract void OnMouseOver(Vector2 mouse);

        public abstract void OnMouseLeave(Vector2 mouse);

        public abstract void OnMousePress(HologramMouse mouse);

        public abstract void OnMouseDown(HologramMouse mouse);
        
        public abstract void OnMouseRelease(HologramMouse mouse);
        
        public virtual void OnResize()
        {

        }
        
        public Manager(MainWindow parent, int width, int height)
        {
            Width = width;
            Height = height;

            SetParent(parent);
            RebuildMatrix();
        }
    }
}
