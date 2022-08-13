using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;

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
                Mesh mesh = new Mesh(vertexCount, faceCount);
                dnoFile.PhysicsMesh = mesh;

                uint anotherCount2 = file.ReadUint(true);
                uint anotherCount3 = file.ReadUint(true);

                for (int i = 0; i < vertexCount; i++)
                {
                    mesh.Vertices[i] = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                }

                Console.WriteLine("Done reading at: {0}", file.Position);

                for (int i = 0; i < faceCount; i++)
                {
                    Face thisFace = new Face();
                    thisFace.vert1 = (ushort)file.ReadUshort(true);
                    thisFace.vert2 = (ushort)file.ReadUshort(true);
                    thisFace.vert3 = (ushort)file.ReadUshort(true);
                    thisFace.vert4 = (ushort)file.ReadUshort(true);
                    thisFace.vert5 = (ushort)file.ReadUshort(true);
                    mesh.Faces[i] = thisFace;
                }
            }
            return dnoFile;
        }
    }
}
