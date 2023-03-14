using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI.Elements
{
    public class ToolbarMenu : UIElement
    {
        public override Shader Shader => UIDefaults.QuadShader;
        public override Shader HoverShader => UIDefaults.QuadShader;

        public ToolbarMenu(UIElement parent) : base(parent) 
        {
            Manager.RemoveElement(this);
            Overlay.AddElement(this);
        }

        public Color4 BackgroundColor = new Color4(20, 20, 20, 255);


        public RenderableString Title;

        public List<(RenderableString Text, Action Callback)> Options = new();

        public ushort BoxHeight;

        private ushort currentY;

        public void SetTitle(string text)
        {
            Title = new RenderableString(this);
            Title.SetText(text);
        }

        public void AddOption(string title, Action callback)
        {
            RenderableString text = new RenderableString(this);
            text.SetText(title);

            Options.Add((text, callback));
            PushOption(text);
        }

        private void PushOption(RenderableString text)
        {
            text.SetPos(text.Width / 2f + XPos, currentY);
        }

        private void DrawColor(Color4 col)
        {
            GL.UniformMatrix4(Shader.GetUniformLocation("model"), false, ref elementMatrix);
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
            throw new NotImplementedException();
        }
    }
}
