using ModLib;
using Hologram.FileTypes.GSC.Primitives;
using Hologram.Objects;

namespace Hologram.FileTypes.GSC
{
	internal class RawMesh
	{
		private ModFile file;

		public List<Part> Parts = new List<Part>();

		public Dictionary<int, VertexList> Vertexlistsdictionary = new();

		public Dictionary<int, List<ushort>> Indexlistsdictionary = new();

		private int referenceCounter = 5;

		public RawMesh(ModFile file)
		{
			this.file = file;
		}

		public int Read()
		{
			int num = file.ReadInt(true);
			//Console.WriteLine("{0:x8}   Number of Parts: 0x{1:x8}", file.Position - 4, num);
			for (int i = 0; i < num; i++)
			{
				//Console.WriteLine("{0:x8}   Part 0x{1:x8}", file.Position, i);
				Parts.Add(ReadPart());
			}
			return (int)file.Position;
		}

		private int ReadRelativePositionList(byte lastByte)
		{
			file.Seek(4, SeekOrigin.Current);
			int num = 1;
			int num2 = 0;
			while (file.ReadInt(true) != 0)
			{
				file.Seek(4, SeekOrigin.Current);
				num++;
			}
			//Console.WriteLine("{0:x8}     Relative Position Lists: 0x{1:x8}", file.Position, num);
			file.Seek(4, SeekOrigin.Current);
			for (int i = 0; i < num; i++)
			{
				file.Seek(4, SeekOrigin.Current);
				int num3 = file.ReadInt(true);
				if (num3 == 0)
				{
					file.Seek(4, SeekOrigin.Current);
					int num4 = file.ReadInt(true);
					file.Seek(num4, SeekOrigin.Current);
					num2++;
					file.Seek(4, SeekOrigin.Current);
					int num5 = file.ReadInt(true);
					file.Seek(4 * num5, SeekOrigin.Current);
					if (num5 > 0)
					{
						num2++;
					}
				}
				else
				{
					file.Seek(4, SeekOrigin.Current);
					num2++;
					for (int j = 0; j < num3; j++)
					{
						file.Seek(12, SeekOrigin.Current);
					}
					file.Seek(12, SeekOrigin.Current);
				}
			}
			return num2;
		}

		private Part ReadPart()
		{
			file.Seek(4, SeekOrigin.Current);
			Part part = new Part();
			int num = file.ReadInt(true);
			//Console.WriteLine("{0:x8}     Number of Vertex Lists: 0x{1:x8}", file.Position - 4, num);
			int offset;
			for (int i = 0; i < num; i++)
			{
				//Console.WriteLine("{0:x8}       Vertex List 0x{1:x8}", file.Position, i);
				VertexListReference vertexListReference = GetVertexListReference(out offset);
				part.VertexListReferences1.Add(vertexListReference);
				if (i == 0)
				{
					part.OffsetVertices = offset / Vertexlistsdictionary[vertexListReference.Reference].VertexSize;
				}
				else
				{
					part.OffsetVertices2 = offset / Vertexlistsdictionary[vertexListReference.Reference].VertexSize;
				}
			}
			file.Seek(4, SeekOrigin.Current);
			part.IndexListReference1 = GetIndexListReference();
			part.OffsetIndices = file.ReadInt(true);
			//Console.WriteLine("{0:x8}     Offset Indices: 0x{1:x8}", file.Position - 4, part.OffsetIndices);
			part.NumberIndices = file.ReadInt(true);
			//Console.WriteLine("{0:x8}     Number Indices: 0x{1:x8}", file.Position - 4, part.NumberIndices);
			offset = file.ReadInt(true);
			//Console.WriteLine("{0:x8}     Offset Vertices: 0x{1:x8}", file.Position - 4, offset);
			if (offset != 0)
			{
				part.OffsetVertices = offset;
				part.OffsetVertices2 = offset;
			}
			else
			{
				if (part.OffsetVertices != 0)
				{
					//Console.WriteLine("{0:x8}       --> Calculated Offset1 Vertices: 0x{1:x8}", file.Position, part.OffsetVertices);
				}
				if (part.VertexListReferences1.Count > 1 && part.OffsetVertices2 != 0)
				{
					//Console.WriteLine("{0:x8}       --> Calculated Offset2 Vertices: 0x{1:x8}", file.Position, part.OffsetVertices2);
				}
			}
			//file.Seek(4, SeekOrigin.Current);
			bool flag = true;
			if (file.ReadShort(true) != 0)
			{
				throw new NotSupportedException("ReadPart Offset Vertices + 4");
			}
			part.NumberVertices = file.ReadInt(true);
			//Console.WriteLine("{0:x8}     Number Vertices: 0x{1:x8}", file.Position - 4, part.NumberVertices);
			file.Seek(4, SeekOrigin.Current);
			int num2 = file.ReadInt(true);
			if (num2 > 0)
			{
				//Console.Write("{0:x8}     ", file.Position);
				for (int i = 0; i < num2; i++)
				{
					//Console.Write("{0:x2} ", file.ReadByte());
				}
				//Console.WriteLine();
				referenceCounter++;
			}
			int num3 = file.ReadInt(true);
			if (num3 != 0)
			{
				int num4 = ReadRelativePositionList(0);
				referenceCounter += num4;
			}
			Logger.Log(new LogSeg("About to skip: "), new LogSeg(file.Position.ToString(), ConsoleColor.Blue));
			file.Seek(40, SeekOrigin.Current);
			referenceCounter++;
			referenceCounter++;
			return part;
		}

