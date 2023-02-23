using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI
{
    public class ToolbarMenu : UIElement
    {
        public UIManager Overlay;

        public ToolbarMenu(int x, int y, int z, int width, int height, UIManager overlay) : base(x, y, z, width, height)
        {
            Overlay = overlay;
        }

        private ushort topY = 0;

        private ushort yOffset = 5;

        public void AddOption(string title, Action callback)
        {
            RenderableString text = new RenderableString(title, UIDefaults.Poppins, 5, topY - yOffset, (int)ZPos + 1, 1);
            Overlay.AddElement(text);
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void DrawForHover(Color4 col)
        {
            throw new NotImplementedException();
        }
    }
}
