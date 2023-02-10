using OpenTK.Mathematics;
using Hologram.Resources;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Engine;

namespace Hologram.Objects.Entities
{
    public class TranslationArrow : EngineEntity
    {
        public Vector3 Axis;

        public TranslationArrow(Vector3 axis, Matrix4 transformation) : base(transformation)
        {
            Mesh = BakedMesh.GetMesh(new ArrowModel());
            Material = new Rendering.Material();
            Bounds = new CameraBounds()
            {
                Center = Vector3.Zero,
                DistSqrd = 1000000
            };
            Axis = axis;

            position = 5 * axis;


            if (axis == new Vector3(1, 0, 0))
            {
                Material.Color = Color4.Red;
                Material.MaterialName = "XArrow";
                Rotate(0, 0, -90);
            }
            else if (axis == new Vector3(0, 1, 0))
            {
                Material.Color = Color4.Blue;
                Material.MaterialName = "YArrow";
                Rotate(0, 0, 0);
            }
            else if (axis == new Vector3(0, 0, 1))
            {
                Material.Color = Color4.Green;
                Material.MaterialName = "ZArrow";
                Rotate(90, 0, 0);
            }
        }

        public override void OnMousePressed(HologramMouse mouseState)
        {
            Color4 current = Material.Color;
            Material.Color = new Color4(current.R * 0.8f, current.G * 0.8f, current.B * 0.8f, 255);
        }

        public override void OnMouseDown(HologramMouse mouseState)
        {
            var camera = ManagerOld.Window.Camera;
            Vector3 previousRay = camera.ScreenToWorldRay((int)mouseState.PreviousPosition.X, (int)mouseState.PreviousPosition.Y);
            Vector3 currentRay = camera.ScreenToWorldRay((int)mouseState.CurrentPosition.X, (int)mouseState.CurrentPosition.Y);
            Vector3 cameraToThis = position - camera.Position;
            Vector3 direction = (currentRay - previousRay) * cameraToThis.Length;

            Parent?.Translate(Vector3.Dot(direction, Axis) * Axis);
        }

        public override void OnMouseReleased(HologramMouse mouseState)
        {
            Color4 current = Material.Color;
            Material.Color = new Color4(current.R / 0.8f, current.G / 0.8f, current.B / 0.8f, 255);
        }

        public override void Update()
        { // Looks ridiculous
            //Vector3 dist = Manager.Window.Camera.Position - Position;
            //float scale = Math.Clamp(dist.LengthFast / 100, 0.1f, 1f);
            //Scale = new Vector3(scale, scale, scale);
        }
    }
}
