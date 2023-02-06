using Hologram.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine
{
    public abstract class Renderer
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            RebuildMatrix();
        }

        public MainWindow Parent { get; private set; }
        public void SetParent(MainWindow window)
        {
            Parent = window;
        }

        protected abstract void RebuildMatrix();

        public abstract void Draw();

        public abstract void OnMouseOver(Vector2 mouse);

        public abstract void OnClick();

        public Renderer(int width, int height)
        {
            Width = width;
            Height = height;

            RebuildMatrix();
        }
    }
}
