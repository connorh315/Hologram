using ModLib;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Rendering
{
    public class Texture
    {
        public int Handle { get; private set; }

        public string Name;
        
        public Texture()
        {
            Handle = GL.GenTexture();
            
            Use();
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        private static Texture missingTexture;

        private static Texture problemTexture;

        private static Texture whiteTexture;

        private static Texture SquareFactory(byte[] byteStream)
        {
            Texture tex = new Texture();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, InternalFormat.CompressedRgbS3tcDxt1Ext, 4, 4, 0, 8, byteStream);

            return tex;
        }

        public static Texture MissingTexture 
        { 
            get 
            {
                if (missingTexture != null) return missingTexture;

                missingTexture = SquareFactory(new byte[] { 0x1f, 0xe0, 0x00, 0x00, 0x50, 0x50, 0x05, 0x05 }); // pink and black
                return missingTexture;
            } 
        }

        public static Texture ProblemTexture
        {
            get
            {
                if (problemTexture != null) return problemTexture;

                problemTexture = SquareFactory(new byte[] { 0x00, 0xf8, 0xe0, 0x4f, 0x50, 0x50, 0x05, 0x05 });
                return problemTexture;
            }
        }

        public static Texture WhiteTexture
        {
            get
            {
                if (whiteTexture != null) return whiteTexture;

                whiteTexture = SquareFactory(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00 });
                return whiteTexture;
            }
        }
    }
}
