using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Objects;

public class Line
{
    public LineDefinition Definition;

    private int lineBuffer, lineArray;

    public void Setup()
    {
        // Lines only
        lineBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, lineBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4, ref Definition, BufferUsageHint.StaticDraw);

        lineArray = GL.GenVertexArray();
        GL.BindVertexArray(lineArray);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);
    }

    public void Draw()
    {
        GL.BindVertexArray(lineArray);
        GL.DrawArrays(PrimitiveType.Lines, 0, 2);
    }
}

public struct LineDefinition
{
    public Vector3 Start;
    public Vector3 End;
}
