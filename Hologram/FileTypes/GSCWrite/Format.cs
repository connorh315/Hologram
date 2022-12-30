using ModLib;
using OpenTK.Mathematics;
using Half = System.Half;

namespace Hologram.FileTypes.GSCWrite
{
    internal class Format
    {
        public static byte GetMinified(float toMinify)
        {
            return (byte)Math.Clamp((toMinify + 1) * 128, 0, 255);
        }

        public static void vec2float(ModFile file, Vector2 data)
        {
            file.WriteFloat(data.X, true); // I think it's little endian
            file.WriteFloat(data.Y, true);
        }

        public static void vec3float(ModFile file, Vector3 data)
        {
            file.WriteFloat(data.X, true);
            file.WriteFloat(data.Y, true);
            file.WriteFloat(data.Z, true);
        }

        public static void vec4float(ModFile file, Vector4 data)
        {
            file.WriteFloat(data.X, true);
            file.WriteFloat(data.Y, true);
            file.WriteFloat(data.Z, true);
            file.WriteFloat(data.W, true);
        }

        public static void vec2half(ModFile file, Vector2 data)
        {
            file.WriteHalf((Half)data.X, true);
            file.WriteHalf((Half)data.Y, true);
        }

        public static void vec4half(ModFile file, Vector4 data)
        {
            file.WriteHalf((Half)data.X, true);
            file.WriteHalf((Half)data.Y, true);
            file.WriteHalf((Half)data.Z, true);
            file.WriteHalf((Half)data.W, true);
        }

        public static void vec4half(ModFile file, Vector3 data)
        {
            file.WriteHalf((Half)data.X, true);
            file.WriteHalf((Half)data.Y, true);
            file.WriteHalf((Half)data.Z, true);
            file.WriteShort(0x3c00, true);
        }

        public static void vec4char(ModFile file, Vector4 data)
        {
            file.WriteUint(0xffffffff); // Couldn't find out what vec4char actually means
        }

        public static void vec4mini(ModFile file, Vector4 data)
        {
            file.WriteByte(GetMinified(data.X));
            file.WriteByte(GetMinified(data.Y));
            file.WriteByte(GetMinified(data.Z));
            file.WriteByte(GetMinified(data.W));
        }

        public static void vec4mini(ModFile file, Vector3 data)
        {
            file.WriteByte(GetMinified(data.X));
            file.WriteByte(GetMinified(data.Y));
            file.WriteByte(GetMinified(data.Z));
            file.WriteByte(0xff);
        }

        public static void color4char(ModFile file, Color4 data)
        {
            file.WriteByte((byte)(data.R * 255)); // High likelihood this should be BGRA
            file.WriteByte((byte)(data.G * 255));
            file.WriteByte((byte)(data.B * 255));
            file.WriteByte((byte)(data.A * 255));
        }
    }
}
