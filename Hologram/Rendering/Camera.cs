using ModLib;
using OpenTK.Mathematics;

namespace Hologram.Rendering
{
    internal class Camera
    {
        public Vector3 Position = Vector3.Zero;
        
        public Vector3 Forward;
        public Vector3 Right;
        public Vector3 Up = new Vector3(0, 1, 0);

        public float Yaw;
        public float Pitch;

        public Camera(Vector3 pos, Vector3 target)
        {
            Position = pos;

            LookAt(target);
        }

        public void LookAt(Vector3 target)
        {
            Forward = (target - Position).Normalized();
            Console.WriteLine(Forward);
            Right = Vector3.Cross(Forward, Up).Normalized();

            Pitch = (float)(Math.Asin(-Forward.Y) + Math.PI);
            Yaw = (float)Math.Acos(Forward.X / Math.Cos(Pitch));
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
            Right = Vector3.Cross(Forward, Up).Normalized();
        }
    }
}
