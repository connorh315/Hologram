using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Rendering;

namespace Hologram.Engine.UI
{
    public class Button
    {
        public string Text;
        public Color4 BackgroundColor = Color4.White;
        public Color4 ForegroundColor = UIDefaults.ForegroundColor;

        private Matrix4 modelMatrix;

        private int vertexArray;

        public Button(string text)
        {
            Text = text;

            modelMatrix = Matrix4.Identity;

            float[] testArray = new float[]
            {
                1.0f, 1.0f,
                400f, 400f,
                400f, 1.0f,
                1.0f, 400f,
                400f, 400f,
                1.0f, 1.0f
            };

            vertexArray = GL.GenVertexArray();
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * testArray.Length, testArray, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 8, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            //ShaderManager.Use(UIDefaults.ButtonShader); // needs the projection matrix so probably can't do it this way

            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.ButtonShader, "model"), false, ref modelMatrix);
            GL.Uniform4(GL.GetUniformLocation(UIDefaults.ButtonShader, "buttonColor"), BackgroundColor);

            GL.BindVertexArray(vertexArray);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }
    }
}
