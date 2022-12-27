using OpenTK.Mathematics;

namespace Hologram.Extensions;

internal static class OpenTKMath
{
    public static string ToCleanString(this Color4 col)
    {
        return $"{col.R} {col.G} {col.B} {col.A}";
    }

    public static string ToCleanString(this Matrix4 mat)
    {
        mat.Transpose();
        return $"{mat.M11} {mat.M12} {mat.M13} {mat.M14} {mat.M21} {mat.M22} {mat.M23} {mat.M24} {mat.M31} {mat.M32} {mat.M33} {mat.M34} {mat.M41} {mat.M42} {mat.M43} {mat.M44}";
    }
}