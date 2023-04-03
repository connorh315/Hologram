using ModLib;
using System.IO;

namespace Hologram.FileTypes.GSC.GSCWriter.NU20;

public class NU204F : NU20
{
    protected override int Version => 0x4F;

    protected override bool ReadINFO()
    {
        if (!file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO)) return false;

        uint stringCount = file.ReadUint(true);
        for (int i = 0; i < stringCount; i++)
        {
            file.ReadPascalString();
        }

        return true;
    }

    protected override bool ReadNTBL()
    {
        if (!file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL)) return false;
        if (!file.CheckInt(Version, Locale.GSCStrings.ExpectedNTBLVersion)) return false;

        int filenameLength = file.ReadInt(true);
        file.ReadString(filenameLength);

        file.Seek(0x14, SeekOrigin.Current);

        uint unknown = file.ReadUint(true);
        //if (unknown != 1)
        //{
        //    Logger.Error("Unknown value at {0:X} does not meet assumption of 1!", file.Position - 4);
        //    return false;
        //}

        return true;
    }
}
