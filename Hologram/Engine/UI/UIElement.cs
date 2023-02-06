using Hologram.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Engine.UI
{
    public abstract class UIElement
    {
        protected virtual Shader Shader { get { return null; } }

        private static bool BuiltQuad = false;

        public static int QuadArray;

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

            QuadArray = GL.GenVertexArray();
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * quadUVs.Length, quadUVs, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(QuadArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            BuiltQuad = true;
        }

        protected Matrix4 modelMatrix = Matrix4.Identity;

        public Matrix4 GetModelMatrix() => modelMatrix;

        public UIElement(int x, int y, int z, int width, int height)
        {
            if (!BuiltQuad)
                BuildQuad();

            modelMatrix.M11 = width;
            modelMatrix.M22 = height;
            modelMatrix.M41 = x;
            modelMatrix.M42 = y;
            modelMatrix.M43 = z;
        }

        public abstract void Draw();

        public virtual void OnMouseEnter(MainWindow window)
        {
            
        }

        public virtual void OnMouseLeave(MainWindow window)
        {

        }

        public virtual void OnClick(MainWindow window)
        {

        }
    }
}