		private int GetIndexListReference()
		{
			int num = -1;
			byte read = file.ReadByte();
			if (read == 192)
			{
				file.Seek(1, SeekOrigin.Current);
				num = file.ReadShort(true);
				//Console.WriteLine("{0:x8}     Index List Reference to 0x{1:x4}", file.Position - 2, num);
				int num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}       Unknown 0x{1:x8}", file.Position - 4, num2);
			}
			else
			{
				file.Seek(-1, SeekOrigin.Current);
				//Console.WriteLine("{0:x8}         New Index List 0x{1:x4}", file.Position, referencecounter);
				int num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				int num3 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Number of Indices: {1:x8}", file.Position - 4, num3);
				num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				List<ushort> list = new List<ushort>();
				for (int i = 0; i < num3; i++)
				{
					list.Add(file.ReadUshort(true));
				}
				Indexlistsdictionary.Add(referenceCounter, list);
				num = referenceCounter++;
			}
			return num;
		}

		private VertexListReference GetVertexListReference(out int offset)
		{
			int num = -1;
			byte read = file.ReadByte();
			if (read == 192)
			{
				file.Seek(1, SeekOrigin.Current);
				num = file.ReadShort(true);
				//Console.WriteLine("{0:x8}         Vertex List Reference to 0x{1:x4}", file.Position - 2, num);
				//file.Seek(2, SeekOrigin.Current);
				int num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				offset = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Offset 0x{1:x8}", file.Position - 4, offset);
			}
			else
			{
				file.Seek(-1, SeekOrigin.Current);
				//Console.WriteLine("{0:x8}         New Vertex List 0x{1:x4}", file.Position, referencecounter);
				int num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				num2 = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Unknown 0x{1:x8}", file.Position - 4, num2);
				int numberofvertices = file.ReadInt(true);
				VertexList value = ReadVertexList(numberofvertices);
				offset = file.ReadInt(true);
				//Console.WriteLine("{0:x8}           Offset 0x{1:x8}", file.Position - 4, offset);
				Vertexlistsdictionary.Add(referenceCounter, value);
				num = referenceCounter++;
			}
			VertexListReference vertexListReference = new VertexListReference();
			vertexListReference.GlobalOffset = offset;
			vertexListReference.Reference = num;
			return vertexListReference;
		}

		private VertexDefinition ReadVertexDefinition()
		{
			VertexDefinition vertexDefinition = new VertexDefinition();
			vertexDefinition.Variable = (VertexDefinition.VariableEnum)file.ReadByte();
			vertexDefinition.VariableType = (VertexDefinition.VariableTypeEnum)file.ReadByte();
			vertexDefinition.Offset = file.ReadByte();
			//Console.WriteLine("{0:x8}             {1} {2}", file.Position - 3, vertexDefinition.VariableType.ToString(), vertexDefinition.Variable.ToString());
			return vertexDefinition;
		}

