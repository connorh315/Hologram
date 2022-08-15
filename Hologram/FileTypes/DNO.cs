using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Text;

namespace Hologram.FileTypes
{
    /// <summary>
    /// Physics collision FileType
    /// </summary>
    public class DNO
    {
        public Mesh PhysicsMesh;

        public static DNO Parse(string fileLocation, uint tempFileOffset)
        {
            DNO dnoFile = new DNO();
            using (ModFile file = ModFile.Open(fileLocation))
            {
                file.Seek(tempFileOffset, SeekOrigin.Begin);

                uint vertexCount = file.ReadUint(true);
                uint anotherCount = file.ReadUint(true);
                uint faceCount = file.ReadUint(true);
                Mesh mesh = new Mesh(vertexCount, faceCount, FaceType.Quads);
                dnoFile.PhysicsMesh = mesh;

                uint anotherCount2 = file.ReadUint(true);
                uint anotherCount3 = file.ReadUint(true);

                StringBuilder str = new StringBuilder();
                for (int i = 0; i < vertexCount; i++)
                {
                    mesh.Vertices[i] = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                    str.AppendLine(string.Format("v {0} {1} {2}", mesh.Vertices[i].X, mesh.Vertices[i].Y + 2, mesh.Vertices[i].Z));
                }
                Dictionary<ushort, int> count = new();
                string face = "f";
                for (int i = 0; i < faceCount; i++)
                {
                    Face thisFace = new Face();
                    thisFace.vert1 = (ushort)file.ReadUshort(true);
                    thisFace.vert2 = (ushort)file.ReadUshort(true);
                    thisFace.vert3 = (ushort)file.ReadUshort(true);
                    thisFace.vert4 = (ushort)file.ReadUshort(true);
                    thisFace.vert5 = (ushort)file.ReadUshort(true);
                    if (!count.ContainsKey(thisFace.vert5))
                    {
                        count.Add(thisFace.vert5, 0);
                    }

                    count[thisFace.vert5]++;

                    str.AppendLine(String.Format("f {0} {1} {2}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));
                    str.AppendLine(String.Format("f {0} {2} {3}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));

                    if (Vector3.Cross(mesh.Vertices[thisFace.vert4] - mesh.Vertices[thisFace.vert3], mesh.Vertices[thisFace.vert5] - mesh.Vertices[thisFace.vert3]).Length <= 0.4)
                    {
                        str.AppendLine(String.Format("f {2} {3} {4}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));
                    }
                    else
                    {
                        //Console.WriteLine("{0} {1} {2} {3} {4}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1);
                    }

                    //face += " " + (thisFace.vert5 + 1);
                    //count++;
                    //if (count == 3)
                    //{
                    //    str.AppendLine(face);
                    //    count = 0;
                    //    face = "f";
                    //}
                    mesh.Faces[i] = thisFace;
                }

                //foreach (KeyValuePair<ushort, int> keyValuePair in count)
                //{
                //    if (keyValuePair.Value > 10)
                //    {
                //        Console.WriteLine("{0} - {1}", keyValuePair.Key, keyValuePair.Value);
                //    }
                //}

                for (int i = 0; i <= anotherCount; i++)
                {
                    str.AppendLine(string.Format("v {0} {1} {2}", file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true)));
                    file.ReadInt(true);
                }

                for (int i = 0; i < faceCount; i++)
                {
                    Face thisFace = new Face();
                    thisFace.vert1 = (ushort)(file.ReadUshort(true) - 32768);
                    thisFace.vert2 = (ushort)(file.ReadUshort(true) - 32768);
                    thisFace.vert3 = (ushort)(file.ReadUshort(true) - 32768);
                    thisFace.vert4 = (ushort)(file.ReadUshort(true) - 32768);
                }

                File.WriteAllText(@"A:\dno.obj", str.ToString());

                Console.WriteLine("Done reading at: {0}", file.Position);
            }
            return dnoFile;
        }
    }
}
