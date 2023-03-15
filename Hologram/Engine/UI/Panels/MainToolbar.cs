using Hologram.Engine.UI.Elements;
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

        public MainToolbar(MainWindow parent, int width, int height) : base(parent, width, height)
        {
            Toolbar = new Toolbar(Panel);
            
            ToolbarMenu fileMenu = new ToolbarMenu(Toolbar);
            fileMenu.SetPos(100, 100);
            fileMenu.SetTitle("File");
            fileMenu.AddOption("New", () => { });
            fileMenu.AddOption("Open", () => { });

            ToolbarMenu editMenu = new ToolbarMenu(Toolbar);
            editMenu.SetTitle("Edit");

            Toolbar.AddMenu(fileMenu);
            Toolbar.AddMenu(editMenu);
        }

        public override void OnResize()
        {
            Toolbar.SetSize(Width, Height);
        }

        public override bool OnMouseOver(Vector2 mouse)
        {
            Toolbar.OnMouseOver(Parent);
            return true;
        }

        public override void OnMouseLeave(Vector2 mouse)
        {
            Toolbar.OnMouseLeave(Parent);
        }
    }
}
