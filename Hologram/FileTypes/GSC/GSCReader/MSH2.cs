using ModLib;
using System;
using System.IO;

// Needs some huge abstractions, but for now this will do.

namespace Hologram.FileTypes.GSC.GSCReader;

public static class MSH2
{
    public static void Read(ModFile file, GSC gsc)
    {
        gsc.BigEndian = false;
        file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO);

        Logger.Log("Metadata:");
        uint stringsCount = file.ReadUint(true);
        for (int i = 0; i < stringsCount; i++)
        {
            Logger.Log(new LogSeg(file.ReadPascalString(), ConsoleColor.Gray));
        }
        file.ReadByte();
        file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL);
        int ntblVersion = file.ReadInt(true);
        if (ntblVersion != 0x57 && ntblVersion != 0x58)
        {
            Logger.Error(Locale.GSCStrings.ExpectedNTBLVersion);
        }

        file.Seek(file.ReadInt(true), SeekOrigin.Current); // Big blob of strings

        file.Seek(24, SeekOrigin.Current); // ROTV

        file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH);
        file.CheckInt(0xAF, Locale.GSCStrings.ExpectedMESHVersion);

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
