using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC
{
    public class VertexList
    {
        public Vertex[] Vertices;

        public VertexDefinition[] Definitions;

        public VertexList(uint vertexCount, uint definitionCount)
        {
            Vertices = new Vertex[vertexCount];

            Definitions = new VertexDefinition[definitionCount];
        }
    }
}
