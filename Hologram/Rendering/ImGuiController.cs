using ImGuiNET;

namespace Hologram.Rendering
{
    public class ImGuiController
    {
        private int windowWidth;
        private int windowHeight;

        public ImGuiController()
        {
            IntPtr context = ImGui.CreateContext();
            ImGui.GetIO().Fonts.AddFontDefault();
            ImGui.StyleColorsDark();
        }

        public void UpdateWindowSize(int width, int height)
        {
            windowWidth = width;
            windowHeight = height;
        }


    }
}
