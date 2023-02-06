using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders
{
    public class TextS
    {
        public static string VertexCode = @"
            #version 330 core

            layout (location = 0) in vec2 pos;
            layout (location = 1) in vec2 uv;
            
            out vec2 outUV;

            uniform mat4 model;
            uniform mat4 projection;

            void main()
            {
                gl_Position = projection * model * vec4(pos, 0, 1);
                outUV = uv;
            }
        ";

        public static string FragmentCode = @"
            #version 330 core

            in vec2 outUV;

            out vec4 color;
            uniform mat4 model;
            uniform vec4 textColor;
            uniform float radius;
            
            uniform sampler2D texture0;
            
            const float width = 0.5;
            const float edge = 0.1;

            void main()
            {
                //float result = texture(texture0, vec2(outUV.x, outUV.y)).r;
                //color = vec4(result, result, result, result);

                float distance = 1.0 - texture(texture0, vec2(outUV.x, outUV.y)).r;

                float alpha = 1 - smoothstep(width, width + edge, distance);

                if (alpha < 0.01) discard;

                color = vec4(alpha * textColor.r, alpha * textColor.g, alpha * textColor.b, alpha);
            }
        ";
    }
}
