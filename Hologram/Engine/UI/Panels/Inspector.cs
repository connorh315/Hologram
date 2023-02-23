using Hologram.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI.Panels
{
    public class Inspector : UIManager
    {
        public Button TestButton;

        public Inspector(MainWindow parent, int width, int height, UIManager overlay) : base(parent, width, height)
        {
            TestButton = new Button(20, 20, 0, 300, 100, "Test", this);
            TestButton.Click += () =>
            {
                Console.WriteLine("Test");
            };
            AddElement(TestButton);
        }
    }
}
