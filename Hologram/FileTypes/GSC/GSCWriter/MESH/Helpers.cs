using Hologram.Objects;
using System.Collections.Generic;
using Hologram.FileTypes.GSC.GSCWriter.MESH.DXTV;

namespace Hologram.FileTypes.GSC.GSCWriter.MESH
{
    public class Reference
    {
        public List<PartData> Parts = new();
        public bool IsVertexReference = true;

        public DXTV.DXTV vertexList;
    }

    public class PartData
    {
        public uint[] VertexReferences;
        public uint IndicesReference;

        public uint IndicesOffset;
        public uint IndicesCount;

        public uint VerticesOffset;
        public uint VerticesCount;

        public MeshX PartMesh;

        public PartData(uint vertexReferenceCount, MeshX partMesh)
        {
            VertexReferences = new uint[vertexReferenceCount];
            PartMesh = partMesh;
        }
    }
}
