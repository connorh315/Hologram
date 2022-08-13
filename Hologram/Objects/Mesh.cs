using OpenTK.Mathematics;

namespace Hologram.Objects
{
    public class Mesh
    {
        public Vector3[] Vertices;
        public int VertexCount => Vertices.Length;

        public Face[] Faces;
        public int FaceCount => Faces.Length;

        public Mesh(uint vertexCount, uint faceCount)
        {
            Vertices = new Vector3[vertexCount];
            Faces = new Face[faceCount];
        }
    }

    public struct Face
    {
        public ushort vert1;
        public ushort vert2;
        public ushort vert3;
        public ushort vert4;
        public ushort vert5;
    }
}