		private VertexList ReadVertexList(int numberofvertices)
		{
			VertexList vertexList = new VertexList();
			file.Seek(8, SeekOrigin.Current);
			int num = file.ReadInt(true);
			//Console.WriteLine("{0:x8}           Number of Vertex Definitions: {1:x8}", file.Position - 4, num);
			for (int i = 0; i < num; i++)
			{
				VertexDefinition vertexDefinition = ReadVertexDefinition();
				vertexList.VertexDefinitions.Add(vertexDefinition);
				switch (vertexDefinition.VariableType)
				{
					case VertexDefinition.VariableTypeEnum.vec2half:
					case VertexDefinition.VariableTypeEnum.vec4char:
					case VertexDefinition.VariableTypeEnum.vec4mini:
					case VertexDefinition.VariableTypeEnum.color4char:
						vertexList.VertexSize += 4;
						break;
					case VertexDefinition.VariableTypeEnum.vec2float:
					case VertexDefinition.VariableTypeEnum.vec4half:
						vertexList.VertexSize += 8;
						break;
					case VertexDefinition.VariableTypeEnum.vec3float:
						vertexList.VertexSize += 12;
						break;
					case VertexDefinition.VariableTypeEnum.vec4float:
						vertexList.VertexSize += 16;
						break;
					default:
						throw new NotSupportedException("VariableType: " + vertexDefinition.VariableType);
				}
			}
			file.Seek(6, SeekOrigin.Current);
			//Console.WriteLine("{0:x8}           Number of Vertices: {1:x8}", file.Position, numberofvertices);
			for (int i = 0; i < numberofvertices; i++)
			{
				vertexList.Vertices.Add(ReadVertex(vertexList.VertexDefinitions));
			}
			return vertexList;
		}

		private Vertex ReadVertex(List<VertexDefinition> vertexdefinitions)
		{
			Vertex vertex = new Vertex();
			foreach (VertexDefinition vertexdefinition in vertexdefinitions)
			{
				switch (vertexdefinition.Variable)
				{
					case VertexDefinition.VariableEnum.position:
						vertex.Position = (Vector3)ReadVariableValue(vertexdefinition.VariableType);
						break;
					case VertexDefinition.VariableEnum.normal:
						vertex.Normal = (Vector3)ReadVariableValue(vertexdefinition.VariableType);
						break;
					case VertexDefinition.VariableEnum.colorSet0:
						vertex.ColorSet0 = (Color4)ReadVariableValue(vertexdefinition.VariableType);
						break;
					case VertexDefinition.VariableEnum.colorSet1:
						vertex.ColorSet1 = (Color4)ReadVariableValue(vertexdefinition.VariableType);
						break;
					case VertexDefinition.VariableEnum.uvSet01:
						vertex.UVSet0 = (Vector2)ReadVariableValue(vertexdefinition.VariableType);
						break;
					case VertexDefinition.VariableEnum.tangent:
					case VertexDefinition.VariableEnum.unknown6:
					case VertexDefinition.VariableEnum.uvSet2:
					case VertexDefinition.VariableEnum.unknown8:
					case VertexDefinition.VariableEnum.blendIndices0:
					case VertexDefinition.VariableEnum.blendWeight0:
					case VertexDefinition.VariableEnum.unknown11:
					case VertexDefinition.VariableEnum.lightDirSet:
					case VertexDefinition.VariableEnum.lightColSet:
						ReadVariableValue(vertexdefinition.VariableType);
						break;
					default:
						throw new NotSupportedException(vertexdefinition.Variable.ToString());
				}
			}
			return vertex;
		}

		private float[] lookUp;

		private float[] LookUp
		{
			get
			{
				if (lookUp == null)
				{
					double num = 0.007874015748031496;
					lookUp = new float[256];
					lookUp[0] = -1f;
					for (int i = 1; i < 256; i++)
					{
						lookUp[i] = (float)((double)lookUp[i - 1] + num);
					}
					lookUp[127] = 0f;
					lookUp[255] = 1f;
				}
				return lookUp;
			}
		}

