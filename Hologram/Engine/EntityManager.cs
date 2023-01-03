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
        public static Entity XArrow;
        public static Entity YArrow;
        public static Entity ZArrow;

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
            // Call all Update functions on entities
        }

        public static void OnMousePressed(HologramMouse mouseState)
        {
            MainWindow window = Manager.Window;
            Entity? result = Physics.Pick(window.Entities.ToArray(), window.Camera, window.CorrectedFlippedMouse);
            if (result is EngineEntity)
            {
                SelectedEngineEntity = (EngineEntity)result;
                SelectedEngineEntity.OnMousePressed(mouseState);
                return;
            }

            if (result == null)
            {
                SelectedEngineEntity = null;

                if (SelectedUserEntity == null) return;

                Manager.Window.Entities.Remove(XArrow);
                Manager.Window.Entities.Remove(YArrow);
                Manager.Window.Entities.Remove(ZArrow);

                SelectedUserEntity.Children.Remove(XArrow);
                SelectedUserEntity.Children.Remove(YArrow);
                SelectedUserEntity.Children.Remove(ZArrow);

                SelectedUserEntity = null;
                return;
            }

            SelectedUserEntity = result;
            SelectedEngineEntity = null;

            int offset = 5;

            Matrix4 test = SelectedUserEntity.Transformation;
            //test.Transpose();
            Console.WriteLine(test.ClearScale().ClearTranslation());
            //Vector3 block = SelectedUserEntity.Transformation.ExtractRotation().ToEulerAngles();
            //Console.WriteLine(new Vector3(block.X.Rad2Deg(), block.Y.Rad2Deg(), block.Z.Rad2Deg()));

            XArrow.Position = result.Position + new Vector3(offset, 0, 0);
            YArrow.Position = result.Position + new Vector3(0, offset, 0);
            ZArrow.Position = result.Position + new Vector3(0, 0, offset);

            Manager.Window.Entities.Add(XArrow);
            Manager.Window.Entities.Add(YArrow);
            Manager.Window.Entities.Add(ZArrow);

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
