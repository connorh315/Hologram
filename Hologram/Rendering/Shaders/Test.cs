using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders
{
    internal static class Test
    {
        public static string VertexCode = @"
            #version 330 core
            uniform vec3 mousePos;

            layout(location = 0) in vec3 pos;

            void main()
            {
                gl_Position = vec4(0.2f * pos, 1);
            }
        ";

        public static string FragmentCode = @"
            #version 330 core

            out vec4 Color;                

            void main()
            {
                Color = vec4(1, 1, 0, 1);
            }
        ";
    }
}
