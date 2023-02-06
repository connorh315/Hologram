using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Rendering;

namespace Hologram.Engine.UI
{
    public class Button : UIElement
    {
        public string Text;
        public Color4 BackgroundColor = UIDefaults.ButtonBG;
        public Color4 ForegroundColor = UIDefaults.FG;

        public event Action Click;

        public Font Font;

        public Button(int x, int y, int width, int height, string text) : base(x, y, width, height)
        {
            Text = text;
            Font = new Font("Poppins");
        }

        public override void Draw()
        {
            //ShaderManager.Use(UIDefaults.ButtonShader); // needs the projection matrix so probably can't do it this way

            Font.Texture.Use();

            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.ButtonShader, "model"), false, ref modelMatrix);
            GL.Uniform4(GL.GetUniformLocation(UIDefaults.ButtonShader, "buttonColor"), BackgroundColor);
            GL.Uniform1(GL.GetUniformLocation(UIDefaults.ButtonShader, "radius"), 16f);

            GL.BindVertexArray(QuadArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
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
