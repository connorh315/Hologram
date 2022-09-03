using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Text;

namespace Hologram.FileTypes
{
    public class OBJ
    {
        public Mesh PhysicsMesh;

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
                        vertex[pos] = float.Parse(builder.ToString());
                        pos++;
                        builder.Clear();
                    }
                }


                if (read == '\n') return vertex;
            }

            return vertex;
        }

        private static Face ReadFace(ModFile file)
        {
            StringBuilder builder = new StringBuilder();
            Face face = new Face();
            byte pos = 0;
            bool ignore = false;
            while (file.Position < file.Length)
            {
                byte read = file.ReadByte();
                if (read != ' ' && read != '\n' && read != '/') builder.Append((char)read);
                else if (!ignore)
                {
                    ushort value = (ushort)(ushort.Parse(builder.ToString()) - 1); // Takeaway 1 because OBJ files start indexing at 1
                    if (pos == 0)
                    {
                        face.vert1 = value;
                    }
                    else if (pos == 1)
                    {
                        face.vert2 = value;
                    }
                    else if (pos == 2)
                    {
                        face.vert3 = value;
                    }
                    else if (pos == 3)
                    {
                        face.vert4 = value;
                    }
                    
                    if (pos != 4)
                    {
                        pos++;
                        builder.Clear();
                    }
                }

                if (read == '/')
                {
                    ignore = true;
                }
                else if (read == ' ')
                {
                    ignore = false;
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

        public static OBJ Parse(string fileLocation)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Face> faces = new List<Face>();

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
                            vertices.Add(ReadVertex(file));
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
                            faces.Add(ReadFace(file));
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

                obj.PhysicsMesh = new Mesh((uint)vertices.Count, (uint)faces.Count, faces[0].vert4 != 0 ? FaceType.Quads : FaceType.Triangles);
                obj.PhysicsMesh.Faces = faces.ToArray();
                obj.PhysicsMesh.Vertices = vertices.ToArray();

                return obj;
            }
        }
    }
}
