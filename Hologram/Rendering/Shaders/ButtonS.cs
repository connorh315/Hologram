using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders
{
    public static class ButtonS
    {
        public static string VertexCode = @"
            #version 330 core

            layout (location = 0) in vec2 vertex;
            
            uniform mat4 model;
            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * model * vec4(vertex, 0, 1);
            }
        ";

        public static string FragmentCode = @"
            #version 330 core

            out vec4 color;
            uniform vec4 buttonColor;
            
            void main()
            {
                color = buttonColor;
            }
        ";
    }
}
