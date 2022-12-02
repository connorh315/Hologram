﻿using ModLib;
using OpenTK.Graphics.OpenGL;

namespace Hologram.Rendering
{
    public class Texture
    {
        int Handle;

        public Texture()
        {
            Handle = GL.GenTexture();
            
            Use();
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
