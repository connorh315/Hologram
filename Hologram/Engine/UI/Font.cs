using Hologram.Rendering;
using ModLib;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
{
    public class Font
    {
        private byte size;
        private byte paddingLeft;
        private byte paddingTop;
        private byte paddingRight;
        private byte paddingBottom;
        private short spacingX;
        private short spacingY;
        private byte stretchHeight;
        private short lineHeight;
        private byte baseNum;
        private short scaleWidth;
        private short scaleHeight;

        public Dictionary<byte, FontChar> Chars = new Dictionary<byte, FontChar>(256);

        public Texture Texture;

        public Font(string cfntName)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fileLocation = Path.Combine(dir, "Resources", "Fonts", (cfntName + ".cfnt"));
            if (!File.Exists(fileLocation))
            {
                Logger.Error($"Could not find font file ({cfntName}) from {fileLocation} - File does not exist!");
                throw new Exception();
            }

            Logger.Log($"Loading font: {cfntName} from {fileLocation}");

            using (ModFile file = ModFile.Open(fileLocation))
            {
                size = file.ReadByte();
                paddingLeft = file.ReadByte();
                paddingTop = file.ReadByte();
                paddingRight = file.ReadByte();
                paddingBottom = file.ReadByte();
                spacingX = file.ReadShort(true);
                spacingY = file.ReadShort(true);
                stretchHeight = file.ReadByte();
                lineHeight = file.ReadShort(true);
                baseNum = file.ReadByte();
                scaleWidth = file.ReadShort(true);
                scaleHeight = file.ReadShort(true);

                ushort charCount = file.ReadUshort(true);

                for (int i = 0; i < charCount; i++)
                {
                    byte id = file.ReadByte();
                    FontChar thisChar = new FontChar()
                    {
                        X = file.ReadShort(true),
                        Y = file.ReadShort(true),
                        Width = file.ReadShort(true),
                        Height = file.ReadShort(true),
                        XOffset = file.ReadShort(true),
                        YOffset = file.ReadShort(true),
                        XAdvance = file.ReadShort(true)
                    };
                    Chars.Add(id, thisChar);
                }

                int width = file.ReadInt(true);
                int height = file.ReadInt(true);
                byte[] buffer = new byte[file.ReadInt(true)];

                byte runByte = file.ReadByte();

                for (int i = 0; i < buffer.Length; i++)
                {
                    byte read = file.ReadByte();
                    if (read == runByte)
                    {
                        ushort runLength = file.ReadUshort(true);
                        byte byteToWrite = file.ReadByte();

                        for (ushort runPos = 0; runPos < runLength; runPos++)
                        {
                            buffer[i + runPos] = byteToWrite;
                        }

                        i += runLength - 1;
                    } 
                    else
                    {
                        buffer[i] = read;
                    }
                }

                Texture = new Texture();

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, buffer);
            }
        }
    }

    public struct FontChar
    {
        public byte Id;
        public short X;
        public short Y;
        public short Width;
        public short Height;
        public short XOffset;
        public short YOffset;
        public short XAdvance;
    }
}
