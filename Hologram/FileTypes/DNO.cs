using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Text;
using System.Collections;

namespace Hologram.FileTypes
{
    /// <summary>
    /// Physics collision FileType
    /// </summary>
    public class DNO
    {
        public Mesh PhysicsMesh;

        private static void PrintBitPattern(short value)
        {
            Console.Write((value & 32768) > 0 ? 1 : 0);
            Console.Write((value & 16384) > 0 ? 1 : 0);
            Console.Write((value & 8192) > 0 ? 1 : 0);
            Console.Write((value & 4096) > 0 ? 1 : 0);
            Console.Write(" ");
            Console.Write((value & 2048) > 0 ? 1 : 0);
            Console.Write((value & 1024) > 0 ? 1 : 0);
            Console.Write((value & 512) > 0 ? 1 : 0);
            Console.Write((value & 256) > 0 ? 1 : 0);
            Console.Write(" ");
            Console.Write((value & 128) > 0 ? 1 : 0);
            Console.Write((value & 64) > 0 ? 1 : 0);
            Console.Write((value & 32) > 0 ? 1 : 0);
            Console.Write((value & 16) > 0 ? 1 : 0);
            Console.Write(" ");
            Console.Write((value & 8) > 0 ? 1 : 0);
            Console.Write((value & 4) > 0 ? 1 : 0);
            Console.Write((value & 2) > 0 ? 1 : 0);
            Console.Write((value & 1) > 0 ? 1 : 0);
        }

