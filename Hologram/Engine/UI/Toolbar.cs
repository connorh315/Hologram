using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
{
    public class Toolbar : UIElement
    {
        public override Shader Shader => UIDefaults.QuadShader;
        public override Shader HoverShader => UIDefaults.QuadShader;

        public Color4 BackgroundColor = new Color4(20, 20, 20, 255);

        public List<RenderableString> Options = new List<RenderableString>();

        public Toolbar(int x, int y, int z, int width, int height) : base(x, y, z, width, height)
        {

        }

        public ushort XOffset = 5;

        public void AddOption(UIManager manager, string title)
        {
            float yPos = YPos + (YScale / 2) - (UIDefaults.Poppins.Size / 2f);
            RenderableString text = new RenderableString(title, UIDefaults.Poppins, (int)XPos + XOffset + 5, (int)(YPos + (YScale/2) - (UIDefaults.Poppins.Size / 2f)), (int)ZPos + 1, UIDefaults.Poppins.Size);
            Options.Add(text);
            manager.AddElement(text);

            XOffset += (ushort)(text.Width + 5);
        }

        private void DrawColor(Color4 col)
        {
            GL.UniformMatrix4(Shader.GetUniformLocation("model"), false, ref modelMatrix);
            GL.Uniform4(Shader.GetUniformLocation("quadColor"), col);

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
    }
}
