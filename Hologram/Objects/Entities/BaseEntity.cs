using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Rendering;
using Hologram.Extensions;

namespace Hologram.Objects.Entities
{
    public class BaseEntity
    {
        protected Matrix4 transformation;

        protected Vector3 position;
        protected Vector3 rotation = new Vector3(0, 0, 0);
        protected Vector3 scale = new Vector3(1, 1, 1);

        public Vector3 Position { get { return position; } set { position = value; UpdateTransformation(); } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; UpdateTransformation(); } }
        public Vector3 Scale { get { return scale; } set { scale = value; UpdateTransformation(); } }

        public Matrix4 Transformation => transformation;

        private void ApplyRotation()
        {
            transformation *= Matrix4.CreateRotationZ(rotation.Z.Deg2Rad());
            transformation *= Matrix4.CreateRotationY(rotation.Y.Deg2Rad());
            transformation *= Matrix4.CreateRotationX(rotation.X.Deg2Rad());
        }

        private void UpdateTransformation()
        {
            transformation = Matrix4.CreateScale(scale);
            ApplyRotation();
            transformation *= Matrix4.CreateTranslation(position);
        }

        public void Rotate(float degX, float degY, float degZ)
        {
            Rotation += new Vector3(degX, degY, degZ);
        }

        public BaseEntity(Matrix4 transformation)
        {
            this.transformation = transformation;
            
            position = transformation.ExtractTranslation();
            scale = transformation.ExtractScale();
            Matrix4 rot = transformation.ClearScale().ClearTranslation();

            Vector3 rad = rot.ExtractRotation().ToEulerAngles();
            rotation = new Vector3(rad.X.Rad2Deg(), rad.Y.Rad2Deg(), rad.Z.Rad2Deg());
        }

        public string Name;

        public MeshX Mesh;

        public Material Material;

        public CameraBounds Bounds;

        public void Draw(int programWorldPosition)
        {
            Material.Diffuse.Use();

            GL.UniformMatrix4(programWorldPosition, false, ref transformation);
            Mesh.Draw(Material);
        }

        public virtual Entity Duplicate()
        {
            return new Entity(this.transformation)
            {
                Bounds = Bounds,
                Material = Material,
                Mesh = Mesh,
                Name = Name + "1",
            };
        }
    }
}
