using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Hologram.Rendering;

namespace Hologram.Objects
{
    public class MeshX
    {
        public string Name;

        // Deprecated
        public Color4 Color = Color4.White;

        public Vertex[] Vertices;
        public int VertexCount => Vertices.Length;
        
        public ushort[] Indices;
        public int IndicesCount => Indices.Length;

        private int vertexBuffer;
        private int vertexArray;
        private int indexBuffer;

        public void Setup()
        {
            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * Vertex.Size, Vertices, BufferUsageHint.StaticDraw);

            vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Size, 12);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, 24);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Vertex.Size, 32);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * 2, Indices, BufferUsageHint.StaticDraw);
        }

        public void Draw(Material material)
        {
            Color4 col = material.Color;
            GL.Uniform3(MainWindow.MeshColorLocation, new Vector3(col.R, col.G, col.B));

            GL.BindVertexArray(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedShort, 0);
        }
    }

    public readonly struct Vertex
    { // The readonly makes them immutable, as they should be.
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 UV;
        public readonly Color4 Color;

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv, Color4 color)
        {
            this.Position = position;
            this.Normal = normal;
            this.UV = uv;
            this.Color = color;
        }

        public const int Size = 48;
    }
}
