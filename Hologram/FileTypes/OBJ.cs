using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Text;
using System.Globalization;

namespace Hologram.FileTypes
{
    public class OBJ
    {
        public MeshX Mesh;

        private static Vector3 ReadVertex(ModFile file)
        {
            StringBuilder builder = new StringBuilder();
            Vector3 vertex = new Vector3();
            byte pos = 0;
            while (file.Position < file.Length)
            {
                byte read = file.ReadByte();
                if (pos != 3)
                {
                    if (read != ' ' && read != '\n') builder.Append((char)read);
                    else
                    {
                        vertex[pos] = float.Parse(builder.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        pos++;
                        builder.Clear();
                    }
                }

                if (read == '\n') return vertex;
            }

            return vertex;
        }

        private static RawFace ReadFace(ModFile file)
        {
            StringBuilder builder = new StringBuilder();
            RawFace face = new RawFace();
            byte pos = 0;
            byte type = 0;
            while (file.Position < file.Length)
            {
                byte read = file.ReadByte();
                if (read != ' ' && read != '\n' && read != '/') builder.Append((char)read);
                else
                {
                    if (!int.TryParse(builder.ToString(), out int massive))
                    { // Will return false if UVs aren't defined etc. etc.
                        massive = 0;
                    }

                    if (massive > ushort.MaxValue) throw new System.Exception("Model has too many vertices.");
                    if (massive > 0)
                    {
                        massive -= 1; // Takeaway 1 because OBJ files start indexing at 1
                    }
                    short value = (short)massive;

                    if (type == 0) // Vertex Position
                    {
                        if (pos == 0) face.vert1 = value;
                        else if (pos == 1) face.vert2 = value;
                        else if (pos == 2) face.vert3 = value;
                        else if (pos == 3) face.vert4 = value;
                    }
                    else if (type == 1) // Vertex UV
                    {
                        if (pos == 0) face.vert1UV = value;
                        else if (pos == 1) face.vert2UV = value;
                        else if (pos == 2) face.vert3UV = value;
                        else if (pos == 3) face.vert4UV = value;
                    }
                    else if (type == 2) // Vertex Normal
                    {
                        if (pos == 0) face.vert1Norm = value;
                        else if (pos == 1) face.vert2Norm = value;
                        else if (pos == 2) face.vert3Norm = value;
                        else if (pos == 3) face.vert4Norm = value;
                    }

                    builder.Clear();
                }

                if (read == '/')
                {
                    type++;
                }
                else if (read == ' ')
                {
                    type = 0;
                    pos++;
                    builder.Clear();
                }
                else if (read == '\n')
                {
                    return face;
                }
            }

            return face;
        }

        /// <summary>
        /// Reads "vt ..." line (UV coords)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Vector2 ReadUV(ModFile file)
        {
            Vector2 result = new Vector2();
            bool xSet = false;
            StringBuilder builder = new StringBuilder();

            while (file.Position < file.Length)
            {
                byte read = file.ReadByte();
                if (read != ' ' && read != '\n')
                {
                    builder.Append((char)read);
                }
                else
                {
                    if (xSet)
                    {
                        result.Y = float.Parse(builder.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        if (read != '\n') ReadThrough(file);
                        return result;
                    }
                    else
                    {
                        result.X = float.Parse(builder.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        xSet = true;
                        builder.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reads "vn ..." line (Normal)
        /// </summary>
        /// <param name="file"></param>
        private static Vector3 ReadNormal(ModFile file)
        {
            Vector3 result = new Vector3();
            byte pos = 0;
            StringBuilder builder = new StringBuilder();

            while (file.Position < file.Length)
            {
                byte read = file.ReadByte();
                if (read != ' ' && read != '\n') builder.Append((char)read);
                else if (pos < 3)
                {
                    result[pos] = float.Parse(builder.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    builder.Clear();
                    pos++;
                }

                if (read == '\n') return result;
            }

            return result;
        }

        /// <summary>
        /// Line not implemented, just read to the end.
        /// </summary>
        /// <param name="file"></param>
        private static void ReadThrough(ModFile file)
        {
            while (file.Position < file.Length)
            {
                if (file.ReadByte() == '\n')
                {
                    return;
                }
            }
        }

        static List<Vector3> rawVertices;
        static List<RawFace> rawFaces;
        static List<Vector2> rawUV;
        static List<Vector3> rawNormals;

        public static OBJ Parse(string fileLocation)
        {
            rawVertices = new List<Vector3>();
            rawFaces = new List<RawFace>();
            rawUV = new List<Vector2>();
            rawNormals = new List<Vector3>();

            OBJ obj = new OBJ();

            using (ModFile file = ModFile.Open(fileLocation))
            {
                while (file.Position < file.Length)
                {
                    byte letter = file.ReadByte();
                    if (letter == 'v')
                    {
                        byte next = file.ReadByte();
                        if (next == ' ')
                        {
                            rawVertices.Add(ReadVertex(file));
                        }
                        else if (next == 't')
                        {
                            file.ReadByte(); // ' '
                            rawUV.Add(ReadUV(file));
                        }
                        else if (next == 'n')
                        {
                            file.ReadByte(); // ' '
                            rawNormals.Add(ReadNormal(file));
                        }
                        else
                        {
                            ReadThrough(file);
                        }
                    }
                    else if (letter == 'f')
                    {
                        byte next = file.ReadByte();
                        if (next == ' ')
                        {
                            rawFaces.Add(ReadFace(file));
                        }
                        else
                        {
                            ReadThrough(file);
                        }
                    }
                    else
                    {
                        ReadThrough(file);
                    }
                }

                // The capacities applied to these elements are solely a starting point, they will exceed this value.
                vertices = new List<Vertex>(rawVertices.Count);
                vertDictionary = new Dictionary<ushort, List<VertexID>>(rawVertices.Count);
                List<ushort> indices = new List<ushort>(rawFaces.Count * 3);
                if (rawUV.Count == 0)
                {
                    rawUV.Add(Vector2.Zero); // Dummy value for when UVs are not exported in OBJ
                }

                for (int i = 0; i < rawFaces.Count; i++)
                {
                    ushort vert1 = GetVertIndex(rawFaces[i].vert1, rawFaces[i].vert1UV, rawFaces[i].vert1Norm);
                    ushort vert2 = GetVertIndex(rawFaces[i].vert2, rawFaces[i].vert2UV, rawFaces[i].vert2Norm);
                    ushort vert3 = GetVertIndex(rawFaces[i].vert3, rawFaces[i].vert3UV, rawFaces[i].vert3Norm);
                    ushort vert4 = rawFaces[i].vert4 == 0 ? (ushort)0xffff : GetVertIndex(rawFaces[i].vert4, rawFaces[i].vert4UV, rawFaces[i].vert4Norm);

                    indices.Add(vert1);
                    indices.Add(vert2);
                    indices.Add(vert3);
                    if (vert4 != 0xffff)
                    {
                        indices.Add(vert1);
                        indices.Add(vert3);
                        indices.Add(vert4);
                    }
                }

                obj.Mesh = new MeshX();
                obj.Mesh.Vertices = vertices.ToArray();
                obj.Mesh.Indices = indices.ToArray();

                //obj.Hash = HashCode.Combine(vertices.GetHashCode(), indices.GetHashCode()); // This is so much hassle.

                return obj;
            }
        }

        static Dictionary<ushort, List<VertexID>> vertDictionary;
        static List<Vertex> vertices;

        private static ushort GetVertIndex(short rawVertIdx, short rawUVIdx, short rawNormIdx)
        {
            ushort vertIndex = GetIndice(rawVertIdx, rawVertices.Count);
            ushort uvIndex = GetIndice(rawUVIdx, rawVertices.Count);
            ushort normIndex = GetIndice(rawNormIdx, rawVertices.Count);

            if (vertDictionary.ContainsKey(vertIndex))
            {
                List<VertexID> list = vertDictionary[vertIndex];
                for (ushort i = 0; i < list.Count; i++)
                {
                    if (list[i].UV == uvIndex) return list[i].Index;
                }

                ushort id = (ushort)vertices.Count;
                list.Add(new VertexID(uvIndex, id));
                Vertex vertex = new Vertex(rawVertices[vertIndex], rawNormals[normIndex], rawUV[uvIndex], Color4.White);
                vertices.Add(vertex);

                return id;
            }
            else
            {
                List<VertexID> newList = new();
                ushort id = (ushort)vertices.Count;
                newList.Add(new VertexID(uvIndex, id));
                vertDictionary.Add(vertIndex, newList);
                Vertex vertex = new Vertex(rawVertices[vertIndex], rawNormals[normIndex], rawUV[uvIndex], Color4.White);
                vertices.Add(vertex);

                return id;
            }
        }

        /// <summary>
        /// Inverses the indice if it is negative.
        /// </summary>
        /// <param name="uncorrected">The raw indice that is read</param>
        /// <param name="vertexCount">The total number of vertices</param>
        /// <returns>Corrected indice</returns>
        private static ushort GetIndice(short uncorrected, int vertexCount)
        {
            if (uncorrected >= 0) return (ushort)uncorrected;

            return (ushort)(vertexCount + uncorrected);
        }

        private struct RawFace
        {
            public short vert1;
            public short vert2;
            public short vert3;
            public short vert4;
            public short vert1UV;
            public short vert2UV;
            public short vert3UV;
            public short vert4UV;
            public short vert1Norm;
            public short vert2Norm;
            public short vert3Norm;
            public short vert4Norm;
        }
    }

    internal class VertexID
    {
        public ushort UV;
        public ushort Index;

        public VertexID(ushort uv, ushort index)
        {
            UV = uv;
            Index = index;
        }
    }
}
