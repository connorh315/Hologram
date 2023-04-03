using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Rendering;

public static class ShaderManager
{
    public static Shader ActiveShader { get; private set; }

    public static void Use(Shader shader)
    {
        if (true)
        {
            GL.UseProgram(shader);
            ActiveShader = shader;
        }
    }
}
