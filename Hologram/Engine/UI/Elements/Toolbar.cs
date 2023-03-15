using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI.Elements
{
    public class Toolbar : UIElement
    {
        public override Shader Shader => UIDefaults.QuadShader;
        public override Shader HoverShader => UIDefaults.QuadShader;

        public Color4 BackgroundColor = new Color4(20, 20, 20, 255);
        public Color4 HoverColor = new Color4(30, 30, 30, 255);
        public Color4 HoverBorderColor = new Color4(255, 255, 255, 255);
        public float HoverBorderWidth = 1f;

        public float PaddingY = 0.2f;

        public List<ToolbarMenu> Options = new List<ToolbarMenu>();

        public UIManager Overlay;

        public Toolbar(UIElement parent) : base(parent)
        {
            //Manager = manager;
            //Overlay = overlay;
        }

        const int paddingX = 30;

        public ushort XOffset = 0;

        private void PushOption(ToolbarMenu menu)
        {
            RenderableString text = menu.Title;
            int height = (int)((1 - 2 * PaddingY) * YScale);

            text.SetPos(XPos + XOffset + paddingX, YPos + YScale / 2 - height / 2f);
            text.SetHeight(height);

            menu.SetPos(XPos + XOffset, Manager.Y + YPos);
            
            XOffset += (ushort)(text.Width + 2 * paddingX);

        }

        public void AddMenu(ToolbarMenu menu)
        {
            PushOption(menu);
            Options.Add(menu);
        }

        private void DrawColor(Color4 col)
        {
            Surface.DrawRect(ref elementMatrix, col);
        }

        public override void Draw()
        {
            if (drawHover)
            {
                Surface.DrawOutlinedRect(ref hoverQuadMatrix, HoverColor, HoverBorderColor, HoverBorderWidth);
            }

            DrawColor(BackgroundColor);
        }

        public override void DrawForHover(Color4 col)
        {
            DrawColor(col);
        }

        private Matrix4 hoverQuadMatrix = Matrix4.Identity;
        private bool drawHover = false;

        public override void OnMouseOver(MainWindow window)
        {
            float mouseX = window.CorrectedFlippedMouse.X;

            drawHover = false;
            foreach (ToolbarMenu menu in Options)
            {
                RenderableString text = menu.Title;
                float lowerX = text.XPos - paddingX;
                float higherX = text.XPos + text.Width + paddingX;
                if (mouseX > lowerX && mouseX < higherX)
                {
                    hoverQuadMatrix.M11 = higherX - lowerX;
                    hoverQuadMatrix.M22 = YScale;
                    hoverQuadMatrix.M41 = lowerX;
                    hoverQuadMatrix.M42 = YPos;
                    hoverQuadMatrix.M43 = ZPos + 0.1f;

                    drawHover = true;
                    break;
                }
            }
        }

        public override void OnMouseLeave(MainWindow window)
        {
            drawHover = false;
        }

        public override void OnResize(Vector2 originalSize)
        {
            XOffset = 0;

            foreach (ToolbarMenu menu in Options)
            {
                PushOption(menu);
            }
        }
    }
}
