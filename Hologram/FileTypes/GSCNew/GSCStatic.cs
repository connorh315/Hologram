using Hologram.Objects;
using ModLib;
using OpenTK.Mathematics;

namespace Hologram.FileTypes.GSCNew
{
    public partial class GSC
    {
        public static GSC Parse(string fileLocation)
        {
            GSC gsc = new GSC();

            using (ModFile file = ModFile.Open(fileLocation))
            {
                gsc.file = file;

                uint mainChunkOffset = file.ReadUint(true);
                file.Seek(mainChunkOffset, SeekOrigin.Current);

                uint mainChunkSize = file.ReadUint(true);
                uint chunkVersion = file.ReadUint(true);

                file.CheckString("02UN", Locale.GSCStrings.ExpectedNU20);
                file.CheckInt(0x4F, Locale.GSCStrings.ExpectedNU20Version);

                file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO);

                uint stringsCount = file.ReadUint(true);
                for (int i = 0; i < stringsCount; i++)
                {
                    Logger.Log(file.ReadPascalString());
                }

                file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL);
                file.CheckInt(0x4F, Locale.GSCStrings.ExpectedNTBLVersion);

                file.Seek(file.ReadInt(true), SeekOrigin.Current); // Big blob of strings

                file.Seek(24, SeekOrigin.Current); // ROTV

                file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH);
                file.CheckInt(0xAF, Locale.GSCStrings.ExpectedMESHVersion);

                uint partCount = file.ReadUint(true);
                gsc.parts = new Part[partCount];
                for (int partId = 0; partId < partCount; partId++)
                {
                    uint one = file.ReadUint(true);

                    uint vertexListCount = file.ReadUint(true);

                    Part part = new Part(vertexListCount);
                    gsc.parts[partId] = part;
                    for (int vertexListId = 0; vertexListId < vertexListCount; vertexListId++)
                    {
                        part.VertexListReferences[vertexListId] = gsc.GetVertexListReference();
                    }
                    file.Seek(4, SeekOrigin.Current);
                    part.IndexListReference = gsc.GetIndexListReference();
                    Console.WriteLine("part offset: " + file.Position);
                    part.OffsetIndices = file.ReadInt(true);
                    part.IndicesCount = file.ReadInt(true);
                    part.OffsetVertices = file.ReadInt(true);

                    Console.WriteLine(part.OffsetIndices);
                    Console.WriteLine(part.IndicesCount);
                    Console.WriteLine(part.OffsetVertices);

                    if (file.ReadShort(true) != 0) { throw new Exception("ReadPart Offset Vertices + 4"); }

                    part.VerticesCount = file.ReadInt(true);

                    Console.WriteLine(part.VerticesCount);

                    file.Seek(4, SeekOrigin.Current);

                    int num = file.ReadInt(true);
                    if (num > 0)
                    {
                        Logger.Warn("Using assumption XXXXXX");
                        file.Seek(num, SeekOrigin.Current);
                    }

                    int num2 = file.ReadInt(true);
                    if (num2 > 0)
                    {
                        int num3 = gsc.ReadRelativePositionList();
                        gsc.referenceCounter += num3;
                    }

                    file.Seek(40, SeekOrigin.Current);

                    gsc.referenceCounter += 2;
                }

                file.Seek(4, SeekOrigin.Current);
                uint materialCount = file.ReadUint(true);
                for (int i = 0; i < materialCount; i++)
                {
                    file.CheckString("LTMU", Locale.GSCStrings.ExpectedUMTL);
                    uint materialVersion = file.ReadUint(true); // I'm not sure about this
                    uint remainingVertexDefinitions = file.ReadUint(true);

                    file.Seek(0x40a, SeekOrigin.Current); // Material data
                    file.ReadPascalString();
                    file.Seek(0x498, SeekOrigin.Current); // Material data pt. 2

                    //for (int vertexDefId = 0; vertexDefId < remainingVertexDefinitions; vertexDefId++)
                    //{
                    //    uint vertexCount = file.ReadUint(true);
                    //    file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
                    //    file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion);
                    //    file.Seek(0x464, SeekOrigin.Current);
                    //    file.ReadPascalString();
                    //    file.Seek(0x498, SeekOrigin.Current);
                    //}
                }

