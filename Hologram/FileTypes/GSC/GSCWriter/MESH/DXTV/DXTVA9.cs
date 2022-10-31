using ModLib;
using System.IO;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Collections.Generic;

namespace Hologram.FileTypes.GSC.GSCWriter.MESH.DXTV
{
    public class DXTVA9 : DXTV
    {
        protected override int Version => 0xA9;

        public override void Read(ModFile file, uint vertexCount)
        {
            this.file = file;
            this.vertexCount = vertexCount;

            uint defCount = file.ReadUint(true);
            definitions = new VertexDefinition[defCount];
            for (int defId = 0; defId < defCount; defId++)
            {
                VertexDefinition def = new VertexDefinition();
                def.Variable = (VertexDefinition.VariableEnum)file.ReadByte();
                def.VariableType = (VertexDefinition.StorageTypeEnum)file.ReadByte();
                def.Offset = file.ReadByte();
                definitions[defId] = def;
            }

            file.Seek(6, SeekOrigin.Current); // 6 bytes of zero padding

            VertexDefinition lastDef = definitions[defCount - 1];
            file.Seek((lastDef.Offset + StorageSize[(byte)lastDef.VariableType]) * vertexCount, SeekOrigin.Current); // skip past the whole list
            file.Seek(4, SeekOrigin.Current); // 0
        }

        public override void Write(bool shouldClear, List<PartData> parts)
        {
            file.Seek(10 + (3 * definitions.Length), SeekOrigin.Current); // Skip past definition header
            long vertexStart = file.Position;

            VertexDefinition lastDef = definitions[definitions.Length - 1];
            file.Seek((lastDef.Offset + StorageSize[(byte)lastDef.VariableType]) * vertexCount, SeekOrigin.Current);
            ModFile remainder = file.GetRemainder();
            if (shouldClear)
            {
                file.Seek(vertexStart, SeekOrigin.Begin);
            }


            uint vertOffset = shouldClear ? 0 : vertexCount;
            for (int meshId = 0; meshId < parts.Count; meshId++)
            {
                MeshX mesh = parts[meshId].PartMesh;
                if (mesh == null) continue;
                parts[meshId].VerticesOffset = vertOffset;
                parts[meshId].VerticesCount = (uint)mesh.VertexCount;
                for (int vertId = 0; vertId < mesh.VertexCount; vertId++)
                {
                    for (int defId = 0; defId < definitions.Length; defId++)
                    {
                        VertexDefinition def = definitions[defId];
                        switch (def.VariableType)
                        {
                            case VertexDefinition.StorageTypeEnum.vec2float:
                                vec2float(GetValue<Vector2>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec3float:
                                vec3float(GetValue<Vector3>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec4float:
                                vec4float(GetValue<Vector4>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec2half:
                                vec2half(GetValue<Vector2>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec4half:
                                vec4half(GetValue<Vector4>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec4char:
                                vec4char(GetValue<Vector4>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.vec4mini:
                                vec4mini(GetValue<Vector4>(mesh, def.Variable, vertId));
                                break;
                            case VertexDefinition.StorageTypeEnum.color4char:
                                color4char(GetValue<Color4>(mesh, def.Variable, vertId));
                                break;
                        }
                    }
                }
                vertOffset += (uint)mesh.VertexCount;
            }

            file.AddRemainder(remainder);

            file.Seek(4, SeekOrigin.Current);
        }
    }
}
