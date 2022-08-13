using OpenTK.Mathematics;

namespace Hologram.Rendering
{
    struct VertexNormPosCol
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color4 Color;
        public VertexNormPosCol(Vector3 position, Vector3 normal, Color4 color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }

        public const int SizeInBytes = 40;
    }

    struct VertexPosNorm
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
}
