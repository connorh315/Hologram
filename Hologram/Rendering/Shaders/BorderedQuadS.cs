using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders;

public static class BorderedQuadS
{
    public static string VertexCode = @"
            #version 330 core

            layout (location = 0) in vec2 uv;
            
            out vec2 outUV;

            uniform mat4 model;
            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * model * vec4(uv, 0, 1);
                outUV = uv;
            }
        ";

    public static string FragmentCode = @"
            #version 330 core

            in vec2 outUV;

            out vec4 color;

            uniform mat4 model;

            uniform vec4 quadColor;
            uniform vec4 borderColor;
            uniform float borderSize;
            
            void main()
            {
                vec2 dimensions = vec2(model[0][0], model[1][1]);
                vec2 pos = outUV * dimensions;
                
                if (pos.x < borderSize || pos.x > (dimensions.x - borderSize) || pos.y < borderSize || pos.y > (dimensions.y - borderSize)) {
                    color = borderColor;
                }
                else {
                    color = quadColor;
                }

            }
        ";
}
