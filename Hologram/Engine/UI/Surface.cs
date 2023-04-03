using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Engine.UI.Elements;

namespace Hologram.Engine.UI;

public static class Surface
{
    private static Shader[] shaders = new Shader[]
    {
        UIDefaults.QuadShader,
        UIDefaults.BorderQuadShader,
        UIDefaults.RoundedQuadShader,
        UIDefaults.TextShader
    };

    public static void SetManager(UIManager manager)
    {
        Matrix4 projectionMatrix = manager.GetProjectionMatrix();

        foreach (Shader shader in shaders)
        {
            ShaderManager.Use(shader);
            GL.UniformMatrix4(shader.GetUniformLocation("projection"), false, ref projectionMatrix);
        }
    }

    public static void DrawRect(ref Matrix4 rectangle, Color4 color)
    {
        Shader basicQuad = UIDefaults.QuadShader;
        ShaderManager.Use(basicQuad);

        GL.UniformMatrix4(basicQuad.GetUniformLocation("model"), false, ref rectangle);
        GL.Uniform4(basicQuad.GetUniformLocation("quadColor"), color);

        GL.BindVertexArray(UIElement.QuadArray);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public static void DrawOutlinedRect(ref Matrix4 rectangle, Color4 inner, Color4 border, float borderWidth)
    {
        Shader borderQuad = UIDefaults.BorderQuadShader;
        ShaderManager.Use(borderQuad);

        GL.UniformMatrix4(borderQuad.GetUniformLocation("model"), false, ref rectangle);
        GL.Uniform4(borderQuad.GetUniformLocation("quadColor"), inner);
        GL.Uniform4(borderQuad.GetUniformLocation("borderColor"), border);
        GL.Uniform1(borderQuad.GetUniformLocation("borderSize"), borderWidth);

        GL.BindVertexArray(UIElement.QuadArray);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public static void DrawRoundedRect(ref Matrix4 rectangle, Color4 color, float radius)
    {
        Shader roundedQuad = UIDefaults.RoundedQuadShader;
        ShaderManager.Use(roundedQuad);

        GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.RoundedQuadShader, "model"), false, ref rectangle);
        GL.Uniform4(GL.GetUniformLocation(UIDefaults.RoundedQuadShader, "buttonColor"), color);
        GL.Uniform1(GL.GetUniformLocation(UIDefaults.RoundedQuadShader, "radius"), radius);

        GL.BindVertexArray(UIElement.QuadArray);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public static void DrawText(RenderableString text)
    {
        ShaderManager.Use(UIDefaults.TextShader);
        text.Font.Texture.Use();
        text.Draw();
    }
}
