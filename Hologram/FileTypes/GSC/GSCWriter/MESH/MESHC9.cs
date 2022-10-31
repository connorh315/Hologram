using System.IO;

namespace Hologram.FileTypes.GSC.GSCWriter.MESH
{
    public class MESHC9 : MESHAF
    {
        protected override int Version => 0xC9;
        //protected override bool BigEndian => false;

        protected override bool Setup()
        {
            parts = new PartData[partCount];
            referenceId = 4;

            for (int partId = 0; partId < partCount; partId++)
            {
                uint unknown = file.ReadUint(true); // 1
                if (!file.CheckString("SMNR", Locale.GSCStrings.ExpectedRNMS)) return false;
                if (!file.CheckInt(0xC9, Locale.GSCStrings.ExpectedRNMSVersion)) return false;

                PartData part = ReadVertexData(partId);

                file.Seek(4, SeekOrigin.Current);

                ReadIndicesAndPartData(part);

                file.Seek(74, SeekOrigin.Current);

                referenceId += 2;
            }

            return true;
        }

        protected override bool Write()
        {
            referenceId = 4;

            for (int partId = 0; partId < partCount; partId++)
            {
                PartData part = parts[partId];

                uint unknown = file.ReadUint(true); // 1
                if (!file.CheckString("SMNR", Locale.GSCStrings.ExpectedRNMS)) return false;
                if (!file.CheckInt(0xC9, Locale.GSCStrings.ExpectedRNMSVersion)) return false;

                uint listCount = file.ReadUint(true);
                for (int listId = 0; listId < listCount; listId++)
                {
                    WriteVertexData();
                }

                file.Seek(4, SeekOrigin.Current);

                WriteIndicesAndPartData(part);

                file.Seek(4, SeekOrigin.Current);
            }

            UpdateSize();

            return true;
        }
    }
}
