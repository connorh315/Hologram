using System;
using System.IO;
using Hologram.Objects;
using ModLib;
using Hologram.FileTypes.GSC;
using System.Collections.Generic;
using OpenTK.Mathematics;
using Half = System.Half;

namespace Hologram.FileTypes.GSCWriter
{
    public class Dimensions
    {
        /// <summary>
        /// Writes custom meshes to the respective parts in the given GSC.
        /// </summary>
        /// <param name="gscLocation">The location of the GSC on the filesystem</param>
        /// <param name="meshes">The meshes to override with</param>
        public static void Write(string gscLocation, Mesh[] meshes)
        {
            using (ModFile file = ModFile.Open(gscLocation))
            {
                uint mainOffset = file.ReadUint(true);
                file.Seek(mainOffset, SeekOrigin.Current);

                uint mainSize = file.ReadUint(true); // The size of the main block of the file.
                uint version = file.ReadUint(true); // Usually just a 1

                file.CheckString("02UN", Locale.GSCStrings.ExpectedNU20);
                file.CheckInt(0x4F, Locale.GSCStrings.ExpectedNU20Version);

                file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO);
                uint stringCount = file.ReadUint(true);
                for (int i = 0; i < stringCount; i++)
                {
                    file.ReadPascalString();
                }

                file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL);
                file.CheckInt(0x4F, Locale.GSCStrings.ExpectedNTBLVersion);

                file.Seek(file.ReadUint(true), SeekOrigin.Current); // Skip past the default_string
                file.Seek(0x18, SeekOrigin.Current); // blank ROTV sectors

                file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH);
                file.CheckInt(0xaf, Locale.GSCStrings.ExpectedMESHVersion);

                uint partCount = file.ReadUint(true);

                long beginParts = file.Position;

                Logger.Log(new LogSeg("Preparing..."));

                Dictionary<int, Reference> listReferences = new();
                int referenceId = 5;

                PartData[] parts = new PartData[partCount];

                for (int partId = 0; partId < partCount; partId++)
                {
                    uint vtxdVersion = file.ReadUint(true); // 1
                    uint listCount = file.ReadUint(true);

                    PartData part = new PartData(listCount, meshes[partId]);
                    parts[partId] = part;

                    for (int listId = 0; listId < listCount; listId++)
                    {
                        Reference vtxReference;
                        uint vtxMarker = file.ReadUint(true);
                        if ((vtxMarker & 0xc0000000) != 0)
                        {
                            vtxMarker &= 0xffff;
                            part.VertexReferences[listId] = vtxMarker;
                            vtxReference = listReferences[(int)vtxMarker];
                            vtxReference.Parts.Add(part);
                            file.Seek(8, SeekOrigin.Current);
                            continue;
                        }

                        part.VertexReferences[listId] = (uint)referenceId;
                        vtxReference = new Reference();
                        listReferences[referenceId] = vtxReference;
                        vtxReference.Parts.Add(part);
                        referenceId++;

                        uint unknown = file.ReadUint(true); // Usually 0x502 or 0x202 or 0x102

                        uint vtxCount = file.ReadUint(true);
                        file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
                        file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion);

                        uint defCount = file.ReadUint(true);
                        VertexDefinition[] definitions = new VertexDefinition[defCount];
                        for (int defId = 0; defId < defCount; defId++)
                        {
                            VertexDefinition def = new VertexDefinition();
                            def.Variable = (VertexDefinition.VariableEnum)file.ReadByte();
                            def.VariableType = (VertexDefinition.StorageTypeEnum)file.ReadByte();
                            def.Offset = file.ReadByte();
                            definitions[defId] = def;
                        }
                        vtxReference.Definitions = definitions;

                        file.Seek(6, SeekOrigin.Current); // 6 bytes of zero padding

                        VertexDefinition lastDef = definitions[defCount - 1];
                        file.Seek((lastDef.Offset + storageSize[(byte)lastDef.VariableType]) * vtxCount, SeekOrigin.Current); // skip past the whole list
                        file.Seek(4, SeekOrigin.Current); // 0
                    }

                    file.Seek(4, SeekOrigin.Current); // 0

