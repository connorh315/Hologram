using Hologram.Objects;
using ModLib;
using Hologram.FileTypes.GSCNew;
using OpenTK.Mathematics;

using Half = System.Half;

namespace Hologram.FileTypes
{
    public static class GSCWrite
    {
        public static void Write(Mesh meshToWrite, string gscLocation)
        {
            using (ModFile file = ModFile.Open(gscLocation))
            {
                Console.WriteLine("About to write {0} vertices", meshToWrite.VertexCount);
                Console.WriteLine("About to write {0} indices", meshToWrite.FaceCount * 3);

                int mainOffset = file.ReadInt(true);

                file.Seek(mainOffset, SeekOrigin.Current);

                int mainSize = file.ReadInt(true);

                int one = file.ReadInt(true);

                file.CheckString("02UN", "Expected NU20");

                file.Seek(4, SeekOrigin.Current);

                file.CheckString("OFNI", "Expected INFO");

                int stringCount = file.ReadInt(true);

                for (int i = 0; i < stringCount; i++)
                {
                    Logger.Log(file.ReadPascalString());
                }

                file.CheckString("LBTN", "Expected NTBL");

                file.Seek(4, SeekOrigin.Current);

                file.Seek(file.ReadInt(true) + 24, SeekOrigin.Current);

                file.CheckString("HSEM", "Expected MESH");
                file.CheckInt(0xAF, "Expected 0xAF");

                uint partCount = file.ReadUint(true);
                uint vtxdVersion = file.ReadUint(true);
                uint vtxdCount = file.ReadUint(true);

                uint somethingIdk = file.ReadUint(true);

                uint unknown = file.ReadUint(true);

                long vertexCountPosition = file.Position;
                uint vertexCount = file.ReadUint(true);

                file.CheckString("DXTV", "Expected VTXD");
                file.CheckInt(0xA9, "Expected 0xA9");

                uint definitions = file.ReadUint(true);
                if (definitions != 1) throw new Exception("Too complex");

                byte firstDef = file.ReadByte();
                if (firstDef != 0) throw new Exception("Not position");

                byte storage = file.ReadByte();
                if (storage != 6) throw new Exception("Not implemented");

                byte offset = file.ReadByte();

                file.Seek(6, SeekOrigin.Current);

                file.Seek(8 * vertexCount, SeekOrigin.Current);

                file.Seek(4, SeekOrigin.Current); // 0

                uint vtxdVersion2 = file.ReadUint(true);

                uint somethingIdk2 = file.ReadUint(true);

                uint vertexCount2 = file.ReadUint(true);

                file.CheckString("DXTV", "Expected VTXD");
                file.CheckInt(0xA9, "Expected 0xA9");

                uint definitions2 = file.ReadUint(true);
                VertexDefinition[] allDefinitions = new VertexDefinition[definitions2];
                for (int i = 0; i < definitions2; i++)
                {
                    VertexDefinition def = new VertexDefinition();
                    def.Variable = (VertexDefinition.VariableEnum)file.ReadByte();
                    def.VariableType = (VertexDefinition.StorageTypeEnum)file.ReadByte();
                    def.Offset = file.ReadByte();
                    allDefinitions[i] = def;
                }

                file.Seek(6, SeekOrigin.Current);

                file.Seek(4, SeekOrigin.Current); // seek past normal

                byte[] vertData = file.ReadArray(12);

                file.Seek(16 * (vertexCount2 - 1), SeekOrigin.Current);

                uint indices1 = file.ReadUint(true);
                uint indices2 = file.ReadUint(true);
                uint indices3 = file.ReadUint(true);
                uint indices4 = file.ReadUint(true);

                uint indicesCount = file.ReadUint(true);

                uint unknownX = file.ReadUint(true);

                file.Seek(2 * indicesCount, SeekOrigin.Current);

                file.Seek(18, SeekOrigin.Current); // indicescount, indicesoffset ect. ect.

                long originalFinish = file.Position;

                ModFile endOfFile = file.LoadSegment(file.Position, (int)(file.Length - file.Position));

                // LET THE PARTY START

                file.Seek(vertexCountPosition, SeekOrigin.Begin);

                file.WriteInt(meshToWrite.VertexCount, true);

                file.Seek(8, SeekOrigin.Current); // DXTV, 0XA9

                file.Seek(7 + 6, SeekOrigin.Current); // The definitions crap

                for (int i = 0; i < meshToWrite.VertexCount; i++)
                {
                    Vector3 pos = meshToWrite.Vertices[i];

                    file.WriteHalf((Half)pos.X, true);
                    file.WriteHalf((Half)pos.Y, true);
                    file.WriteHalf((Half)pos.Z, true);
                    file.WriteUshort(0x3c00, true);
                }

                file.WriteInt(0, true);

                file.WriteUint(vtxdVersion2, true);

                file.WriteUint(somethingIdk2, true);

                file.WriteInt(meshToWrite.VertexCount, true);

                file.WriteString("DXTV");
                file.WriteInt(0xA9, true);

                file.WriteUint(definitions2, true);

                for (int i = 0; i < definitions2; i++)
                {
                    VertexDefinition definition = allDefinitions[i];
                    file.WriteByte((byte)definition.Variable);
                    file.WriteByte((byte)definition.VariableType);
                    file.WriteByte((byte)definition.Offset);
                }

                file.WritePadding(6);

                for (int i = 0; i < meshToWrite.VertexCount; i++)
                {
                    Vector3 norm = meshToWrite.vertices2[i].Normal;
                    file.WriteByte(GetMinified(norm.X));
                    file.WriteByte(GetMinified(norm.Y));
                    file.WriteByte(GetMinified(norm.Z));
                    file.WriteByte(0xFF);
                    file.fileStream.Write(vertData);
                }

                file.WriteUint(indices1, true);
                file.WriteUint(indices2, true);
                file.WriteUint(indices3, true);
                file.WriteUint(indices4, true);

                file.WriteInt(meshToWrite.FaceCount * 3, true);

                file.WriteUint(unknownX, true);

                for (int i = 0; i < meshToWrite.FaceCount; i++)
                {
                    file.WriteUshort(meshToWrite.Faces[i].vert1, true);
                    file.WriteUshort(meshToWrite.Faces[i].vert2, true);
                    file.WriteUshort(meshToWrite.Faces[i].vert3, true);
                }

                file.WriteInt(0, true);
                file.WriteInt(meshToWrite.FaceCount * 3, true);
                file.WriteInt(0, true);

                file.WriteShort(0);

                file.WriteInt(meshToWrite.VertexCount, true);

                long finish = file.Position;
                
                endOfFile.Seek(0, SeekOrigin.Begin);
                endOfFile.fileStream.CopyTo(file.fileStream);
                
                file.Seek(mainOffset + 4, SeekOrigin.Begin);
                file.WriteInt((int)(mainSize + finish - originalFinish), true);

                Console.WriteLine("INCREASED BY " + (finish - originalFinish));
            }
        }

        private static byte GetMinified(float toMinify)
        {
            toMinify = Math.Clamp(toMinify, -1f, 1f);
            return (byte)((toMinify + 1) * 128);
        }
    }
}
