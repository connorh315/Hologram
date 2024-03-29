﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Rendering.Shaders;

internal static class Textured
{
    public static string VertexCode = @"
            #version 330 core
            layout(location = 0) in vec3 Position;
            layout(location = 1) in vec3 Normal;
            layout(location = 2) in vec2 UV;
            layout(location = 3) in vec4 Color;

            out vec3 outPosition;
            out vec3 outNormal;
            out vec2 outUV;
            out vec4 outColor;

            uniform mat4 world;
            uniform mat4 view;
            uniform mat4 projection;

            void main()
            {
                //gl_Position = vec4(Position, 1) * world * view * projection;
                gl_Position = projection * view * world * vec4(Position, 1);
                outPosition = vec3(world * vec4(Position, 1));
                outNormal = Normal;
                outUV = UV;
                outColor = Color;
            }
        ";

    public static string FragmentCode = @"
            #version 330 core

            in vec3 outPosition;
            in vec3 outNormal;
            in vec2 outUV;
            in vec4 outColor;

            uniform vec3 cameraDir;
            uniform vec3 meshColor;
            //uniform int selectedPrimitive;

            uniform sampler2D texture0;

            out vec4 Color;                

            void main()
            {
                float ambientStrength = 0.2;
                vec3 lightColor = vec3(1,1,1);
                vec3 objColor = vec3(outColor);
                vec3 ambient = ambientStrength * lightColor;
                    
                // Diffuse:
                //vec3 lightDir = (lightPos - outPosition);
                //vec3 lightDir = (-outPosition);

                float diff = min(max(dot(outNormal, -cameraDir), 0.0),0.9); // max(...,0.9) bc otherwise eyes are scorched
                vec3 diffuse = diff * lightColor;
                    
                vec3 result = (ambient + diffuse) * objColor;

                //Color = vec4(result, 1);

                vec4 vertColor = outColor;
                if ((vertColor.r + vertColor.g + vertColor.b) == 0)
                    vertColor = vec4(1, 1, 1, 1);

                Color = texture(texture0, outUV) * vec4(meshColor, 1) * vertColor;
                if (Color.a < 0.1) 
                    discard;
            }
        ";
}
