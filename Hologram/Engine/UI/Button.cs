using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Rendering;

namespace Hologram.Engine.UI
{
    public class Button : BaseUI
    {
        public string Text;
        public Color4 BackgroundColor = Color4.White;
        public Color4 ForegroundColor = UIDefaults.ForegroundColor;

        public Button(int x, int y, int width, int height, string text) : base(x, y, width, height)
        {
            Text = text;
        }

        public override void Draw()
        {
            //ShaderManager.Use(UIDefaults.ButtonShader); // needs the projection matrix so probably can't do it this way

            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.ButtonShader, "model"), false, ref modelMatrix);
            GL.Uniform4(GL.GetUniformLocation(UIDefaults.ButtonShader, "buttonColor"), BackgroundColor);
            GL.Uniform1(GL.GetUniformLocation(UIDefaults.ButtonShader, "radius"), 16f);

            GL.BindVertexArray(vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }
    }
}
