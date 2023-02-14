using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
{
    public class UIManager : Manager
    {
        private Dictionary<Shader, List<UIElement>> interactableElements = new Dictionary<Shader, List<UIElement>>();

        private List<RenderableString> textElements = new List<RenderableString>();

        public Font Font;

        private Matrix4 projection;

        public Matrix4 GetProjectionMatrix()
        {
            return projection;
        }

        protected override void RebuildMatrix()
        {
            projection = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -10f, 10f);
        }

        public override void Draw()
        {
            foreach ((Shader shader, List<UIElement> elements) in interactableElements)
            {
                ShaderManager.Use(shader);

                GL.UniformMatrix4(shader.GetUniformLocation("projection"), false, ref projection);

                foreach (UIElement element in elements)
                {
                    element.Draw();
                }
            }

            ShaderManager.Use(UIDefaults.TextShader);
            Font.Texture.Use();
            GL.UniformMatrix4(UIDefaults.TextShader.GetUniformLocation("projection"), false, ref projection);
            
            foreach (RenderableString element in textElements)
            {
                element.Draw();
            }
        }

        public override void Update(double deltaTime)
        {
            throw new NotImplementedException();
        }

        //private UIElement? GetHovered(Vector2i mouse)
        //{
        //    Vector4 BL = new Vector4(0, 0, 0, 1);
        //    Vector4 TR = new Vector4(1, 1, 0, 1);

        //    UIElement bestFit = null;

        //    for (int i = 0; i < Elements.Count; i++)
        //    {
        //        UIElement thisElement = Elements[i];

        //        Vector4 result = thisElement.GetModelMatrix() * BL;
        //        if (mouse.X < result.X || mouse.Y < result.Y) continue;
        //        result = thisElement.GetModelMatrix() * TR;
        //        if (mouse.X > result.X || mouse.Y > result.Y) continue;

        //        bestFit = thisElement;
        //    }

        //    return bestFit;
        //}

        private UIElement? GetHovered(Vector2i mouse)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(X, Y, Width, Height);

            Shader[] shaders = interactableElements.Keys.ToArray();

            if (shaders.Length > 255) throw new Exception("Too many active shaders!");

            for (int index = 0; index < shaders.Length; index++)
            {
                List<UIElement> elements = interactableElements[shaders[index]];

                if (elements.Count == 0) continue;

                Shader hoverShader = elements[0].HoverShader;

                ShaderManager.Use(hoverShader);
                GL.UniformMatrix4(hoverShader.GetUniformLocation("projection"), false, ref projection);

                int id = 0;
                float red = (index + 1) / 255f;
                foreach (UIElement element in elements)
                {
                    Color4 col = new Color4(red, (((id) & 0xff00) >> 8) / 255f, ((id) & 0xff) / 255f, 255f);
                    element.DrawForHover(col);

                    id++;
                }
            }

            GL.UseProgram(0);
            GL.Flush();
            GL.Finish();

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            byte[] pixel = new byte[4];
            GL.ReadPixels(mouse.X, mouse.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);

            if (pixel[0] == 0) return null;

            int elementId = (pixel[1] * 256) + pixel[2];

            int shaderIndex = (pixel[0]) - 1;

            return interactableElements[shaders[shaderIndex]][elementId];
        }

        private Vector2i previousMousePos = new Vector2i(-1, -1);
        private UIElement? previousHovered;
        public override void OnMouseOver(Vector2 mouse)
        {
            Vector2i clean = new Vector2i((int)mouse.X, (int)mouse.Y);
            if (clean == previousMousePos) return; // Need a dirty flag for when UI has been rebuilt. Something for later though
            previousMousePos = clean;

            UIElement? hovered = GetHovered(clean);
            
            if (hovered != previousHovered)
            {
                previousHovered?.OnMouseLeave(Parent);
                hovered?.OnMouseEnter(Parent);
            }
            else
            {
                hovered?.OnMouseOver(Parent);
            }

            previousHovered = hovered;
        }

        public override void OnMouseRelease(HologramMouse mouse)
        {
            previousHovered?.OnMouseRelease(Parent);
        }

        public override void OnMouseDown(HologramMouse mouse)
        {
            
        }

        public override void OnMouseEnter(Vector2 mouse)
        {
            
        }

        public override void OnMouseLeave(Vector2 mouse)
        {
            previousHovered?.OnMouseLeave(Parent);
            previousHovered = null;
        }

        public override void OnMousePress(HologramMouse mouse)
        {
            
        }

        public void AddElement(UIElement element)
        {
            if (element.Shader == UIDefaults.TextShader)
            {
                textElements.Add((RenderableString)element);
            }
            else
            {
                if (!interactableElements.ContainsKey(element.Shader)) interactableElements[element.Shader] = new List<UIElement>();

                interactableElements[element.Shader].Add(element);
            }
        }

        public UIManager(MainWindow parent, int width, int height) : base(parent, width, height) 
        {
            Font = UIDefaults.Poppins;

            //AddElement(new RenderableString("Hologram - Render Test", Font, 5, 360, 10, Font.Size / 2));

            //Button test = new Button(100, 0, 0, 300, 150, "");
            //test.Click += () =>
            //{
            //    Console.WriteLine("Button Clicked!");
            //};
            //AddElement(test);

            //Toolbar testToolbar = new Toolbar(0, 0, 5, width, height);
            //testToolbar.AddOption(this, "File");
            //testToolbar.AddOption(this, "Edit");
            //AddElement(testToolbar);

            //Cursor.Setup();
        }
    }
}
