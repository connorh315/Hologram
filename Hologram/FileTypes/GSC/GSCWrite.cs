using Hologram.Objects;
using System.IO;
using Hologram.FileTypes.GSC.GSCWriter.NU20;
using Hologram.FileTypes.GSC.GSCWriter.MESH;
using ModLib;
using System;
using OpenTK.Mathematics;

namespace Hologram.FileTypes.GSC
{
    public partial class GSC
    {
        public static GSCWriteStatus Write(string gscLocation, MeshX[] overrides, GSC originalGSC)
        {
            using (ModFile file = ModFile.Open(gscLocation))
            {
                if (file.Status != ModFileStatus.Success) return GSCWriteStatus.IO;

                Logger.Log(new LogSeg("Beginning importing into: {0}", ConsoleColor.Blue, gscLocation));

                uint mainOffset = file.ReadUint(true);
                file.Seek(mainOffset, SeekOrigin.Current);

                uint mainSize = file.ReadUint(true);

                uint unknown = file.ReadUint(true); // Normal value: 1 (does this mean 1 mesh, or version 1, or is just a 1?)

                if (!file.CheckString("02UN", Locale.GSCStrings.ExpectedNU20)) return GSCWriteStatus.Failed;
                uint nu20Version = file.ReadUint(true);

                NU20 header = ChooseHeader(nu20Version);
                if (header == null) return GSCWriteStatus.NotSupported;

                if (!header.Read(file)) return GSCWriteStatus.Failed;

                if (!file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH)) return GSCWriteStatus.Failed;
                uint meshVersion = file.ReadUint(true);

                MESH mesh = ChooseMesh(meshVersion);
                if (mesh == null) return GSCWriteStatus.NotSupported;
                mesh.BigEndian = originalGSC.BigEndian;
                if (!mesh.Write(file, overrides)) return GSCWriteStatus.Failed;

                return GSCWriteStatus.Success;
            }
        }

        private static MESH ChooseMesh(uint meshVersion)
        {
            switch (meshVersion)
            {
                case 0xaf:
                    return new MESHAF();
                case 0xc9:
                    return new MESHC9();
                default:
                    return null;
            }
        }

        private static NU20 ChooseHeader(uint nu20Version)
        {
            switch (nu20Version)
            {
                case 0x4f:
                    return new NU204F();
                case 0x50:
                    return new NU2050();
                case 0x53:
                    return new NU2053();
                case 0x5c:
                    return new NU205C();
                case 0x57:
                    return new NU2057();
                case 0x58:
                    return new NU2058();
                default:
                    return null;
            }
        }
    }

    public enum GSCWriteStatus
    {
        Success,
        IO,
        NotSupported,
        Failed
    }
}
