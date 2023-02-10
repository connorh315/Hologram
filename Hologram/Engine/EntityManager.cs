using Hologram.Resources;
using Hologram.Rendering;
using OpenTK.Mathematics;
using Hologram.Objects.Entities;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Extensions;

namespace Hologram.Engine
{
    public static class EntityManager
    {
        public static EngineEntity XArrow;
        public static EngineEntity YArrow;
        public static EngineEntity ZArrow;

        public static void Initialize()
        {
            XArrow = new TranslationArrow(Vector3.UnitX, Matrix4.Identity);
            YArrow = new TranslationArrow(Vector3.UnitY, Matrix4.Identity);
            ZArrow = new TranslationArrow(Vector3.UnitZ, Matrix4.Identity);
        }

        public static Entity? SelectedUserEntity { get; private set; }

        public static EngineEntity? SelectedEngineEntity { get; private set; }

        public static void Update()
        {
            XArrow.Update();
            YArrow.Update();
            ZArrow.Update();
            // Call all Update functions on entities
        }

        public static void OnMousePressed(HologramMouse mouseState)
        {
            MainWindow window = ManagerOld.Window;
            Entity? result = Physics.Pick(window.Entities.ToArray(), window.EngineEntities.ToArray(), window.Camera, window.CorrectedFlippedMouse);
            if (result is EngineEntity)
            {
                SelectedEngineEntity = (EngineEntity)result;
                SelectedEngineEntity.OnMousePressed(mouseState);
                return;
            }

            if (result == null || SelectedUserEntity == result)
            {
                SelectedEngineEntity = null;

                if (SelectedUserEntity == null) return;

                window.EngineEntities.Remove(XArrow);
                window.EngineEntities.Remove(YArrow);
                window.EngineEntities.Remove(ZArrow);

                XArrow.SetParent(null);
                YArrow.SetParent(null);
                ZArrow.SetParent(null);

                SelectedUserEntity = null;
                Console.WriteLine("cleared");
                return;
            }

            SelectedUserEntity = result;
            SelectedEngineEntity = null;

            int offset = 5;

            XArrow.Position = result.Position + new Vector3(offset, 0, 0);
            YArrow.Position = result.Position + new Vector3(0, offset, 0);
            ZArrow.Position = result.Position + new Vector3(0, 0, offset);

            if (!window.EngineEntities.Contains(XArrow))
            {
                window.EngineEntities.Add(XArrow);
                window.EngineEntities.Add(YArrow);
                window.EngineEntities.Add(ZArrow);
            }

            result.AddChild(XArrow);
            result.AddChild(YArrow);
            result.AddChild(ZArrow);
        }

        public static void OnMouseDown(HologramMouse mouseState)
        {
            SelectedEngineEntity?.OnMouseDown(mouseState);
        }

        public static void OnMouseReleased(HologramMouse mouseState)
        {
            SelectedEngineEntity?.OnMouseReleased(mouseState);
        }
    }
}
