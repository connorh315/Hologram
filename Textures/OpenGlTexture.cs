using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Textures
{
    public sealed class OpenGlTexture : IDisposable
    {
        private readonly int _handle;
        private bool _disposedValue;

        public OpenGlTexture()
        {
            _handle = GL.GenTexture();
        }

        public void LoadFromFile(string path)
        {
            List<byte> pixels = new(4 * 100 * 100);

            for (int i = 0; i < pixels.Count; i += 4)
            {
                pixels[i] = 255;
                pixels[i + 1] = 0;
                pixels[i + 2] = 0;
                pixels[i + 3] = 255;
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 100, 100, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        ~OpenGlTexture()
        {
            GL.DeleteTexture(_handle);
        }
		
        public void Dispose()
        {
            if (_disposedValue)
            {
                return;
            }

            GL.DeleteTexture(_handle);

            _disposedValue = true;
            
            GC.SuppressFinalize(this);
        }
    }
}