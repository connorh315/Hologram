using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
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
    public class MainWindow : GameWindow
    {
        private Camera camera;
        private Shader primaryShader;
        private Shader lineShader;

        public static int MeshColorLocation;

        private GLFWCallbacks.FramebufferSizeCallback fbSizeCallback;
        private int fbWidth;
        
        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        public MainWindow()
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Flags = ContextFlags.ForwardCompatible,
            })
        {
            camera = new Camera(new Vector3(30, 30, 30), Vector3.Zero, Size);
            
            primaryShader = new Shader(Shaders.Textured.VertexCode, Shaders.Textured.FragmentCode);
            lineShader = new Shader(Shaders.LineS.VertexCode, Shaders.LineS.FragmentCode);

            MeshColorLocation = GL.GetUniformLocation(primaryShader, "meshColor");

            this.RenderFrequency = 120;
            this.UpdateFrequency = 120;
            this.VSync = VSyncMode.Off;
            this.Title = "Hologram";
            sw.Start();
            
            unsafe
            {
                fbSizeCallback = SizeCallback;
                GLFW.SetFramebufferSizeCallback(this.WindowPtr, fbSizeCallback);
                GLFW.GetFramebufferSize(this.WindowPtr, out fbWidth, out int height);
                UpdateViewport(new Vector2i(fbWidth, height));
            }
        }

        public List<Entity> Entities = new List<Entity>();

        const float cameraHSpeed = 24f;
        const float cameraVSpeed = 24f;

        double[] frameTimeBuffer = new double[240];
        int offset = 0;

        private unsafe void SizeCallback(Window* window, int width, int height)
        {
            fbWidth = width;
            UpdateViewport(new Vector2i(width, height));
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
            int[] viewport = new int[4];
            
            if (MouseState.IsButtonReleased(MouseButton.Left))
            {
                float horizontalScale = fbWidth / (float)Size.X;
                Vector2 corrected = new Vector2(MouseState.Position.X, Size.Y - MouseState.Position.Y);
                Entity result = Physics.Pick(Entities.ToArray(), camera, corrected * horizontalScale);
                if (result != null)
                {
                    Logger.Log(result.Name);
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(primaryShader);

            int cameraDir = GL.GetUniformLocation(primaryShader, "cameraDir");
            GL.Uniform3(cameraDir, camera.Forward);

            int worldLoc = GL.GetUniformLocation(primaryShader, "world");
            foreach (Entity entity in Entities)
            {
                if (Vector3.DistanceSquared(camera.Position, entity.Bounds.Center) <= entity.Bounds.DistSqrd)
                {
                    entity.Draw(worldLoc);
                }
            }

            GL.UseProgram(lineShader);

            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // UpdateViewport(e.Size);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.DeleteProgram(primaryShader);

            base.OnUnload();
        }

        protected override void OnFileDrop(FileDropEventArgs e)
        {
            base.OnFileDrop(e);

            foreach (string file in e.FileNames)
            {
                Entity[] entities = FileLoader.LoadModelFile(file);
                if (entities == null) continue;
                
                Entities.AddRange(entities);
            }
        }
    }
}
