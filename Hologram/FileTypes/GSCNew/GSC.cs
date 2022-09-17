using ModLib;
using Hologram.Objects;

namespace Hologram.FileTypes.GSCNew
{
    public partial class GSC
    {
        private ModFile file;

        private int referenceCounter = 5;

        private Dictionary<int, VertexList> vertexLists = new Dictionary<int, VertexList>();

        private Dictionary<int, ushort[]> indexLists = new Dictionary<int, ushort[]>();

        public Mesh Mesh { get; private set; }

        private Part[] parts;
    }
}
