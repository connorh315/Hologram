using ModLib;
using OpenTK.Mathematics;
using Hologram.Extensions;

namespace Hologram.Rendering
{
    public class Camera
    {
        public Vector3 Position = Vector3.Zero;
        
        public Vector3 Forward;
        public Vector3 Right;
        public Vector3 Up = new Vector3(0, 1, 0);

        public float Yaw;
        public float Pitch;

        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }
        private Vector2i viewportSize;

        public Camera(Vector3 pos, Vector3 target, Vector2i size)
        {
            Position = pos;

            LookAt(target);

            ResizeViewport(size);
        }

        public void LookAt(Vector3 target)
        {
            Forward = (target - Position).Normalized();

            Right = Vector3.Cross(Forward, Vector3.UnitY).Normalized(); // Here we use the "World" Up

            Up = Vector3.Cross(Right, Forward).Normalized();

            Pitch = (float)(Math.Asin(-Forward.Y) + Math.PI);
            Yaw = (float)Math.Acos(Forward.X / Math.Cos(Pitch));

            viewNeedsUpdating = true;
        }

        private const float lowerClamp = 0.99f * (float)((3 * Math.PI) / 2);
        private const float upperClamp = 1.01f * (float)(Math.PI / 2);

        public void RotateCamera(float yaw, float pitch)
        {
            Yaw += yaw;
            Pitch += pitch;
            Pitch = Math.Clamp(Pitch, upperClamp, lowerClamp);

            float cPitch = (float)Math.Cos(Pitch);
            Forward = new Vector3((float)(Math.Cos(Yaw) * cPitch), (float)Math.Sin(Pitch), (float)(Math.Sin(Yaw) * cPitch));
            Right = Vector3.Cross(Forward, Vector3.UnitY).Normalized();
            Up = Vector3.Cross(Right, Forward).Normalized();
            viewNeedsUpdating = true;
        }

        public void Translate(Vector3 translation)
        {
            Position += translation;

            viewNeedsUpdating = true;
        }

        public void ResizeViewport(Vector2i size)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView((45f).Deg2Rad(), (float)size.X / size.Y, 0.5f, 300f);
            viewportSize = size;
        }

        public Vector3 ScreenToWorldPoint(int posX, int posY)
        {
            Vector2 normalized = GetNormalizedDeviceCoords(posX, posY);
            Vector4 clipCoords = new Vector4(normalized.X, normalized.Y, -1, 1);
            Vector4 eyeCoords = ToEyeCoords(clipCoords);
            Vector3 worldRay = ToWorldCoords(eyeCoords);

            return worldRay;
        }

        private Vector3 ToWorldCoords(Vector4 eyeCoords)
        {
            Matrix4 invertedView = Matrix4.Invert(ViewMatrix);
            Vector4 rayWorld = eyeCoords * invertedView;
            Vector3 mouseRay = new Vector3(rayWorld);
            return mouseRay.Normalized();
        }

        private Vector4 ToEyeCoords(Vector4 clipCoords)
        {
            Matrix4 invertedProjection = Matrix4.Invert(ProjectionMatrix);
            Vector4 eyeCoords = invertedProjection * clipCoords;

            return new Vector4(eyeCoords.X, eyeCoords.Y, -1, 0);
        }

        private Vector2 GetNormalizedDeviceCoords(int posX, int posY)
        {
            float x = (2f * posX) / viewportSize.X - 1;
            float y = 1 - (2f * posY) / viewportSize.Y;

            return new Vector2(x, y);
        }

        private bool viewNeedsUpdating = true;

        /// <summary>
        /// Recalculates the view matrix, if necessary.
        /// </summary>
        /// <returns>Whether the matrix was updated</returns>
        public bool CalculateViewMatrix()
        {
            if (viewNeedsUpdating)
            {
                viewNeedsUpdating = false;

                ViewMatrix = Matrix4.LookAt(Position, Position + Forward, Up);

                return true;
            }

            return false;
        }
    }
}
