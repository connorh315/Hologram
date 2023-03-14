using Hologram.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using Hologram.Engine.UI.Elements;

namespace Hologram.Engine.UI.Panels
{
    public class Inspector : UIManager
    {
        public Button TestButton;

        public Inspector(MainWindow parent, int width, int height) : base(parent, width, height)
        {
            TestButton = new Button(Panel);
            TestButton.SetBounds(100, 100, 150, 75);
            TestButton.SetText("Clip!");
            TestButton.Click += () =>
            {
                Console.WriteLine("Test");
            };
        }
    }
}
