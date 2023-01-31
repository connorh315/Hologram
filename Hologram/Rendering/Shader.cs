using OpenTK.Graphics.OpenGL4;
using ModLib;

namespace Hologram.Rendering
{
    // TODO: DeleteProgram on application closing.
    public class Shader
    {
        int Handle;

        private void Throw(int componentId, bool isProgram = false)
        {
            string message = "Shader compilation failed:\n" + (isProgram ? GL.GetProgramInfoLog(componentId) : GL.GetShaderInfoLog(componentId));
            Logger.Log(new LogSeg(message, ConsoleColor.Red));
            throw new Exception(message);
        }

        public Shader(string vertexCode, string fragmentCode)
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexCode);

            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0) Throw(vertexShader);


            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentCode);

            GL.CompileShader(fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0) Throw(fragmentShader);


            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0) Throw(Handle);


            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public static implicit operator int(Shader shader) => shader.Handle;
    }
}
