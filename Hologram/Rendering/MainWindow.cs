using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Hologram.Objects;

using System.Diagnostics;

namespace Hologram.Rendering
{
    internal class MainWindow : GameWindow
    {
        private Mesh mesh;
        private int vertexBuffer;
        private int vertexArray;
        private int indexBuffer;
        private int indexCount;
        private int shader;

        private Vector3 cameraPos = new Vector3(30, 30, 30);

        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        public MainWindow(Mesh mesh)
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.RenderFrequency = 60;
            this.UpdateFrequency = 60;
            this.mesh = mesh;
            sw.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            const float cameraSpeed = 24f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.LeftShift))
            {
                cameraPos.Y += cameraSpeed * (float)args.Time;
            }
            else if (input.IsKeyDown(Keys.LeftControl))
            {
                cameraPos.Y -= cameraSpeed * (float)args.Time;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);        

            int projectionLoc = GL.GetUniformLocation(shader, "projection");
            Matrix4 projectionMat = Matrix4.CreatePerspectiveFieldOfView(1, (float)Size.X / Size.Y, 0.5f, 100f); // Can be setup once as opposed to every frame...
            GL.UniformMatrix4(projectionLoc, true, ref projectionMat);

            int viewLoc = GL.GetUniformLocation(shader, "view");
            Matrix4 viewMat = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
            GL.UniformMatrix4(viewLoc, true, ref viewMat);

            int worldLoc = GL.GetUniformLocation(shader, "world");
            Matrix4 rotMat = Matrix4.CreateRotationY((float)TimeAlive);
            GL.UniformMatrix4(worldLoc, true, ref rotMat);

            GL.UseProgram(shader);

            GL.BindVertexArray(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedShort, 0);

            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            VertexPosNorm[] vertices = new VertexPosNorm[mesh.VertexCount];
            ushort[] vertexIndex = new ushort[mesh.FaceCount * 6];
            uint currentOffset = 0;

            int biggestOffset = 0;
            float biggestValue = 0;
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face currentFace = mesh.Faces[i];

                Vector3 vert1 = mesh.Vertices[currentFace.vert1];
                Vector3 vert2 = mesh.Vertices[currentFace.vert2];
                Vector3 vert3 = mesh.Vertices[currentFace.vert3];
                Vector3 vert4 = mesh.Vertices[currentFace.vert4];

                Vector3 normal = Vector3.Cross(vert2 - vert1, vert3 - vert1);

                float totalSize = (Vector3.Cross(vert2 - vert1, vert3 - vert1)).Length / 2;
                if (totalSize > biggestValue)
                {
                    biggestValue = totalSize;
                    biggestOffset = i;
                }
                vertices[currentFace.vert1].Normal += normal;
                vertices[currentFace.vert2].Normal += normal;
                vertices[currentFace.vert3].Normal += normal;
                vertices[currentFace.vert4].Normal += normal;

                vertexIndex[currentOffset] = currentFace.vert1;
                vertexIndex[currentOffset + 1] = currentFace.vert2;
                vertexIndex[currentOffset + 2] = currentFace.vert3;
                vertexIndex[currentOffset + 3] = currentFace.vert1;
                vertexIndex[currentOffset + 4] = currentFace.vert3;
                vertexIndex[currentOffset + 5] = currentFace.vert4;

                currentOffset += 6;
            }

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                vertices[i].Position = mesh.Vertices[i];
                vertices[i].Normal.Normalize();
            }

            Face biggestFace = mesh.Faces[biggestOffset];
            vertices[biggestFace.vert1].Position = Vector3.Zero; vertices[biggestFace.vert2].Position = Vector3.Zero; vertices[biggestFace.vert3].Position = Vector3.Zero; vertices[biggestFace.vert4].Position = Vector3.Zero;

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * VertexPosNorm.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

            string VertexCode = @"
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
                    outPosition = Position;
                    outNormal = Normal;
                }
            ";

            string FragmentCode = @"
                #version 330 core

                in vec3 outPosition;
                in vec3 outNormal;

                out vec4 Color;                

                void main()
                {
                    float ambientStrength = 0.2;
                    vec3 lightColor = vec3(1,1,1);
                    vec3 lightPos = vec3(0, 2, 0);
                    vec3 objColor = vec3(0.8, 0.8, 0.8);
                    vec3 ambient = ambientStrength * lightColor;
                    
                    // Diffuse:
                    vec3 lightDir = (lightPos - outPosition);
                    //vec3 lightDir = (-outPosition);

                    float diff = max(dot(outNormal, lightDir), 0.0);
                    vec3 diffuse = diff * lightColor;
                    
                    vec3 result = (ambient + diffuse) * objColor;

                    Color = vec4(result, 1);
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VertexCode);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, FragmentCode);

            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0) { throw new Exception(GL.GetShaderInfoLog(vertexShader)); }

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0) { throw new Exception(GL.GetShaderInfoLog(fragmentShader)); }

            shader = GL.CreateProgram();

            GL.AttachShader(shader, vertexShader);
            GL.AttachShader(shader, fragmentShader);

            GL.LinkProgram(shader);

            GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out success);
            if (success == 0) { throw new Exception(GL.GetProgramInfoLog(shader)); }

            GL.DetachShader(shader, vertexShader);
            GL.DetachShader(shader, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 24, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 24, 12);

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndex.Length * 2, vertexIndex, BufferUsageHint.StaticDraw);
            indexCount = vertexIndex.Length;

            GL.Enable(EnableCap.DepthTest);

            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBuffer);

            GL.DeleteProgram(shader);

            base.OnUnload();
        }
    }
}
