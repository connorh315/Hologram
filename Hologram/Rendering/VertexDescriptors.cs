using OpenTK.Mathematics;

namespace Hologram.Rendering;

public struct VertexPosNormCol
{
    public Vector3 Position;
    public Vector3 Normal;
    public Color4 Color;
    public VertexPosNormCol(Vector3 position, Vector3 normal, Color4 color)
    {
        Position = position;
        Normal = normal;
        Color = color;
    }

    public const int SizeInBytes = 40;
}

public struct VertexPosNorm
{
    public Vector3 Position;
    public Vector3 Normal = Vector3.Zero;
    public VertexPosNorm(Vector3 position, Vector3 normal)
    {
        Position = position;
        Normal = normal;
    }
    public const int SizeInBytes = 24;
}

//public struct Vertex
//{
//    public Vector3 Position;
//    public Vector3 Normal;
//    public Vector2 UV;
//    public Vertex(Vector3 position, Vector3 normal, Vector2 uv)
//    {
//        Position = position;
//        Normal = normal;
//        UV = uv;
//    }
//    public const int SizeInBytes = 32;
//}
