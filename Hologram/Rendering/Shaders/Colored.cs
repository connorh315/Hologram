using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders;

internal static class Colored
{
    public static string VertexCode = @"
            #version 330 core
            layout(location = 0) in vec3 Position;
            layout(location = 1) in vec3 Normal;
            layout(location = 2) in vec2 UV;
            layout(location = 3) in vec4 Color;

            uniform mat4 world;
            uniform mat4 view;
            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * view * world * vec4(Position, 1);
            }
        ";

    public static string FragmentCode = @"
            #version 330 core

            uniform vec3 PickingColor;

            out vec4 Color;                

            void main()
            {
                Color = vec4(PickingColor, 1);
            }
        ";
}
