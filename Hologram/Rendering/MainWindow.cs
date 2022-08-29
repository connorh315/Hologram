using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Hologram.Objects;
using Hologram.Extensions;

using System.Diagnostics;
using ModLib;

namespace Hologram.Rendering
{
    internal class MainWindow : GameWindow
    {
        private Mesh activeMesh;

        private Camera camera;
        private Shader shader;

        private Mesh[] Meshes = new Mesh[50];

        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        public MainWindow()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            camera = new Camera(new Vector3(30, 30, 30), Vector3.Zero);
            shader = new Shader(Shaders.Basic.VertexCode, Shaders.Basic.FragmentCode);

            //this.RenderFrequency = 120;
            //this.UpdateFrequency = 120;
            this.VSync = VSyncMode.Off;
            this.Title = "Hologram";
            //this.activeMesh = mesh;
            sw.Start();
        }

        public void AddMesh(Mesh mesh, bool isActive = false)
        {
            if (isActive)
            {
                activeMesh = mesh;
            }

            for (int i = 0; i < Meshes.Length; i++)
            {
                if (Meshes[i] == null)
                {
                    Meshes[i] = mesh;
                    break;
                }
            }
        }

        const float cameraHSpeed = 24f;
        const float cameraVSpeed = 24f;

        double[] frameTimeBuffer = new double[240];
        int offset = 0;

        private int currentNumero = 0;
        private void SwitchMeshes()
        { // Chuck this whole function out the window at some point
            var input = KeyboardState;

            int numero = -1;
            if (input.IsKeyDown(Keys.D1))
            {
                numero = 0;
            }
            else if (input.IsKeyDown(Keys.D2))
            {
                numero = 1;
            }
            else if (input.IsKeyDown(Keys.D3))
            {
                numero = 2;
            }
            else if (input.IsKeyDown(Keys.D4))
            {
                numero = 3;
            }
            else if (input.IsKeyDown(Keys.D5))
            {
                numero = 4;
            }
            else if (input.IsKeyDown(Keys.D6))
            {
                numero = 5;
            }
            else if (input.IsKeyDown(Keys.D7))
            {
                numero = 6;
            }
            else if (input.IsKeyDown(Keys.D8))
            {
                numero = 7;
            }
            else if (input.IsKeyDown(Keys.D9))
            {
                numero = 8;
            }
            else if (input.IsKeyReleased(Keys.Right))
            {
                numero = currentNumero + 1;
            }
            else if (input.IsKeyReleased(Keys.Left))
            {
                numero = currentNumero - 1;
            }

            if (numero != -1)
            {
                if (Meshes[numero] == null || numero == currentNumero) { return; }

                activeMesh = Meshes[numero];
                currentNumero = numero;

                Logger.Log(new LogSeg("Active Scene: {0}", activeMesh.Name));
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position.Y += cameraVSpeed * (float)args.Time;
            }
            else if (input.IsKeyDown(Keys.LeftControl))
            {
                camera.Position.Y -= cameraVSpeed * (float)args.Time;
            }

            float adjustedSpeed = cameraHSpeed * (float)args.Time;

            if (input.IsKeyDown(Keys.LeftShift))
            {
                adjustedSpeed *= 2;
            }

            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += adjustedSpeed * camera.Forward;
            }
            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= adjustedSpeed * camera.Forward;
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= adjustedSpeed * camera.Right;
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += adjustedSpeed * camera.Right;
            }

            SwitchMeshes();

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
                    camera.RotateCamera(MouseState.Delta.X.Deg2Rad() * 0.1f, MouseState.Delta.Y.Deg2Rad() * 0.1f);
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
            Matrix4 viewMat = Matrix4.LookAt(camera.Position, camera.Position + camera.Forward, Vector3.UnitY);
            GL.UniformMatrix4(viewLoc, true, ref viewMat);

            int worldLoc = GL.GetUniformLocation(shader, "world");
            //Matrix4 rotMat = Matrix4.CreateRotationY((float)TimeAlive);
            Matrix4 rotMat = Matrix4.Identity;
            GL.UniformMatrix4(worldLoc, true, ref rotMat);

            int cameraDir = GL.GetUniformLocation(shader, "cameraDir");
            GL.Uniform3(cameraDir, camera.Forward);
            GL.UseProgram(shader);

            activeMesh.Draw();

            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            for (int i = 0; i < Meshes.Length; i++)
            {
                if (Meshes[i] == null) break;
                Meshes[i].Setup();
            }

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
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(vertexBuffer);

            GL.DeleteProgram(shader);

            base.OnUnload();
        }
    }
}
