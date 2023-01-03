using OpenTK.Mathematics;

namespace Hologram.Rendering
{
    public class Material
    {
        public Color4 Color = Color4.White;

        public Texture Diffuse = Texture.WhiteTexture;

        public Texture Normal = Texture.WhiteTexture;

        public string ShaderName = "Unnamed";

        public Material Duplicate()
        {
            return new Material()
            {
                Color = Color,
                Diffuse = Diffuse,
                Normal = Normal,
                ShaderName = ShaderName
            };
        }
    }
}
