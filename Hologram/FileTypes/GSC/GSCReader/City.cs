using ModLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.FileTypes.GSC.GSCReader
{
    public class City
    {
        public static void Read(ModFile file, GSC gsc)
        {
            file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO);

            Logger.Log("Metadata:");
            uint stringsCount = file.ReadUint(true);
            for (int i = 0; i < stringsCount; i++)
            {
                Logger.Log(new LogSeg(file.ReadBigPascalString(), ConsoleColor.Gray));
            }

            Logger.Log(new LogSeg(file.ReadBigPascalString(), ConsoleColor.Gray)); // Conversion date

            file.ReadByte(); // 1

            file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL);
            int ntblVersion = file.ReadInt(true);
            if (ntblVersion != 0x43)
            {
                Logger.Error(Locale.GSCStrings.ExpectedNTBLVersion);
            }

            file.Seek(file.ReadInt(true), SeekOrigin.Current); // Big blob of strings

            file.Seek(24, SeekOrigin.Current); // ROTV

            int textureCount = file.ReadInt(true);
            for (int textureId = 0; textureId < textureCount; textureId++)
            {
                file.Seek(16, SeekOrigin.Current);
                file.ReadBigPascalString();
                file.Seek(4, SeekOrigin.Current);
            }

            file.Seek(0x1e, SeekOrigin.Current);

            file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH);
            file.CheckInt(0xA9, Locale.GSCStrings.ExpectedMESHVersion);
            file.Seek(4, SeekOrigin.Current); // ROTV

            uint partCount = file.ReadUint(true);
            gsc.parts = new Part[partCount];
            for (int partId = 0; partId < partCount; partId++)
            {
                uint one = file.ReadUint(true);

                uint vertexListCount = file.ReadUint(true);

                Part part = new Part(vertexListCount);
                gsc.parts[partId] = part;
                for (int vertexListId = 0; vertexListId < vertexListCount; vertexListId++)
                {
                    part.VertexListReferences[vertexListId] = gsc.GetVertexListReference();
                }
                file.Seek(4, SeekOrigin.Current);
                part.IndexListReference = gsc.GetIndexListReference();
                //Console.WriteLine("part offset: " + file.Position);
                part.OffsetIndices = file.ReadInt(true);
                part.IndicesCount = file.ReadInt(true);
                part.OffsetVertices = file.ReadInt(true);

                //Console.WriteLine(part.OffsetIndices);
                //Console.WriteLine(part.IndicesCount);
                //Console.WriteLine(part.OffsetVertices);

                if (file.ReadShort(true) != 0) { throw new Exception("ReadPart Offset Vertices + 4"); }

                part.VerticesCount = file.ReadInt(true);

                //Console.WriteLine(part.VerticesCount);

                file.Seek(4, SeekOrigin.Current);

                int num = file.ReadInt(true);
                if (num > 0)
                {
                    Logger.Warn("Using assumption XXXXXX");
                    file.Seek(num, SeekOrigin.Current);
                }

                int num2 = file.ReadInt(true);
                if (num2 > 0)
                {
                    int num3 = gsc.ReadRelativePositionList();
                    gsc.referenceCounter += num3;
                }

                file.Seek(40, SeekOrigin.Current);

                gsc.referenceCounter += 2;
            }
        }
    }
}
