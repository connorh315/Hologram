using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Engine.UI
{
    public abstract class BaseUI
    {
        private static bool BuiltQuad = false;

        protected static int vertexArray;

        private static void BuildQuad()
        {
            float[] quadUVs = new float[]
            {
                0f, 0f,
                1f, 1f,
                1f, 0f,
                0f, 1f,
                1f, 1f,
                0f, 0f
            };

            vertexArray = GL.GenVertexArray();
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * quadUVs.Length, quadUVs, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            BuiltQuad = true;
        }

        protected Matrix4 modelMatrix = Matrix4.Identity;

        public BaseUI(int x, int y, int width, int height)
        {
            if (!BuiltQuad)
                BuildQuad();

            modelMatrix.M11 = width;
            modelMatrix.M22 = height;
            modelMatrix.M41 = x;
            modelMatrix.M42 = y;
        }

        public abstract void Draw();
    }
}
