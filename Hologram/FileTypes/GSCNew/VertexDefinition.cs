using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSCNew
{
    public class VertexDefinition
    {
		public enum VariableEnum
		{
			position,
			normal,
			colorSet0,
			tangent,
			colorSet1,
			uvSet01,
			unknown6,
			uvSet2,
			unknown8,
			blendIndices0,
			blendWeight0,
			unknown11,
			lightDirSet,
			lightColSet
		}

		public enum StorageTypeEnum
		{
			vec2float = 2,
			vec3float,
			vec4float,
			vec2half,
			vec4half,
			vec4char,
			vec4mini,
			color4char
		}

		public VariableEnum Variable;

		public StorageTypeEnum VariableType;

		public int Offset;
	}
}