		private object ReadVariableValue(VertexDefinition.VariableTypeEnum variabletype)
		{
			switch (variabletype)
			{
				case VertexDefinition.VariableTypeEnum.vec2float:
					{ // TODO: Check if copying is necessary.
						Vector2 vector6 = new Vector2();
						vector6.X = file.ReadFloat(true);
						vector6.Y = file.ReadFloat(true);
						Vector2 result7 = vector6;
						return result7;
					}
				case VertexDefinition.VariableTypeEnum.vec3float:
					{
						Vector3 vector5 = new Vector3();
						vector5.X = file.ReadFloat(true);
						vector5.Y = file.ReadFloat(true);
						vector5.Z = file.ReadFloat(true);
						Vector3 result6 = vector5;
						return result6;
					}
				case VertexDefinition.VariableTypeEnum.vec4float:
					{
						Vector4 vector4 = new Vector4();
						vector4.X = file.ReadFloat(true);
						vector4.Y = file.ReadFloat(true);
						vector4.Z = file.ReadFloat(true);
						vector4.W = file.ReadFloat(true);
						Vector4 result5 = vector4;
						return result5;
					}
				case VertexDefinition.VariableTypeEnum.vec2half:
					{
						Vector2 vector3 = new Vector2();
						vector3.X = (float)file.ReadHalf(true);
						vector3.Y = (float)file.ReadHalf(true);
						Vector2 result4 = vector3;
						return result4;
					}
				case VertexDefinition.VariableTypeEnum.vec4half:
					{
						Vector4 vector2 = new Vector4();
						vector2.X = (float)file.ReadHalf(true);
						vector2.Y = (float)file.ReadHalf(true);
						vector2.Z = (float)file.ReadHalf(true);
						vector2.W = (float)file.ReadHalf(true);
						Vector4 result3 = vector2;
						return result3;
					}
				case VertexDefinition.VariableTypeEnum.vec4char:
					file.Seek(4, SeekOrigin.Current);
					return 1;
				case VertexDefinition.VariableTypeEnum.vec4mini:
					{
						Vector4 vector = new Vector4();
						vector.X = LookUp[file.ReadByte()];
						vector.Y = LookUp[file.ReadByte()];
						vector.Z = LookUp[file.ReadByte()];
						vector.W = LookUp[file.ReadByte()];
						Vector4 result2 = vector;
						return result2;
					}
				case VertexDefinition.VariableTypeEnum.color4char:
					{
						Color4 color = new Color4();
						color.R = file.ReadByte();
						color.G = file.ReadByte();
						color.B = file.ReadByte();
						color.A = file.ReadByte();
						Color4 result = color;
						return result;
					}
				default:
					throw new NotImplementedException(variabletype.ToString());
			}
		}

		public Mesh ConvertToHologramMesh()
        {
			uint vertexCount = 0;
			uint faceCount = 0;
			const int start = 0;
			int partsToRender = Parts.Count;
			for (int partId = start; partId < partsToRender; partId++)
            {
				vertexCount += (uint)Parts[partId].NumberVertices;
				faceCount += (uint)(Parts[partId].NumberIndices/3);
			}

			int vertexOffset = 0;
			int faceOffset = 0;
			Mesh mesh = new Mesh(vertexCount, faceCount, FaceType.Triangles);
			for (int partId = start; partId < partsToRender; partId++)
            {
				Part part = Parts[partId];
				VertexList vertexList = Vertexlistsdictionary[part.VertexListReferences1[0].Reference];
				VertexList vertexList2 = null;
				List<ushort> indexList = Indexlistsdictionary[part.IndexListReference1];
				if (part.VertexListReferences1.Count > 1)
				{
					vertexList2 = Vertexlistsdictionary[part.VertexListReferences1[1].Reference];
				}


				for (int i = part.OffsetVertices; i < part.OffsetVertices + part.NumberVertices; i++)
				{
					Vector3 orig = vertexList.Vertices[i].Position;
					mesh.Vertices[vertexOffset + (i - part.OffsetVertices)] = new OpenTK.Mathematics.Vector3(orig.X, orig.Y, orig.Z);
				}

				for (int i = part.OffsetIndices; i < part.OffsetIndices + part.NumberIndices; i += 3)
				{
					Face thisFace = new Face();
					thisFace.vert1 = (ushort)(vertexOffset + indexList[i]);
					thisFace.vert2 = (ushort)(vertexOffset + indexList[i + 1]);
					thisFace.vert3 = (ushort)(vertexOffset + indexList[i + 2]);
					mesh.Faces[faceOffset + ((i - part.OffsetIndices) / 3)] = thisFace;
				}

				vertexOffset += part.NumberVertices;
				faceOffset += part.NumberIndices / 3;
            }

			return mesh;

			//bool flag = false;
			//bool flag2 = false;

			//if (vertexList.Vertices[0].UVSet0 != null || vertexList2 != null && vertexList2.Vertices[0].UVSet0 != null)
   //         {
			//	flag2 = true;
   //         }
			//else if (vertexList.Vertices[0].Normal != null || vertexList2 != null && vertexList2.Vertices[0].Normal != null)
   //         {
			//	flag = true;
   //         }

			//List<Vertex> list;
			//if (vertexList.Vertices[0].ColorSet0 != null)
			//{
			//	list = vertexList.Vertices;
			//}
			//else if (vertexList2 != null && vertexList2.Vertices[0].ColorSet0 != null)
			//{
			//	list = vertexList2.Vertices;
			//}
		}
	}
}
