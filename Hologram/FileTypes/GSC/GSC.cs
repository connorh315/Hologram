using ModLib;
using Hologram.Objects;
using System.Collections.Generic;

namespace Hologram.FileTypes.GSC
{
    public partial class GSC
    {
        private ModFile file;

        internal int referenceCounter = 5;

        private Dictionary<int, VertexList> vertexLists = new Dictionary<int, VertexList>();

        private Dictionary<int, ushort[]> indexLists = new Dictionary<int, ushort[]>();

        public MeshX Mesh { get; private set; }

        public Part[] parts;

        public Entity[] entities;
    }
}
