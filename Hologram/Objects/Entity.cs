using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using Hologram.Rendering;

namespace Hologram.Objects
{
    public class Entity
    {
        private Matrix4 transformation;

        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public Vector3 Position { get { return position; } set { position = value; UpdateTransformation(); } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; UpdateTransformation(); } }
        public Vector3 Scale { get { return scale; } set { scale = value; UpdateTransformation(); } }

        public Matrix4 Transformation => transformation;

        public MeshX Mesh;

        public Material Material;

        public CameraBounds Bounds;

        public Entity(Matrix4 transformation)
        {
            this.transformation = transformation;
        }

        private void UpdateTransformation()
        {
            transformation = Matrix4.CreateTranslation(position) * Matrix4.CreateFromAxisAngle(rotation, 0) * Matrix4.CreateScale(scale);
        }

        public void Draw(int programWorldPosition)
        {
            Material.Diffuse.Use();

            GL.UniformMatrix4(programWorldPosition, false, ref transformation);
            Mesh.Draw(Material);
        }
    }
}
