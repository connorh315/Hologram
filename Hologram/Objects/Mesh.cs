using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Hologram.Rendering;

namespace Hologram.Objects
{
    public class Mesh
    {
        public string Name = "ROAR";

        public Vector3[] Vertices;
        public int VertexCount => Vertices.Length;

        public Face[] Faces;
        public int FaceCount => Faces.Length;

        public FaceType Type;

        public Mesh(uint vertexCount, uint faceCount, FaceType type)
        {
            Vertices = new Vector3[vertexCount];
            Faces = new Face[faceCount];
            Type = type;
        }

        private ushort[] vertexIndex;
        private VertexPosNorm[] vertices;

        private void Triangulate()
        {
            vertexIndex = new ushort[FaceCount * (Type == FaceType.Quads ? 6 : 3)]; // Quads are two triangles, so will have double the vertices.
            vertices = new VertexPosNorm[VertexCount];

            int pos = 0;

            for (int i = 0; i < FaceCount; i++)
            {
                Face face = Faces[i];

                Vector3 vert1 = Vertices[face.vert1];
                Vector3 vert2 = Vertices[face.vert2];
                Vector3 vert3 = Vertices[face.vert3];

                Vector3 normal = Vector3.Cross(vert2 - vert1, vert3 - vert1);

                vertexIndex[pos] = face.vert1;
                vertexIndex[pos + 1] = face.vert2;
                vertexIndex[pos + 2] = face.vert3;

                vertices[face.vert1].Normal += normal;
                vertices[face.vert2].Normal += normal;
                vertices[face.vert3].Normal += normal;

                if (Type == FaceType.Quads)
                {
                    vertexIndex[pos + 3] = face.vert1;
                    vertexIndex[pos + 4] = face.vert3;
                    vertexIndex[pos + 5] = face.vert4;

                    vertices[face.vert4].Normal += normal;

                    pos += 6;
                }
                else
                {
                    pos += 3;
                }
            }

            for (int i = 0; i < VertexCount; i++)
            {
                vertices[i].Position = Vertices[i];
                vertices[i].Normal.Normalize();
            }
        }

        private int vertexBuffer;
        private int vertexArray;
        private int indexBuffer;

        public void Setup()
        {
            Triangulate();

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPosNorm.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

            vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, 12);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndex.Length * 2, vertexIndex, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, vertexIndex.Length, DrawElementsType.UnsignedShort, 0);
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

    public enum FaceType
    {
        Triangles = 0,
        Quads
    }
}