                    Reference indicesReference;
                    uint indicesMarker = file.ReadUint(true);
                    if ((indicesMarker & 0xc0000000) != 0)
                    {
                        indicesMarker &= 0xffff;
                        file.Seek(4, SeekOrigin.Current); // 1

                        part.IndicesReference = indicesMarker;
                        indicesReference = listReferences[(int)indicesMarker];
                        indicesReference.Parts.Add(part);
                    }
                    else
                    {
                        part.IndicesReference = (uint)referenceId;
                        indicesReference = new Reference();
                        listReferences[referenceId] = indicesReference;
                        indicesReference.Parts.Add(part);
                        referenceId++;

                        file.Seek(4, SeekOrigin.Current); // 2
                        uint indicesCount = file.ReadUint(true);
                        file.Seek(4, SeekOrigin.Current); // 2

                        file.Seek(indicesCount * 2, SeekOrigin.Current);
                    }


                    file.Seek(70, SeekOrigin.Current);

                    referenceId += 2;
                }

                Logger.Log(new LogSeg("Completed preparation. Beginning importing..."));

                referenceId = 5; // reset

                file.Seek(beginParts, SeekOrigin.Begin);
                for (int partId = 0; partId < partCount; partId++)
                {
                    PartData part = parts[partId];

                    uint vtxdVersion = file.ReadUint(true);
                    uint listCount = file.ReadUint(true);

                    for (int listId = 0; listId < listCount; listId++)
                    {
                        uint vtxMarker = file.ReadUint(true);
                        if ((vtxMarker & 0xc0000000) != 0)
                        {
                            file.Seek(8, SeekOrigin.Current); // two unknown ints
                            continue;
                        }

                        Reference vtxReference = listReferences[referenceId];
                        referenceId++;

                        uint unknown = file.ReadUint(true); // Usually 0x502 or 0x202 or 0x102

                        bool shouldClearVtx = true; // Should remove all existing data.

                        int totalVertices = 0;
                        for (int meshId = 0; meshId < vtxReference.Parts.Count; meshId++)
                        {
                            if (vtxReference.Parts[meshId].PartMesh == null)
                            {
                                shouldClearVtx = false;
                                continue;
                            }
                            totalVertices += vtxReference.Parts[meshId].PartMesh.VertexCount;
                        }

                        uint origVtxCount = file.ReadUint(true);
                        file.Seek(-4, SeekOrigin.Current);
                        file.WriteInt(shouldClearVtx ? totalVertices : (int)(totalVertices + origVtxCount), true);

                        file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
                        file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion);

                        VertexDefinition[] definitions = vtxReference.Definitions;
                        file.Seek(10 + (3 * definitions.Length), SeekOrigin.Current); // Skip past definition header
                        ModFile remainder;
                        if (shouldClearVtx)
                        {
                            long origPosition = file.Position;
                            remainder = file.LoadSegment(file.Position, (int)(file.Length - file.Position));
                            file.Seek(origPosition, SeekOrigin.Begin);
                        }
                        else
                        {
                            VertexDefinition lastDef = definitions[vtxReference.Definitions.Length - 1];
                            file.Seek((lastDef.Offset + storageSize[(byte)lastDef.VariableType]) * origVtxCount, SeekOrigin.Current);
                            long origPosition = file.Position;
                            remainder = file.LoadSegment(file.Position, (int)(file.Length - file.Position));
                            file.Seek(origPosition, SeekOrigin.Begin);
                        }

                        uint vertOffset = shouldClearVtx ? 0 : origVtxCount;
                        for (int meshId = 0; meshId < vtxReference.Parts.Count; meshId++)
                        {
                            Mesh mesh = vtxReference.Parts[meshId].PartMesh;
                            if (mesh == null) continue;
                            vtxReference.Parts[meshId].VerticesOffset = vertOffset;
                            vtxReference.Parts[meshId].VerticesCount = (uint)mesh.VertexCount;
                            for (int vertId = 0; vertId < mesh.VertexCount; vertId++)
                            {
                                for (int defId = 0; defId < definitions.Length; defId++)
                                {
                                    VertexDefinition def = definitions[defId];
                                    switch (def.VariableType)
                                    {
                                        case VertexDefinition.StorageTypeEnum.vec2float:
                                            vec2float(file, GetValue<Vector2>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec3float:
                                            vec3float(file, GetValue<Vector3>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec4float:
                                            vec4float(file, GetValue<Vector4>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec2half:
                                            vec2half(file, GetValue<Vector2>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec4half:
                                            vec4half(file, GetValue<Vector4>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec4char:
                                            vec4char(file, GetValue<Vector4>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.vec4mini:
                                            vec4mini(file, GetValue<Vector4>(mesh, def.Variable, vertId));
                                            break;
                                        case VertexDefinition.StorageTypeEnum.color4char:
                                            color4char(file, GetValue<Color4>(mesh, def.Variable, vertId));
                                            break;
                                    }
                                }
                            }
                            vertOffset += (uint)mesh.VertexCount;
                        }

                        long vtxOrigPosition = file.Position;
                        remainder.Seek(0, SeekOrigin.Begin);
                        remainder.fileStream.CopyTo(file.fileStream);
                        file.fileStream.SetLength(file.Position);
                        file.Seek(vtxOrigPosition, SeekOrigin.Begin);

                        file.Seek(4, SeekOrigin.Current);
                    }

                    file.Seek(4, SeekOrigin.Current);

                    Reference idcReference;
                    uint indicesMarker = file.ReadUint(true);
                    if ((indicesMarker & 0xc0000000) != 0)
                    {
                        file.Seek(4, SeekOrigin.Current);
                        if (part.IndicesCount == 0) // No changes were made to this part, so skip.
                        {
                            file.Seek(70, SeekOrigin.Current);
                        }
                        else
                        {
                            file.WriteUint(part.IndicesOffset, true);
                            file.WriteUint(part.IndicesCount, true);
                            file.WriteUint(part.VerticesOffset, true);
                            file.Seek(2, SeekOrigin.Current);
                            file.WriteUint(part.VerticesCount, true);
                            file.Seek(48, SeekOrigin.Current);
                        }

                        referenceId += 2;
                        continue;
                    }

                    idcReference = listReferences[referenceId];
                    referenceId++;

                    file.Seek(4, SeekOrigin.Current); // 2

                    bool shouldClearIdc = true; // Should remove all existing data.

                    int totalIndices = 0;
                    for (int meshId = 0; meshId < idcReference.Parts.Count; meshId++)
                    {
                        if (idcReference.Parts[meshId].PartMesh == null)
                        {
                            shouldClearIdc = false;
                            continue;
                        }
                        totalIndices += idcReference.Parts[meshId].PartMesh.FaceCount;
                    }
                    totalIndices *= 3;
                    uint origIdcCount = file.ReadUint(true);
                    file.Seek(-4, SeekOrigin.Current);
                    file.WriteInt(shouldClearIdc ? totalIndices : (int)(totalIndices + origIdcCount), true);

                    file.Seek(4, SeekOrigin.Current); // 2

                    ModFile idcRemainder;
                    if (shouldClearIdc)
                    {
                        long origPosition = file.Position;
                        idcRemainder = file.LoadSegment(file.Position, (int)(file.Length - file.Position));
                        file.Seek(origPosition, SeekOrigin.Begin);
                    }
                    else
                    {
                        file.Seek(2 * origIdcCount, SeekOrigin.Current);
                        long origPosition = file.Position;
                        idcRemainder = file.LoadSegment(file.Position, (int)(file.Length - file.Position));
                        file.Seek(origPosition, SeekOrigin.Begin);
                    }

                    uint idcOffset = shouldClearIdc ? 0 : origIdcCount;
                    for (int meshId = 0; meshId < idcReference.Parts.Count; meshId++)
                    {
                        Mesh mesh = idcReference.Parts[meshId].PartMesh;
                        if (mesh == null) continue;
                        idcReference.Parts[meshId].IndicesOffset = idcOffset;
                        idcReference.Parts[meshId].IndicesCount = (uint)(mesh.FaceCount * 3);
                        for (int faceId = 0; faceId < mesh.FaceCount; faceId++)
                        {
                            Face face = mesh.Faces[faceId];
                            file.WriteUshort(face.vert1, true);
                            file.WriteUshort(face.vert2, true);
                            file.WriteUshort(face.vert3, true);
                        }

                        idcOffset += (uint)(mesh.FaceCount * 3);
                    }

                    long idcOrigPosition = file.Position;
                    idcRemainder.Seek(0, SeekOrigin.Begin);
                    idcRemainder.fileStream.CopyTo(file.fileStream);
                    file.fileStream.SetLength(file.Position);
                    file.Seek(idcOrigPosition, SeekOrigin.Begin);

                    if (part.IndicesCount != 0 && part.VerticesCount != 0)
                    {
                        file.WriteUint(part.IndicesOffset, true);
                        file.WriteUint(part.IndicesCount, true);
                        file.WriteUint(part.VerticesOffset, true);
                        file.Seek(2, SeekOrigin.Current); // 00
                        file.WriteUint(part.VerticesCount, true);

                        file.Seek(52, SeekOrigin.Current);
                    }
                    else
                    {
                        file.Seek(70, SeekOrigin.Current);
                    }

                    referenceId += 2;
                }

                file.Seek(4 + mainOffset, SeekOrigin.Begin);
                file.WriteUint((uint)(file.Length - file.Position - 4), true);
            }
        }

        private static T GetValue<T>(Mesh mesh, VertexDefinition.VariableEnum variable, int vertexId)
        {
            Type returnType = typeof(T);
            object value;
            switch (variable)
            {
                case VertexDefinition.VariableEnum.position:
                    value = mesh.Vertices[vertexId];
                    break;
                case VertexDefinition.VariableEnum.normal:
                    value = mesh.vertices2[vertexId].Normal;
                    break;
                case VertexDefinition.VariableEnum.colorSet0:
                    value = new Color4(255, 255, 255, 255);
                    break;
                case VertexDefinition.VariableEnum.tangent:
                    value = new Vector4(-1, -1, -1, -1);
                    break;
                case VertexDefinition.VariableEnum.uvSet01:
                    value = Vector2.Zero;
                    break;
                case VertexDefinition.VariableEnum.uvSet2:
                    value = Vector2.Zero;
                    break;
                default:
                    throw new Exception("Original GSC uses unknown variable type");

            }

            Type valueType = value.GetType();
            if (valueType == returnType) return (T)value;
            if (valueType == typeof(Vector2) && returnType == typeof(Vector3)) return (T)(object)new Vector3((Vector2)value);
            if (valueType == typeof(Vector2) && returnType == typeof(Vector4)) return (T)(object)new Vector4((Vector2)value);
            if (valueType == typeof(Vector3) && returnType == typeof(Vector4)) return (T)(object)new Vector4((Vector3)value, 1);

            throw new Exception($"Reached end of GetValue and could not identify a suitable cast from {valueType} to {returnType} for a {variable}");
        }

        private static byte GetMinified(float toMinify)
        {
            return (byte)Math.Clamp((toMinify + 1) * 128, 0, 255);
        }

        private static byte[] storageSize = new byte[] { 0, 0, 8, 12, 16, 4, 8, 4, 4, 4 }; // Check the vec4char one

        private static void vec2float(ModFile file, Vector2 data)
        {
            file.WriteFloat(data.X, false); // I think it's little endian
            file.WriteFloat(data.Y, false);
        }

        private static void vec3float(ModFile file, Vector3 data)
        {
            file.WriteFloat(data.X, false);
            file.WriteFloat(data.Y, false);
            file.WriteFloat(data.Z, false);
        }

        private static void vec4float(ModFile file, Vector4 data)
        {
            file.WriteFloat(data.X, false);
            file.WriteFloat(data.Y, false);
            file.WriteFloat(data.Z, false);
            file.WriteFloat(data.W, false);
        }

        private static void vec2half(ModFile file, Vector2 data)
        {
            file.WriteHalf((Half)data.X, false);
            file.WriteHalf((Half)data.Y, false);
        }

        private static void vec4half(ModFile file, Vector4 data)
        {
            file.WriteHalf((Half)data.X, true);
            file.WriteHalf((Half)data.Y, true); 
            file.WriteHalf((Half)data.Z, true);
            file.WriteHalf((Half)data.W, true);
        }

        private static void vec4char(ModFile file, Vector4 data)
        {
            file.WriteUint(0xffffffff); // Couldn't find out what vec4char actually means
        }

        private static void vec4mini(ModFile file, Vector4 data)
        {
            file.WriteByte(GetMinified(data.X));
            file.WriteByte(GetMinified(data.Y));
            file.WriteByte(GetMinified(data.Z));
            file.WriteByte(GetMinified(data.W));
        }

        private static void color4char(ModFile file, Color4 data)
        {
            file.WriteByte((byte)(data.R * 255)); // High likelihood this should be BGRA
            file.WriteByte((byte)(data.G * 255));
            file.WriteByte((byte)(data.B * 255));
            file.WriteByte((byte)(data.A * 255));
        }
    }

    public class Reference
    {
        public List<PartData> Parts = new();
        public bool IsVertexReference = true;

        public VertexDefinition[] Definitions;
    }

    public class PartData
    {
        public uint[] VertexReferences;
        public uint IndicesReference;

        public uint IndicesOffset;
        public uint IndicesCount;

        public uint VerticesOffset;
        public uint VerticesCount;

        public Mesh PartMesh;

        public PartData(uint vertexReferenceCount, Mesh partMesh)
        {
            VertexReferences = new uint[vertexReferenceCount];
            PartMesh = partMesh;
        }
    }
}
