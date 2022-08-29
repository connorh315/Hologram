using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders
{
    internal static class Basic
    {
        public static string VertexCode = @"
            #version 330 core
            layout(location = 0) in vec3 Position;
            layout(location = 1) in vec3 Normal;

            out vec3 outPosition;
            out vec3 outNormal;

            uniform mat4 world;
            uniform mat4 view;
            uniform mat4 projection;

            void main()
            {
                gl_Position = vec4(Position, 1) * world * view * projection;
                outPosition = vec3(world * vec4(Position, 1));
                outNormal = Normal;
            }
        ";

        public static string FragmentCode = @"
            #version 330 core

            in vec3 outPosition;
            in vec3 outNormal;

            uniform vec3 cameraDir;

            out vec4 Color;                

            void main()
            {
                float ambientStrength = 0.2;
                vec3 lightColor = vec3(1,1,1);
                vec3 objColor = vec3(0.8, 0.8, 0.8);
                vec3 ambient = ambientStrength * lightColor;
                    
                // Diffuse:
                //vec3 lightDir = (lightPos - outPosition);
                //vec3 lightDir = (-outPosition);

                float diff = min(max(dot(outNormal, cameraDir), 0.0),0.9); // max(...,0.9) bc otherwise eyes are scorched
                vec3 diffuse = diff * lightColor;
                    
                vec3 result = (ambient + diffuse) * objColor;

                Color = vec4(result, 1);
            }
        ";
    }
}