        public static DNO Parse(string fileLocation, uint tempFileOffset)
        {
            DNO dnoFile = new DNO();
            using (ModFile file = ModFile.Open(fileLocation))
            {
                uint fileLength = file.ReadUint(true);

                if (file.ReadString(11) != "SERIALISED") { throw new Exception("Does not appear to be a DNO file!"); }

                uint version = file.ReadUint(true); // Not sure, (14)

                ushort nameCount = file.ReadUshort(true); // Pretty sure

                uint notSure = file.ReadUint(true);

                for (int i = 0; i < nameCount; i++)
                {
                    Logger.Log(new LogSeg(file.ReadString(file.ReadInt(true)), ConsoleColor.Gray));
                }

                ushort unknown = file.ReadUshort(true); // Not sure, (2)

                uint one = file.ReadUint(true); // Not sure, (1)

                for (int i = 0; i < unknown; i++)
                {
                    file.Seek(0x40, SeekOrigin.Current);
                }

                ushort someCount = file.ReadUshort(true); // Not sure, (2)

                uint vectorCount = file.ReadUint(true); // Not sure, (F)

                for (int i = 0; i < someCount; i++)
                {
                    file.Seek(0x4b, SeekOrigin.Current);
                }

                uint aZero = file.ReadUint(true);
                ushort aTwo = file.ReadUshort(true);
                uint aZero2 = file.ReadUint(true);
                ushort aEight = file.ReadUshort(true);
                ushort thisCount = file.ReadUshort(true);
                uint aTwo2 = file.ReadUint(true);

                for (int i = 0; i < thisCount; i++)
                {
                    Vector4 vec4 = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                    int something = file.ReadInt(true);
                    ushort value1 = file.ReadUshort(true);
                    ushort value2 = file.ReadUshort(true);
                    uint value3 = file.ReadUint(true);
                    uint value4 = file.ReadUint(true);
                    ushort value5 = file.ReadUshort(true);
                }

                uint aZeroA = file.ReadUint(true);
                ushort aOneA = file.ReadUshort(true);
                uint aZeroB = file.ReadUint(true);
                ushort aOneB = file.ReadUshort(true);
                uint aZeroC = file.ReadUint(true);
                ushort aOneC = file.ReadUshort(true);

                ushort type = file.ReadUshort(true);
                
                if (type == 9) { throw new Exception("Type == 9"); }

                ushort aZeroX = file.ReadUshort(true);
                ushort something1 = file.ReadUshort(true);
                ushort something2 = file.ReadUshort(true);
                uint something3 = file.ReadUint(true);

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
                    //Console.WriteLine(Math.Round(mesh.Vertices[i].X, 4));
                    if (Math.Round(mesh.Vertices[i].X, 4) == -2.1413d)
                    {
                        Console.WriteLine("FOUND X VALUE");
                    }
                    str.AppendLine(string.Format("v {0} {1} {2}", mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z));
                }
                Dictionary<short, int> count = new();
                for (int i = 0; i < faceCount; i++)
                {
                    Face thisFace = new Face();
                    thisFace.vert1 = (ushort)file.ReadUshort(true);
                    thisFace.vert2 = (ushort)file.ReadUshort(true);
                    thisFace.vert3 = (ushort)file.ReadUshort(true);
                    thisFace.vert4 = (ushort)file.ReadUshort(true);
                    //file.WriteUshort(0x00ff);
                    thisFace.vert5 = file.ReadUshort(true);

                    //if (thisFace.vert1 == 0x48d || thisFace.vert2 == 0x48d || thisFace.vert3 == 0x48d || thisFace.vert4 == 0x48d)
                    //{
                    //    Vector3 vert1 = mesh.Vertices[thisFace.vert1];
                    //    Vector3 normal = Vector3.Cross(mesh.Vertices[thisFace.vert3] - vert1, mesh.Vertices[thisFace.vert2] - vert1).Normalized();
                    //    Console.WriteLine(normal);
                    //    PrintBitPattern(value5);
                    //    Console.WriteLine(" " + value5);
                    //}

                    //if (!count.ContainsKey(value5))
                    //{
                    //    count.Add(value5, 0);
                    //}

                    //count[value5]++;

                    str.AppendLine(String.Format("f {0} {1} {2}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));
                    str.AppendLine(String.Format("f {0} {2} {3}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));

                    //if (Vector3.Cross(mesh.Vertices[thisFace.vert4] - mesh.Vertices[thisFace.vert3], mesh.Vertices[thisFace.vert5] - mesh.Vertices[thisFace.vert3]).Length <= 0.4)
                    //{
                    //    str.AppendLine(String.Format("f {2} {3} {4}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1));
                    //}
                    //else
                    //{
                    //    //Console.WriteLine("{0} {1} {2} {3} {4}", thisFace.vert1 + 1, thisFace.vert2 + 1, thisFace.vert3 + 1, thisFace.vert4 + 1, thisFace.vert5 + 1);
                    //}

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

                //foreach (KeyValuePair<short, int> keyValuePair in count)
                //{
                //    if (keyValuePair.Value > 10)
                //    {
                //        Console.WriteLine("{0} - {1}", keyValuePair.Key, keyValuePair.Value);
                //    }
                //}

                // From here:

                //for (int i = 0; i <= anotherCount; i++)
                //{
                //    str.AppendLine(string.Format("v {0} {1} {2}", file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true)));
                //    file.ReadInt(true);
                //}

                //for (int i = 0; i < 500; i++)
                //{
                //    short vert1 = (short)(file.ReadShort(true) & 0x81ff);
                //    short vert2 = (short)(file.ReadShort(true) & 0x81ff);
                //    short vert3 = (short)(file.ReadShort(true) & 0x81ff);
                //    short vert4 = (short)(file.ReadShort(true) & 0x81ff);
                //    short vert5 = (short)(file.ReadShort(true) & 0x81ff);

                //    //Face thisFace = new Face();
                //    //thisFace.vert1 = (ushort)(file.ReadUshort(true) & 0x00ff);
                //    //thisFace.vert2 = (ushort)(file.ReadUshort(true) & 0x00ff);
                //    //thisFace.vert3 = (ushort)(file.ReadUshort(true) & 0x00ff);
                //    //thisFace.vert4 = (ushort)(file.ReadUshort(true) & 0x00ff);
                //    //thisFace.vert5 = (ushort)(file.ReadUshort(true) & 0x00ff);

                //    //Logger.Log(new LogSeg("{0} {1} {2}", thisFace.vert1.ToString(), thisFace.vert2.ToString(), thisFace.vert3.ToString()));

                //    //str.AppendLine(String.Format("f {0} {1} {2}", vert1 + 1, vert2 + 1, vert3 + 1));
                //}

                File.WriteAllText(@"A:\dno.obj", str.ToString());

                Console.WriteLine("Done reading at: {0}", file.Position);
            }
            return dnoFile;
        }
    }
}
