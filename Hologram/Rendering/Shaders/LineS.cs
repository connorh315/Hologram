using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders;

internal class LineS
{
    public static string VertexCode = @"
            #version 330 core
            layout(location = 0) in vec3 Position;

            uniform mat4 view;
            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * view * vec4(Position, 1);
                
            }
        ";

    public static string FragmentCode = @"
            #version 330 core

            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(0, 0, 1, 1);
            }
        ";
}
