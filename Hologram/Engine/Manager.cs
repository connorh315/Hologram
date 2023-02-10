﻿using Hologram.Rendering;
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

        public abstract void OnMouseEntered(Vector2 mouse);

        public abstract void OnMouseExited(Vector2 mouse);

        public abstract void OnMousePressed(HologramMouse mouse);

        public abstract void OnMouseDown(HologramMouse mouse);
        
        public abstract void OnMouseReleased(HologramMouse mouse);
        
        
        
        public Manager(int width, int height)
        {
            Width = width;
            Height = height;

            RebuildMatrix();
        }
    }
}
