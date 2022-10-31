using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC
{
    public class Part
    {
        public VertexListReference[] VertexListReferences;

        public Part(uint vertexListCount)
        {
            VertexListReferences = new VertexListReference[vertexListCount];
        }

        public int IndexListReference;

        public int OffsetIndices;
        public int IndicesCount;

        public int OffsetVertices;
        public int VerticesCount;
    }
}
