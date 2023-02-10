using Hologram.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Hologram.Engine
{
    public static class ManagerOld
    {
        public static MainWindow Window;

        private static Vector2 previousMousePosition;

        /// <summary>
        /// Initializes all managers
        /// </summary>
        public static void Initialize(MainWindow window)
        {
            Window = window;

            EntityManager.Initialize();
        }

        public static void Update()
        {
            HologramMouse mouseState = new HologramMouse()
            {
                PreviousPosition = previousMousePosition,
                CurrentPosition = Window.CorrectedMouse,
                State = Window.MouseState
            };

            if (Window.IsMouseButtonPressed(MouseButton.Left))
            {
                EntityManager.OnMousePressed(mouseState);
            }
            else if (Window.IsMouseButtonDown(MouseButton.Left))
            {
                EntityManager.OnMouseDown(mouseState);
            }
            else if (Window.IsMouseButtonReleased(MouseButton.Left))
            {
                EntityManager.OnMouseReleased(mouseState);
            }

            EntityManager.Update();

            previousMousePosition = Window.CorrectedMouse;
        }
    }

    public struct HologramMouse
    {
        public Vector2 PreviousPosition;
        public Vector2 CurrentPosition;
        public Vector2 Delta => CurrentPosition - PreviousPosition;
        public MouseState State;
    }
}
