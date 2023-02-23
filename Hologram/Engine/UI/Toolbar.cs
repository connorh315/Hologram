﻿using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
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

        public List<RenderableString> Options = new List<RenderableString>();

        public UIManager Manager;

        public UIManager Overlay;

        public Toolbar(int x, int y, int z, int width, int height, UIManager manager, UIManager overlay) : base(x, y, z, width, height)
        {
            Manager = manager;
            Overlay = overlay;
        }

        const int paddingX = 30;

        public ushort XOffset = 0;

        private void PushOption(RenderableString text)
        {
            int height = (int)((1 - (2 * PaddingY)) * YScale);

            text.SetPos(XPos + XOffset + paddingX, YPos + (YScale / 2) - (height / 2f));
            text.SetHeight(height);

            XOffset += (ushort)(text.Width + (2 * paddingX));
        }

        public void AddOption(string title)
        {
            RenderableString text = new RenderableString(title, UIDefaults.Poppins, 1, 1, (int)ZPos + 1, 1);
            PushOption(text);
            Options.Add(text);
            Manager.AddElement(text);
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
            if (drawHover)
            {
                Shader original = ShaderManager.ActiveShader;
                Shader border = UIDefaults.BorderQuadShader;
                ShaderManager.Use(border);
                Matrix4 projection = Manager.GetProjectionMatrix();
                GL.UniformMatrix4(border.GetUniformLocation("projection"), false, ref projection);
                GL.UniformMatrix4(border.GetUniformLocation("model"), false, ref hoverQuadMatrix);
                GL.Uniform4(border.GetUniformLocation("quadColor"), HoverColor);
                GL.Uniform4(border.GetUniformLocation("borderColor"), HoverBorderColor);
                GL.Uniform1(border.GetUniformLocation("borderSize"), HoverBorderWidth);

                GL.BindVertexArray(QuadArray);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                ShaderManager.Use(original);
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
            foreach (RenderableString text in Options)
            {
                float lowerX = text.XPos - paddingX;
                float higherX = text.XPos + text.Width + paddingX;
                if (mouseX > (lowerX) && mouseX < (higherX))
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

            foreach (RenderableString text in Options)
            {
                PushOption(text);
            }
        }
    }
}
