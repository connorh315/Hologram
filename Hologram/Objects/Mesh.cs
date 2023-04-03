using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Rendering;

namespace Hologram.Objects;

public class Mesh
{
    public string Name = "ROAR";

    public Vector3[] Vertices;
    public int VertexCount => Vertices.Length;

    public Face[] Faces;
    public int FaceCount => Faces.Length;

    public FaceType Type;

    public Mesh(int vertexCount, int faceCount, FaceType type)
    {
        Vertices = new Vector3[vertexCount];
        Faces = new Face[faceCount];
        Type = type;
    }

    private ushort[] vertexIndex;
    private VertexPosNorm[] vertices;

    public VertexPosNormCol[] vertices2;

    private Line[] lines;

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

    private void TriangulateColor()
    {
        vertexIndex = new ushort[FaceCount * (Type == FaceType.Quads ? 6 : 3)]; // Quads are two triangles, so will have double the vertices.
        vertices2 = new VertexPosNormCol[VertexCount];
        lines = new Line[VertexCount];

        int pos = 0;

        for (int i = 0; i < FaceCount; i++)
        {
            Face face = Faces[i];

            Vector3 vert1 = Vertices[face.vert1];
            Vector3 vert2 = Vertices[face.vert2];
            Vector3 vert3 = Vertices[face.vert3];

            Vector3 normal = Vector3.Cross(vert2 - vert1, vert3 - vert1).Normalized();

            Faces[i].normal = normal;

            vertexIndex[pos] = face.vert1;
            vertexIndex[pos + 1] = face.vert2;
            vertexIndex[pos + 2] = face.vert3;

            vertices2[face.vert1].Normal += normal;
            vertices2[face.vert2].Normal += normal;
            vertices2[face.vert3].Normal += normal;

            //Color4 col = new Color4((face.vert5 & 256) * 255, 0, 0, 255);
            Color4 col = new Color4(255, 255, 255, 255);

            vertices2[face.vert1].Color = col;
            vertices2[face.vert2].Color = col;
            vertices2[face.vert3].Color = col;
            vertices2[face.vert4].Color = col;

            if (Type == FaceType.Quads)
            {
                vertexIndex[pos + 3] = face.vert1;
                vertexIndex[pos + 4] = face.vert3;
                vertexIndex[pos + 5] = face.vert4;

                vertices2[face.vert4].Normal += normal;

                pos += 6;
            }
            else
            {
                pos += 3;
            }
        }

        for (int i = 0; i < VertexCount; i++)
        {
            vertices2[i].Position = Vertices[i];
            vertices2[i].Normal.Normalize();

            Line line = new Line();
            line.Definition.Start = Vertices[i];
            line.Definition.End = line.Definition.Start + (vertices2[i].Normal * 0.1f);
            lines[i] = line;
            line.Setup();
        }

        Face[] oldFaces = Faces;
        Faces = new Face[Faces.Length * 2];
        for (int i = 0; i < oldFaces.Length; i++)
        {
            Faces[i * 2].vert1 = oldFaces[i].vert1;
            Faces[i * 2].vert2 = oldFaces[i].vert2;
            Faces[i * 2].vert3 = oldFaces[i].vert3;
            Faces[i * 2].normal = oldFaces[i].normal;

            Faces[i * 2 + 1].vert1 = oldFaces[i].vert1;
            Faces[i * 2 + 1].vert2 = oldFaces[i].vert3;
            Faces[i * 2 + 1].vert3 = oldFaces[i].vert4;
            Faces[i * 2 + 1].normal = oldFaces[i].normal;

        }
    }

    private int vertexBuffer;
    private int vertexArray;
    private int indexBuffer;

    public void Setup()
    {
        TriangulateColor();

        vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * VertexPosNormCol.SizeInBytes, vertices2, BufferUsageHint.StaticDraw);

        vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(vertexArray);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 40, 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 40, 12);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 40, 24);

        indexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndex.Length * 2, vertexIndex, BufferUsageHint.StaticDraw);

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].Setup();
        }
    }

    public void Draw()
    {
        GL.BindVertexArray(vertexArray);
        GL.DrawElements(PrimitiveType.Triangles, vertexIndex.Length, DrawElementsType.UnsignedShort, 0);
    }

    public void DrawLines()
    {
        GL.LineWidth(2);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].Draw();
        }
    }
}

public struct Face
{
    public ushort vert1;
    public ushort vert2;
    public ushort vert3;
    public ushort vert4;
    public Vector3 normal;
}

public enum FaceType
{
    Triangles = 0,
    Quads
}
