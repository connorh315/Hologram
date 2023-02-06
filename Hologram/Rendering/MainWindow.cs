using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Extensions;
using Hologram.Engine;
using Hologram.FileTypes;

using System.Diagnostics;
using ModLib;
using ImGuiNET;
using Hologram.Objects.Entities;
using Hologram.Engine.UI;

namespace Hologram.Rendering
{
    public class MainWindow : GameWindow
    {
        public Camera Camera;
        private Shader primaryShader;
        private Shader lineShader;

        public static int MeshColorLocation;

        private GLFWCallbacks.FramebufferSizeCallback fbSizeCallback;
        private int fbWidth;
        
        public double TimeAlive => sw.Elapsed.TotalSeconds;
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Mouse position from bottom-left of window, corrected with framebuffer scaling
        /// </summary>
        public Vector2 CorrectedFlippedMouse => (fbWidth / (float)Size.X) * (new Vector2(MouseState.Position.X, Size.Y - MouseState.Position.Y));

        /// <summary>
        /// Mouse position from top-left of window, corrected with framebuffer scaling
        /// </summary>
        public Vector2 CorrectedMouse => (fbWidth / (float)Size.X) * (new Vector2(MouseState.Position.X, MouseState.Position.Y));

        public MainWindow()
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Flags = ContextFlags.ForwardCompatible,
                Size = new Vector2i(1280, 720)
            })
        {
            profile.Start();
            Camera = new Camera(new Vector3(30, 30, 30), Vector3.Zero, Size);
            
            primaryShader = new Shader(Shaders.Textured.VertexCode, Shaders.Textured.FragmentCode);
            lineShader = new Shader(Shaders.LineS.VertexCode, Shaders.LineS.FragmentCode);

            MeshColorLocation = GL.GetUniformLocation(primaryShader, "meshColor");

            //this.RenderFrequency = 120;
            //this.UpdateFrequency = 120;
            this.VSync = VSyncMode.Off;
            this.Title = "Hologram";
            sw.Start();

            SetupCursors();

            unsafe
            {
                fbSizeCallback = SizeCallback;
                GLFW.SetFramebufferSizeCallback(this.WindowPtr, fbSizeCallback);
                GLFW.GetFramebufferSize(this.WindowPtr, out fbWidth, out int height);
                UpdateViewport(new Vector2i(fbWidth, height));
            }
        }

        public UIRenderer ui;

        public List<Entity> Entities = new List<Entity>();

        public List<Entity> EngineEntities = new List<Entity>();

        const float cameraHSpeed = 24f;
        const float cameraVSpeed = 24f;

        double[] frameTimeBuffer = new double[240];
        int offset = 0;

        private unsafe Cursor* handCursor;
        private unsafe Cursor* arrowCursor;

        public unsafe void SetCursor(CursorShape shape)
        {
            switch (shape)
            {
                case CursorShape.Hand:
                    GLFW.SetCursor(WindowPtr, handCursor);
                    break;
                default:
                    GLFW.SetCursor(WindowPtr, arrowCursor);
                    break;

            }
        }

        private unsafe void SetupCursors()
        {
            handCursor = GLFW.CreateStandardCursor(CursorShape.Hand);
            arrowCursor = GLFW.CreateStandardCursor(CursorShape.Arrow);
        }

        private unsafe void SizeCallback(Window* window, int width, int height)
        {
            fbWidth = width;
            UpdateViewport(new Vector2i(width, height));
        }
        
        private void UpdateViewport(Vector2i size)
        {
            GL.Viewport(0, 0, size.X, size.Y);
            Camera.ResizeViewport(size);

            //Logger.Log(new LogSeg(Size.X.ToString(), ConsoleColor.Red), new LogSeg(" - "), new LogSeg(size.X.ToString(), ConsoleColor.Blue));

            if (ui != null)
                ui.SetSize(size.X, size.Y);

            Matrix4 projectionMat = Camera.ProjectionMatrix;

            GL.UseProgram(primaryShader);
            int projectionLoc = GL.GetUniformLocation(primaryShader, "projection");
            GL.UniformMatrix4(projectionLoc, false, ref projectionMat);

            GL.UseProgram(lineShader);
            int projectionLocLine = GL.GetUniformLocation(lineShader, "projection");
            GL.UniformMatrix4(projectionLocLine, false, ref projectionMat);
        }

        private double average = 0;
        private int total = 0;
        private Stopwatch profile = new Stopwatch();

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Space))
            {
                Camera.Translate(Vector3.UnitY * cameraVSpeed * (float)args.Time);
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
                Camera.Translate(adjustedSpeed * Camera.Forward);
            }
            if (input.IsKeyDown(Keys.S))
            {
                Camera.Translate(-adjustedSpeed * Camera.Forward);
            }
            if (input.IsKeyDown(Keys.A))
            {
                Camera.Translate(-adjustedSpeed * Camera.Right);
            }
            if (input.IsKeyDown(Keys.D))
            {
                Camera.Translate(adjustedSpeed * Camera.Right);
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
                    Camera.RotateCamera(MouseState.Delta.X.Deg2Rad() * 0.1f, MouseState.Delta.Y.Deg2Rad() * 0.1f);
                }
            }
            else
            {
                CursorState = CursorState.Normal;
            }

            Manager.Update();

            ui.OnMouseOver(CorrectedFlippedMouse);

            if (Camera.CalculateViewMatrix())
            {
                Matrix4 viewMat = Camera.ViewMatrix;

                GL.UseProgram(primaryShader);
                int viewLoc = GL.GetUniformLocation(primaryShader, "view");
                GL.UniformMatrix4(viewLoc, false, ref viewMat);

                GL.UseProgram(lineShader);
                int viewLocLine = GL.GetUniformLocation(lineShader, "view");
                GL.UniformMatrix4(viewLocLine, false, ref viewMat);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(primaryShader);

            int cameraDir = GL.GetUniformLocation(primaryShader, "cameraDir");
            GL.Uniform3(cameraDir, Camera.Forward);

            int worldLoc = GL.GetUniformLocation(primaryShader, "world");
            foreach (Entity entity in Entities)
            {
                if (Vector3.DistanceSquared(Camera.Position, entity.Bounds.Center) <= entity.Bounds.DistSqrd)
                {
                    entity.Draw(worldLoc);
                }
            }

            GL.Clear(ClearBufferMask.DepthBufferBit);

            foreach (Entity engineEntity in EngineEntities)
            {
                engineEntity.Draw(worldLoc);
            }

            ui.Draw();

            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            ui = new UIRenderer(Size.X, Size.Y);
            ui.SetParent(this);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Manager.Initialize(this);

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

                Entities.Clear();

                Entities.AddRange(entities);
            }
        }
    }
}