                Logger.Log(new LogSeg("Reached offset {0}", ConsoleColor.Green, file.Position.ToString()));
            }

            return gsc;
        }

        private int GetIndexListReference()
        {
            ushort marker = file.ReadUshort(true);
            int reference;
            if ((marker & 0xc000) != 0)
            {
                reference = file.ReadUshort(true);
                int unknown = file.ReadInt(true);
            }
            else
            {
                ushort unknown = file.ReadUshort(true);

                uint unknown2 = file.ReadUint(true);

                uint indicesCount = file.ReadUint(true);

                uint unknown3 = file.ReadUint(true);

                ushort[] indexBuffer = new ushort[indicesCount];

                for (int indexId = 0; indexId < indicesCount; indexId++)
                {
                    indexBuffer[indexId] = file.ReadUshort(true);
                }

                indexLists[referenceCounter] = indexBuffer;

                reference = referenceCounter++;
            }

            return reference;
        }

        private VertexListReference GetVertexListReference()
        {
            ushort marker = file.ReadUshort(true);
            int reference;
            int offset;
            if ((marker & 0xc000) != 0)
            {
                reference = file.ReadUshort(true);
                int unknown = file.ReadInt(true);
                offset = file.ReadInt(true);
            }
            else
            {
                ushort unknown = file.ReadUshort(true);
                int unknown2 = file.ReadInt(true);
                VertexList list = ReadVertexList();
                vertexLists.Add(referenceCounter, list);
                offset = file.ReadInt(true);
                reference = referenceCounter++;
            }

            VertexListReference listReference = new VertexListReference();
            listReference.Reference = reference;
            listReference.GlobalOffset = offset;
            return listReference;
        }

        private VertexList ReadVertexList()
        {
            uint vertexCount = file.ReadUint(true);
            file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
            file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion);

            uint vertexDefinitionCount = file.ReadUint(true);
            VertexList list = new VertexList(vertexCount, vertexDefinitionCount);
            for (int id = 0; id < vertexDefinitionCount; id++)
            {
                VertexDefinition definition = new VertexDefinition();
                definition.Variable = (VertexDefinition.VariableEnum)file.ReadByte();
                definition.VariableType = (VertexDefinition.StorageTypeEnum)file.ReadByte();
                definition.Offset = file.ReadByte();

                list.Definitions[id] = definition;
            }

            file.Seek(6, SeekOrigin.Current); // All zeroes

            for (int i = 0; i < vertexCount; i++)
            {
                list.Vertices[i] = ReadVertex(list);
            }

            return list;
        }

        private Vertex ReadVertex(VertexList list)
        {
            Vertex vertex = new Vertex();
            foreach (VertexDefinition definition in list.Definitions)
            {
                switch (definition.Variable)
                {
                    case VertexDefinition.VariableEnum.position:
                        vertex.Position = ReadValue<Vector3>(definition.VariableType);
                        break;
                    case VertexDefinition.VariableEnum.normal:
                        vertex.Normal = ReadValue<Vector3>(definition.VariableType);
                        break;
                    case VertexDefinition.VariableEnum.colorSet0:
                        vertex.ColorSet0 = ReadValue<Color4>(definition.VariableType);
                        break;
                    case VertexDefinition.VariableEnum.colorSet1:
                        vertex.ColorSet1 = ReadValue<Color4>(definition.VariableType);
                        break;
                    case VertexDefinition.VariableEnum.uvSet01:
                        vertex.UVSet0 = ReadValue<Vector2>(definition.VariableType);
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
                        ReadValue<object>(definition.VariableType);
                        break;
                    default:
                        throw new NotSupportedException(definition.Variable.ToString());
                }
            }
            return vertex;
        }

        private T ReadValue<T>(VertexDefinition.StorageTypeEnum storageType)
        {
            Vector4 extendedResult;
            switch (storageType)
            {
                case VertexDefinition.StorageTypeEnum.vec2float:
                    {
                        return (T)(object)new Vector2(file.ReadFloat(true), file.ReadFloat(true));
                    }
                case VertexDefinition.StorageTypeEnum.vec3float:
                    {
                        extendedResult = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), 1);
                        break;
                    }
                case VertexDefinition.StorageTypeEnum.vec4float:
                    {
                        extendedResult = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                        break;
                    }
                case VertexDefinition.StorageTypeEnum.vec2half:
                    {
                        return (T)(object)new Vector2((float)file.ReadHalf(true), (float)file.ReadHalf(true));
                    }
                case VertexDefinition.StorageTypeEnum.vec4half:
                    {
                        extendedResult = new Vector4((float)file.ReadHalf(true), (float)file.ReadHalf(true), (float)file.ReadHalf(true), (float)file.ReadHalf(true));
                        break;
                    }
                case VertexDefinition.StorageTypeEnum.vec4char:
                    file.Seek(4, SeekOrigin.Current);
                    extendedResult = new Vector4();
                    break;
                case VertexDefinition.StorageTypeEnum.vec4mini:
                    {
                        extendedResult = new Vector4(ScaleByte(file.ReadByte()), ScaleByte(file.ReadByte()), ScaleByte(file.ReadByte()), ScaleByte(file.ReadByte()));
                        break;
                    }
                case VertexDefinition.StorageTypeEnum.color4char:
                    {
                        return (T)(object)new Color4(file.ReadByte(), file.ReadByte(), file.ReadByte(), file.ReadByte());
                    }
                default:
                    throw new NotImplementedException(storageType.ToString());
            }
            Type returnType = typeof(T);
            if (returnType == typeof(Vector2))
            {
                return (T)(object)new Vector2(extendedResult.X, extendedResult.Y);
            }
            else if (returnType == typeof(Vector3))
            {
                return (T)(object)new Vector3(extendedResult.X, extendedResult.Y, extendedResult.Z);
            }
            else
            { // Assume Vector4
                return (T)(object)extendedResult;
            }
        }

        private int ReadRelativePositionList()
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

        private float ScaleByte(byte toScale)
        {
            if (floatScales == null)
            {
                floatScales = new float[256];

                double num = 0.007874015748031496;
                floatScales[0] = -1f;
                for (int i = 1; i < 256; i++)
                {
                    floatScales[i] = (float)((double)floatScales[i - 1] + num);
                }
                floatScales[127] = 0f;
                floatScales[255] = 1f;
            }

            return floatScales[toScale];
        }

        private float[] floatScales;

        public Mesh ConvertToMesh()
        {
            int vertexCount = 0;
            int faceCount = 0;
            
            //Part part = parts[2];
            //faceCount = part.IndicesCount / 3;

            foreach (Part part in parts)
            {
                vertexCount += part.VerticesCount;
                faceCount += part.IndicesCount / 3;
            }

            Mesh mesh = new Mesh(vertexCount, faceCount, FaceType.Triangles);
            int vertexOffset = 0;
            int faceOffset = 0;

            VertexList a = vertexLists[parts[0].VertexListReferences[0].Reference];
            for (int i = 0; i < a.Vertices.Length; i++)
            {
                mesh.Vertices[i] = a.Vertices[i].Position;
            }


            foreach (Part part in parts)
            {

                VertexList primaryList = vertexLists[part.VertexListReferences[0].Reference];
                VertexList secondaryList;
                if (part.VertexListReferences.Length > 1)
                {
                    secondaryList = vertexLists[part.VertexListReferences[1].Reference];
                }

                ushort[] indexList = indexLists[part.IndexListReference];

                for (int i = part.OffsetVertices; i < part.OffsetVertices + part.VerticesCount; i++)
                {
                    mesh.Vertices[vertexOffset + i - part.OffsetVertices] = primaryList.Vertices[i].Position;
                }

                for (int i = part.OffsetIndices; i < part.OffsetIndices + part.IndicesCount; i+=3)
                {
                    Face face = new Face();
                    face.vert1 = (ushort)(vertexOffset + indexList[i]);
                    face.vert2 = (ushort)(vertexOffset + indexList[i + 1]);
                    face.vert3 = (ushort)(vertexOffset + indexList[i + 2]);
                    mesh.Faces[faceOffset + ((i-part.OffsetIndices)/3)] = face;
                }

                vertexOffset += part.VerticesCount;
                faceOffset += part.IndicesCount / 3;
            }

            return mesh;
        }
    }
}
