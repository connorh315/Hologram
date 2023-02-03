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

        private UIElement? GetHovered(Vector2i mouse)
        {
            Vector4 BL = new Vector4(0, 0, 0, 1);
            Vector4 TR = new Vector4(1, 1, 0, 1);

            UIElement bestFit = null;

            for (int i = 0; i < Elements.Count; i++)
            {
                UIElement thisElement = Elements[i];

                Vector4 result = thisElement.GetModelMatrix() * BL;
                if (mouse.X < result.X || mouse.Y < result.Y) continue;
                result = thisElement.GetModelMatrix() * TR;
                if (mouse.X > result.X || mouse.Y > result.Y) continue;

                bestFit = thisElement;
            }

            return bestFit;
        }

        private UIElement? GetHovered2(Vector2i mouse)
        {
            Shader shader = UIDefaults.ButtonShader;

            ShaderManager.Use(shader);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UniformMatrix4(shader.GetUniformLocation("projection"), false, ref projection);

            GL.BindVertexArray(UIElement.QuadArray);

            int color = shader.GetUniformLocation("buttonColor");

            for (int i = 0; i < Elements.Count; i++)
            {
                int id = i + 1;
                Matrix4 modelMatrix = Elements[i].GetModelMatrix();
                GL.UniformMatrix4(shader.GetUniformLocation("model"), false, ref modelMatrix);
                GL.Uniform4(color, new Vector4((((id) & 0xff0000) >> 16) / 255f, (((id) & 0xff00) >> 8) / 255f, ((id) & 0xff) / 255f, 255f));
                GL.Uniform1(shader.GetUniformLocation("radius"), 16f);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

            GL.UseProgram(0);
            GL.Flush();
            GL.Finish();

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            byte[] pixel = new byte[4];
            GL.ReadPixels(mouse.X, mouse.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);

            if ((pixel[2] == 0) && (pixel[1] == 0) && (pixel[0] == 0)) return null;

            int elementId = ((pixel[0] * 65536) + (pixel[1] * 256) + pixel[2]) - 1;

            return Elements[elementId];
        }

        private Vector2i previousMousePos = new Vector2i(-1, -1);
        private UIElement? previousHovered;
        public override void OnMouseOver(Vector2 mouse)
        {
            Vector2i clean = new Vector2i((int)mouse.X, (int)mouse.Y);
            if (clean == previousMousePos) return; // Need a dirty flag for when UI has been rebuilt. Something for later though
            previousMousePos = clean;

            UIElement? hovered = GetHovered2(clean);
            
            if (hovered != previousHovered)
            {
                hovered?.OnMouseEnter();
                previousHovered?.OnMouseLeave();
            }

            previousHovered = hovered;
        }

        public UIRenderer(int width, int height) : base(width, height) 
        {
            Elements = new List<Button>();

            Button test = new Button(100, 0, 300, 150, "");
            Elements.Add(test);
        }
    }
}
