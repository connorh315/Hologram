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
        //public Camera Camera;
        //private Shader primaryShader;
        //private Shader lineShader;

        //public static int MeshColorLocation;

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
            UI = new UIManager(this, Size.X, Size.Y);
            Scene = new SceneManager(this, Size.X, Size.Y);

            this.RenderFrequency = 120;
            this.UpdateFrequency = 120;
            this.VSync = VSyncMode.Off;
            this.Title = "Hologram";
            sw.Start();

            SetupCursors();

            SetupSizeCallback();
        }

        public UIManager UI;
        public SceneManager Scene;

        public List<Entity> Entities = new List<Entity>();

        public List<Entity> EngineEntities = new List<Entity>();



        double[] frameTimeBuffer = new double[240];
        int offset = 0;

        private bool cursorDisabled = false;

        private Vector2 unlockPos;
        public Vector2 LockPos { get; private set; }
        /// <summary>
        /// Locks the cursor to the given position.
        /// </summary>
        /// <param name="pos">The position to lock the cursor (in pixels)</param>
        /// <returns>Whether the cursor was just locked.</returns>
        public unsafe bool LockCursor(Vector2 pos)
        {
            if (!cursorDisabled)
            {
                unlockPos = MousePosition;
                LockPos = pos;
                GLFW.SetInputMode(WindowPtr, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
                GLFW.SetCursorPos(WindowPtr, LockPos.X, LockPos.Y);
                cursorDisabled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts raw framebuffer coordinates to window coordinates.
        /// </summary>
        /// <param name="pos">Framebuffer coordinates</param>
        /// <returns>Window coordinates</returns>
        public Vector2 ScaleFBToPixels(Vector2 pos) => pos / (fbWidth / (float)Size.X);
        
        public unsafe void UnlockCursor()
        {
            if (cursorDisabled)
            {
                GLFW.SetInputMode(WindowPtr, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
                GLFW.SetCursorPos(WindowPtr, unlockPos.X, unlockPos.Y);
                cursorDisabled = false;
            }
        }

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

        private unsafe void SetupSizeCallback()
        {
            fbSizeCallback = SizeCallback;
            GLFW.SetFramebufferSizeCallback(this.WindowPtr, fbSizeCallback);
            GLFW.GetFramebufferSize(this.WindowPtr, out fbWidth, out int height);
            ScaleComponents(new Vector2i(fbWidth, height));
        }

        private void ScaleComponents(Vector2i size)
        {
            int uiWidth = (int)(0.3 * size.X);
            if (UI != null)
            {
                UI.SetPos(0, 0);
                UI.SetSize(uiWidth, size.Y);
            }

            if (Scene != null)
            {
                Scene.SetPos(uiWidth, 0);
                Scene.SetSize(size.X - uiWidth, size.Y);
            }
        }

        private unsafe void SizeCallback(Window* window, int width, int height)
        {
            fbWidth = width;
            ScaleComponents(new Vector2i(width, height));
        }

        private Manager? previousHovered;

        private bool IsManagerHovered(Manager manager)
        {
            int x = (int)CorrectedFlippedMouse.X;
            int y = (int)CorrectedFlippedMouse.Y;
            return (x > manager.X && x < (manager.X + manager.Width))
                && (y > manager.Y && y < (manager.Y + manager.Height));
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) return;

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

            Manager hovered = null;

            if (IsManagerHovered(Scene))
            {
                Scene.SetFocus(true);
                UI.SetFocus(false);
                hovered = Scene;
            }
            else if (IsManagerHovered(UI))
            {
                Scene.SetFocus(false);
                UI.SetFocus(true);
                hovered = UI;
            }

            if (hovered != previousHovered)
            {
                previousHovered?.OnMouseLeave(MouseState.Position);
            }
            else
            {
                previousHovered?.OnMouseOver(MouseState.Position);
            }

            Scene.Update(args.Time);

            ManagerOld.Update();

            UI.OnMouseOver(CorrectedFlippedMouse);

            if (cursorDisabled)
            {
                MousePosition = LockPos;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(Scene.X, Scene.Y, Scene.Width, Scene.Height);
            Scene.Draw();

            GL.Viewport(UI.X, UI.Y, UI.Width, UI.Height);
            UI.Draw();

            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ManagerOld.Initialize(this);

            base.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // UpdateViewport(e.Size);

            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            //GL.DeleteProgram(primaryShader);

            base.OnUnload();
        }

        protected override void OnFileDrop(FileDropEventArgs e)
        {
            base.OnFileDrop(e);

            foreach (string file in e.FileNames)
            {
                Entity[] entities = FileLoader.LoadModelFile(file);
                if (entities == null) continue;

                Scene.Entities.Clear();

                Scene.Entities.AddRange(entities);
            }
        }
    }
}
