using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Rendering;

namespace Hologram.Engine.UI
{
    public class Button : UIElement
    {
        public override Shader Shader => UIDefaults.ButtonShader;
        public override Shader HoverShader => UIDefaults.ButtonShader;

        public string Text;
        public Color4 BackgroundColor = UIDefaults.ButtonBG;
        public Color4 ForegroundColor = UIDefaults.FG;

        public float Radius = 16f;

        public event Action Click;

        public Button(int x, int y, int z, int width, int height, string text) : base(x, y, z, width, height)
        {
            Text = text;
        }

        private void DrawColor(Color4 col)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.ButtonShader, "model"), false, ref modelMatrix);
            GL.Uniform4(GL.GetUniformLocation(UIDefaults.ButtonShader, "buttonColor"), col);
            GL.Uniform1(GL.GetUniformLocation(UIDefaults.ButtonShader, "radius"), Radius);

            GL.BindVertexArray(QuadArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        public override void Draw()
        {
            DrawColor(BackgroundColor);
        }

        public override void DrawForHover(Color4 col)
        {
            DrawColor(col);
        }

        public override void OnMouseEnter(MainWindow window)
        {
            BackgroundColor = new Color4(255, 0, 0, 255);
            window.SetCursor(CursorShape.Hand);
        }

        public override void OnMouseLeave(MainWindow window)
        {
            BackgroundColor = UIDefaults.ButtonBG;
            window.SetCursor(CursorShape.Arrow);
        }

        public override void OnClick(MainWindow window)
        {
            Click();
        }
    }
}
