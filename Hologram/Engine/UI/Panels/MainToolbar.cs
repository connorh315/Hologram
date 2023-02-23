using Hologram.Rendering;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI.Panels
{
    public class MainToolbar : UIManager
    {
        public Toolbar Toolbar;

        public MainToolbar(MainWindow parent, int width, int height, UIManager overlay) : base(parent, width, height)
        {
            Toolbar = new Toolbar(0, 0, 0, Width, Height, this, overlay);
            Toolbar.AddOption("File");
            Toolbar.AddOption("Edit");
            AddElement(Toolbar);
        }

        public override void OnResize()
        {
            Toolbar.SetSize(Width, Height);
        }

        public override void OnMouseOver(Vector2 mouse)
        {
            Toolbar.OnMouseOver(Parent);
        }

        public override void OnMouseLeave(Vector2 mouse)
        {
            Toolbar.OnMouseLeave(Parent);
        }
    }
}
