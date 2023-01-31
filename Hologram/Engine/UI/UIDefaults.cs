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
        public static readonly Color4 ForegroundColor = Color4.White;

        public static Shader ButtonShader = new Shader(ButtonS.VertexCode, ButtonS.FragmentCode);
    }
}
