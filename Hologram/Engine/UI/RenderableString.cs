using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI
{
    public class RenderableString
    {
        public RenderableString(string text, Font font, float scale)
        {
            float[] vertices = new float[4 * 4 * text.Length];

            float xOffset = 0;
            float yOffset = 0;

            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);

            for (int i = 0; i < text.Length; i++)
            {
                FontChar thisChar = font.Chars[asciiBytes[i]];
                vertices[i] = xOffset; // These are coordinates - No texture coordinates are represented here.
                vertices[i + 1] = yOffset;
                vertices[i + 2] = xOffset + thisChar.Width;
                vertices[i + 3] = yOffset;
                vertices[i + 4] = xOffset + thisChar.Width;
                vertices[i + 5] = yOffset + thisChar.Height;
            }
        }
    }
}
