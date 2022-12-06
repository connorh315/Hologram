using OpenTK.Mathematics;

namespace Hologram.Rendering
{
    public class Material
    {
        public Color4 Color = Color4.White;

        public Texture Diffuse;

        public Texture Normal;

        public string ShaderName;
    }
}
