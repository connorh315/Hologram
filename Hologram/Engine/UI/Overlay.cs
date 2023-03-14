using Hologram.Engine.UI.Elements;
using Hologram.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI
{
    /// <summary>
    /// Just wraps around a UIManager in a static class so it can be accessed from anywhere
    /// </summary>
    public static class Overlay
    {
        private static UIManager manager;
        public static UIManager GetManager() => manager;

        public static void Initialize(MainWindow parent, int width, int height)
        {
            manager = new UIManager(parent, width, height);
        }

        public static void AddElement(UIElement element)
        {
            manager.AddElement(element);
        }

        public static void RemoveElement(UIElement element)
        {
            manager.RemoveElement(element);
        }

        public static void SetSize(int width, int height)
        {
            manager.SetSize(width, height);
        }

        public static void Draw()
        {
            manager.Draw();
        }
    }
}
