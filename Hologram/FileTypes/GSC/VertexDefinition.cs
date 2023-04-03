using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC;

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
			diffuse,
			uvSet2,
			albedo,
			blendIndices0,
			blendWeight0,
			tangent2,
			lightDirSet,
			lightColSet,
			blendPos2,
			random
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
