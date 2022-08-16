using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Hologram.Objects;
using Hologram.Extensions;

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
        private Vector3 cameraFront = new Vector3(0, 0, -1);
        private Vector3 cameraUp = new Vector3(0, 1, 0);
        private Vector3 cameraRight = new Vector3(1, 0, 0);

        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        public MainWindow(Mesh mesh)
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.RenderFrequency = 0;
            this.UpdateFrequency = 0;
            this.Title = "Hologram";
            this.mesh = mesh;
            sw.Start();
        }

        const float cameraHSpeed = 24f;
        const float cameraVSpeed = 24f;

        double[] frameTimeBuffer = new double[240];
        int offset = 0;

        private float yaw = (float)(Math.PI/2);
        private float pitch = (float)Math.PI;

        private const float lClamp = 0.99f * (float)((3*Math.PI)/2);
        private const float uClamp = 1.01f * (float)(Math.PI/2);

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Space))
            {
                cameraPos.Y += cameraVSpeed * (float)args.Time;
            }
            else if (input.IsKeyDown(Keys.LeftControl))
            {
                cameraPos.Y -= cameraVSpeed * (float)args.Time;
            }

            float adjustedSpeed = cameraHSpeed * (float)args.Time;

            if (input.IsKeyDown(Keys.LeftShift))
            {
                adjustedSpeed *= 2;
            }

            if (input.IsKeyDown(Keys.W))
            {
                cameraPos += adjustedSpeed * cameraFront;
            }
            if (input.IsKeyDown(Keys.S))
            {
                cameraPos -= adjustedSpeed * cameraFront;
            }
            if (input.IsKeyDown(Keys.A))
            {
                cameraPos -= adjustedSpeed * cameraRight;
            }
            if (input.IsKeyDown(Keys.D))
            {
                cameraPos += adjustedSpeed * cameraRight;
            }

            frameTimeBuffer[offset] = args.Time;
            offset++;
            if (offset == frameTimeBuffer.Length)
            {
                offset = 0;
            }

            double totalTime = 0;
            for (int i = 0; i < frameTimeBuffer.Length; i++)
            {
                totalTime += frameTimeBuffer[i];
            }

            Title = "Hologram - " + Math.Round(frameTimeBuffer.Length / totalTime) + " FPS";

            if (MouseState.IsButtonDown(MouseButton.Right))
            {
                CursorState = CursorState.Grabbed;
                if (MouseState.Delta.X != 0 || MouseState.Delta.Y != 0)
                {
                    yaw += MouseState.Delta.X.Deg2Rad() * 0.1f;
                    pitch += MouseState.Delta.Y.Deg2Rad() * 0.1f;
                    pitch = Math.Clamp(pitch, uClamp, lClamp);
                    Vector3 dir = new Vector3((float)(Math.Cos(yaw) * Math.Cos(pitch)), (float)(Math.Sin(pitch)), (float)(Math.Sin(yaw) * Math.Cos(pitch)));
                    cameraFront = dir.Normalized();
                    cameraRight = Vector3.Cross(cameraFront, cameraUp).Normalized();
                }
            }
            else
            {
                CursorState = CursorState.Normal;
            }

            //cameraPos.X = cameraPos.Z = Math.Max(cameraPos.Z-MouseState.ScrollDelta.Y, 0.5f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);        

            int projectionLoc = GL.GetUniformLocation(shader, "projection");
            Matrix4 projectionMat = Matrix4.CreatePerspectiveFieldOfView(1, (float)Size.X / Size.Y, 0.5f, 100f); // Can be setup once as opposed to every frame...
            GL.UniformMatrix4(projectionLoc, true, ref projectionMat);

            int viewLoc = GL.GetUniformLocation(shader, "view");
            Matrix4 viewMat = Matrix4.LookAt(cameraPos, cameraPos + cameraFront, Vector3.UnitY);
            GL.UniformMatrix4(viewLoc, true, ref viewMat);

            int worldLoc = GL.GetUniformLocation(shader, "world");
            //Matrix4 rotMat = Matrix4.CreateRotationY((float)TimeAlive);
            Matrix4 rotMat = Matrix4.Identity;
            GL.UniformMatrix4(worldLoc, true, ref rotMat);

            int cameraDir = GL.GetUniformLocation(shader, "cameraDir");
            GL.Uniform3(cameraDir, cameraFront);
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
            ushort[] vertexIndex = new ushort[mesh.FaceCount * (mesh.Type == FaceType.Quads ? 6 : 3)]; // should be 3 when gsc, 6 when dno
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

                vertexIndex[currentOffset] = currentFace.vert1;
                vertexIndex[currentOffset + 1] = currentFace.vert2;
                vertexIndex[currentOffset + 2] = currentFace.vert3;

                if (mesh.Type == FaceType.Quads)
                {
                    vertices[currentFace.vert4].Normal += normal;
                    vertexIndex[currentOffset + 3] = currentFace.vert1;
                    vertexIndex[currentOffset + 4] = currentFace.vert3;
                    vertexIndex[currentOffset + 5] = currentFace.vert4;
                }

                currentOffset += (mesh.Type == FaceType.Quads ? 6U : 3U); // should be 6
            }

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                vertices[i].Position = mesh.Vertices[i];
                vertices[i].Normal.Normalize();
            }

            //Face biggestFace = mesh.Faces[biggestOffset];
            //vertices[biggestFace.vert1].Position = Vector3.Zero; vertices[biggestFace.vert2].Position = Vector3.Zero; vertices[biggestFace.vert3].Position = Vector3.Zero; vertices[biggestFace.vert4].Position = Vector3.Zero;

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
                    outPosition = vec3(world * vec4(Position, 1));
                    outNormal = Normal;
                }
            ";

            string FragmentCode = @"
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
