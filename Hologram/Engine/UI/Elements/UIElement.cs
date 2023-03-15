using Hologram.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Engine.UI.Elements
{
    public abstract class UIElement
    {
        public virtual Shader Shader { get { return null; } }
        public virtual Shader HoverShader { get { return null; } }

        private static bool BuiltQuad = false;

        public static int QuadArray;

        public bool Enabled = true;

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

        protected Matrix4 elementMatrix = Matrix4.Identity;

        public Matrix4 GetModelMatrix() => elementMatrix;

        public float XPos { get { return elementMatrix.M41; } set { elementMatrix.M41 = value; } }
        public float YPos { get { return elementMatrix.M42; } set { elementMatrix.M42 = value; } }
        public float ZPos { get { return elementMatrix.M43; } set { elementMatrix.M43 = value; } }
        public float XScale { get { return elementMatrix.M11; } set { elementMatrix.M11 = value; } }
        public float YScale { get { return elementMatrix.M22; } set { elementMatrix.M22 = value; } }

        public UIManager Manager;

        public UIElement Parent;

        public UIElement(UIElement parent)
        {
            if (!BuiltQuad)
                BuildQuad();

            ZPos = parent.ZPos + 1;

            Parent = parent;

            parent.AddChild(this);

            Manager.AddElement(this);
        }

        public UIElement() { }

        protected List<UIElement> children = new List<UIElement>();

        public virtual void AddChild(UIElement child)
        {
            children.Add(child);

            child.Manager = Manager;
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
            float origX = XPos;
            float origY = YPos;
            XPos = x;
            YPos = y;

            OnMove(new Vector2(origX, origY));
        }

        public virtual void SetBounds(float x, float y, float width, float height)
        {
            SetPos(x, y);
            SetSize(width, height);
        }

        /// <summary>
        /// Default draw method.
        /// </summary>
        public virtual void Draw()
        {
            foreach (UIElement element in children)
            {
                element.Draw();
            }
        }

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

        public virtual void OnMove(Vector2 originalPos)
        {

        }
    }
}
