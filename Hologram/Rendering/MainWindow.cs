using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Hologram.Objects;
using Hologram.Extensions;
using Hologram.Engine;
using Hologram.FileTypes;

using System.Diagnostics;
using ModLib;
using ImGuiNET;

namespace Hologram.Rendering
{
    internal class MainWindow : GameWindow
    {
        private MeshX activeMesh;

        private Camera camera;
        private Shader primaryShader;
        private Shader lineShader;

        private MeshX[] Meshes = new MeshX[50];

        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        public MainWindow()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            camera = new Camera(new Vector3(30, 30, 30), Vector3.Zero, Size);
            
            primaryShader = new Shader(Shaders.Textured.VertexCode, Shaders.Textured.FragmentCode);
            lineShader = new Shader(Shaders.LineS.VertexCode, Shaders.LineS.FragmentCode);

            UpdateViewport(Size);

            //GL.Uniform1(GL.GetUniformLocation(primaryShader, "selectedPrimitive"), -1);

            this.RenderFrequency = 120;
            this.UpdateFrequency = 120;
            this.VSync = VSyncMode.Off;
            this.Title = "Hologram";
            //this.activeMesh = mesh;
            sw.Start();
        }

        public void AddMesh(MeshX mesh, bool isActive = false)
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

        private Entity[] entities;
        public void AddEntities(Entity[] entities)
        {
            this.entities = entities;
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

        private void UpdateViewport(Vector2i size)
        {
            GL.Viewport(0, 0, size.X, size.Y);
            camera.ResizeViewport(size);

            Matrix4 projectionMat = camera.ProjectionMatrix;

            GL.UseProgram(primaryShader);
            int projectionLoc = GL.GetUniformLocation(primaryShader, "projection");
            GL.UniformMatrix4(projectionLoc, false, ref projectionMat);

            GL.UseProgram(lineShader);
            int projectionLocLine = GL.GetUniformLocation(lineShader, "projection");
            GL.UniformMatrix4(projectionLocLine, false, ref projectionMat);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Space))
            {
                camera.Translate(Vector3.UnitY * cameraVSpeed * (float)args.Time);
            }
            else if (input.IsKeyDown(Keys.LeftControl))
            {
                //camera.Translate(Vector3.UnitY * -cameraVSpeed * (float)args.Time);
            }

            float adjustedSpeed = cameraHSpeed * (float)args.Time;

            if (input.IsKeyDown(Keys.LeftShift))
            {
                adjustedSpeed *= 2;
            }

            if (input.IsKeyDown(Keys.W))
            {
                camera.Translate(adjustedSpeed * camera.Forward);
            }
            if (input.IsKeyDown(Keys.S))
            {
                camera.Translate(-adjustedSpeed * camera.Forward);
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Translate(-adjustedSpeed * camera.Right);
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Translate(adjustedSpeed * camera.Right);
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

            if (camera.CalculateViewMatrix())
            {
                Matrix4 viewMat = camera.ViewMatrix;

                GL.UseProgram(primaryShader);
                int viewLoc = GL.GetUniformLocation(primaryShader, "view");
                GL.UniformMatrix4(viewLoc, false, ref viewMat);

                GL.UseProgram(lineShader);
                int viewLocLine = GL.GetUniformLocation(lineShader, "view");
                GL.UniformMatrix4(viewLocLine, false, ref viewMat);
            }

            Vector3 dir = camera.ScreenToWorldPoint((int)MouseState.Position.X, (int)MouseState.Position.Y);

            if (MouseState.IsButtonReleased(MouseButton.Left))
            {
                //int raycastResult = Physics.Raycast(camera, dir, activeMesh); // Needs converting to MeshX
                //int truePrim = ((raycastResult & 1) == 1) ? (raycastResult - 1) / 2 : raycastResult / 2;
                //Console.WriteLine("Selected primitive: {0}", truePrim);
                //GL.UseProgram(primaryShader);
                //GL.Uniform1(GL.GetUniformLocation(primaryShader, "selectedPrimitive"), raycastResult);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(primaryShader);

            //int worldLoc = GL.GetUniformLocation(primaryShader, "world");
            //Matrix4 rotMat = Matrix4.CreateRotationY((float)TimeAlive);
            //Matrix4 rotMat = Matrix4.Identity;
            //GL.UniformMatrix4(worldLoc, true, ref rotMat);

            int cameraDir = GL.GetUniformLocation(primaryShader, "cameraDir");
            GL.Uniform3(cameraDir, camera.Forward);

            int worldLoc = GL.GetUniformLocation(primaryShader, "world");
            foreach (Entity entity in entities)
            {
                entity.Draw(worldLoc);
            }
            //activeMesh.Draw();

            GL.UseProgram(lineShader);
            //activeMesh.DrawLines();
            //debugLine.Draw();

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
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //GL.UseProgram(primaryShader);

            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            UpdateViewport(e.Size);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(vertexBuffer);

            GL.DeleteProgram(primaryShader);

            base.OnUnload();
        }
    }
}
