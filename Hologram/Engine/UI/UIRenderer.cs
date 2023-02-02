using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
{
    public class UIRenderer : Renderer
    {
        public List<Button> Elements;

        private Matrix4 projection;

        protected override void RebuildMatrix()
        {
            projection = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1f, 1f);
        }

        public override void Draw()
        {
            ShaderManager.Use(UIDefaults.ButtonShader);
            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.ButtonShader, "projection"), false, ref projection);
            foreach (Button button in Elements)
            {
                button.Draw();
            }
        }

        public UIRenderer(int width, int height) : base(width, height) 
        {
            Elements = new List<Button>();

            Button test = new Button(100, 0, 300, 150, "");
            Elements.Add(test);
        }
    }
}
