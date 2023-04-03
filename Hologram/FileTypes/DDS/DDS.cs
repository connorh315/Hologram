using Hologram.Rendering;
using ModLib;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.FileTypes.DDS;

internal class DDS
{
    public static Texture Load(ModFile file, bool isCubemap)
    {
        if (file.Status != ModFileStatus.Success || !file.CheckString("DDS ", string.Empty)) return null;
        file.Seek(8, SeekOrigin.Current);
        int height = file.ReadInt();
        int width = file.ReadInt();
        file.Seek(8, SeekOrigin.Current);
        int mipmapCount = Math.Max(file.ReadInt(), 1);
        file.Seek(52, SeekOrigin.Current);

        string comSign = file.ReadString(4);
        file.Seek(40, SeekOrigin.Current);
        InternalFormat compressionFormat;
        int blockSize = 0;
        int maxSize = 0;
        switch(comSign)
        {
            case "DXT1":
                compressionFormat = InternalFormat.CompressedRgbaS3tcDxt1Ext;
                blockSize = 8;
                break;
            case "DXT5":
                compressionFormat = InternalFormat.CompressedRgbaS3tcDxt5Ext;
                blockSize = 16;
                break;
            case "t\0\0":
                compressionFormat = InternalFormat.Rgba32f;
                maxSize = width * height * 16;
                break;
            default:
                return null;
        }

        Texture texture = new Texture();

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, mipmapCount - 1);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        byte[] buffer = new byte[maxSize != 0 ? maxSize : CalculateSize(width, height, blockSize)]; // Build an array that is the size of the first mipmap, every subsequent mipmap will be smaller than this and as such can just re-use this.

        for (int i = 0; i < mipmapCount; i++)
        {
            if (width == 0 || height == 0)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, i - 1);
                break;
            }

            if (blockSize != 0)
            {
                int mipmapSize = CalculateSize(width, height, blockSize);
                file.fileStream.Read(buffer, 0, mipmapSize);
                GL.CompressedTexImage2D(TextureTarget.Texture2D, i, compressionFormat, width, height, 0, mipmapSize, buffer);
            }
            else
            {
                int mipmapSize = width * height * 16;
                file.fileStream.Read(buffer, 0, mipmapSize);
                GL.TexImage2D(TextureTarget.Texture2D, i, (PixelInternalFormat)compressionFormat, width, height, 0, PixelFormat.Rgba, PixelType.Float, buffer);
            }
            width /= 2;
            height /= 2;
        }

        return texture;
    }

    public static Texture Load(string fileLocation)
    {
        using (ModFile file = ModFile.Open(fileLocation))
        {
            return Load(file, false);
        }
    }

    private static int CalculateSize(int width, int height, int blockSize)
    {
        //return (width * height * blockSize)/16;
        return Math.Max(1, ((width + 3) / 4) * ((height + 3) / 4)) * blockSize;
    }
}
