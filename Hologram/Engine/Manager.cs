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
        }

        public bool Focused { get; private set; }
        public void SetFocus(bool focused)
        {
            Focused = focused;
        }

        public bool HasFocus() => Focused;

        public MainWindow Parent { get; private set; }
        public void SetParent(MainWindow window)
        {
            Parent = window;
        }

        public abstract void Update(double deltaTime);

        public abstract void Draw();

        protected abstract void RebuildMatrix();

        public abstract void OnMouseOver(Vector2 mouse);

        public abstract void OnMouseEntered(Vector2 mouse);

        public abstract void OnMouseLeave(Vector2 mouse);

        public abstract void OnMousePress(HologramMouse mouse);

        public abstract void OnMouseDown(HologramMouse mouse);
        
        public abstract void OnMouseReleased(HologramMouse mouse);
        
        
        
        public Manager(MainWindow parent, int width, int height)
        {
            Width = width;
            Height = height;

            SetParent(parent);
            RebuildMatrix();
        }
    }
}
