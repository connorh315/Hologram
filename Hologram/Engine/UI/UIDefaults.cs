using Hologram.Rendering;
using Hologram.Rendering.Shaders;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI
{
    internal static class UIDefaults
    {
        public static readonly Color4 ButtonBG = new Color4(40, 40, 40, 255);
        public static readonly Color4 FG = Color4.White;

        public static Shader ButtonShader = new Shader(ButtonS.VertexCode, ButtonS.FragmentCode);
        public static Shader TextShader = new Shader(TextS.VertexCode, TextS.FragmentCode);
        public static Shader QuadShader = new Shader(QuadS.VertexCode, QuadS.FragmentCode);
        public static Shader BorderQuadShader = new Shader(BorderedQuadS.VertexCode, BorderedQuadS.FragmentCode);

        public static Font Poppins = new Font("Poppins");
    }
}
