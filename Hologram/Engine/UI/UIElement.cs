using Hologram.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Engine.UI
{
    public abstract class UIElement
    {
        public virtual Shader Shader { get { return null; } }
        public virtual Shader HoverShader { get { return null; } }

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

        public float XPos { get { return modelMatrix.M41; } set { modelMatrix.M41 = value; } }
        public float YPos { get { return modelMatrix.M42; } set { modelMatrix.M42 = value; } }
        public float ZPos => modelMatrix.M43;
        public float XScale { get { return modelMatrix.M11; } set { modelMatrix.M11 = value; } }
        public float YScale { get { return modelMatrix.M22; } set { modelMatrix.M22 = value; } }

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

        public virtual void SetSize(float width, float height)
        {
            float origWidth = XScale;
            float origHeight = YScale;
            XScale = width;
            YScale = height;

            OnResize(new Vector2(origWidth, origHeight));
        }

        public virtual void SetPos(float x, float y)
        {
            XPos = x;
            YPos = y;
        }

        /// <summary>
        /// Default draw method.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Method for drawing to the framebuffer when trying to find the currently hovered element.
        /// </summary>
        /// <param name="col">The id color</param>
        public abstract void DrawForHover(Color4 col);

        /// <summary>
        /// When the mouse enters this elements bounds.
        /// </summary>
        /// <param name="window"></param>
        public virtual void OnMouseEnter(MainWindow window)
        {
            
        }

        /// <summary>
        /// When the mouse stays inside this elements bounds.
        /// </summary>
        /// <param name="window"></param>
        public virtual void OnMouseOver(MainWindow window)
        {

        }

        /// <summary>
        /// When the mouse leaves this elements bounds.
        /// </summary>
        /// <param name="window"></param>
        public virtual void OnMouseLeave(MainWindow window)
        {

        }

        public virtual void OnMousePress(MainWindow window)
        {

        }

        public virtual void OnMouseDown(MainWindow window)
        {

        }

        public virtual void OnMouseRelease(MainWindow window)
        {

        }

        public virtual void OnResize(Vector2 originalSize)
        {

        }
    }
}
