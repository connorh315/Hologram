using ModLib;
using Hologram.Objects;

/// Hi! Note that all of this code is specific to MESH Version 0xAF. If you are looking for code for other versions, view this github repo:
/// https://github.com/AlubJ/TTGames-Extraction-Tools

namespace Hologram.FileTypes.GSC
{
    public partial class GSCFile
    {
        public static Mesh Read(string fileLocation)
        {
            using (ModFile file = ModFile.Open(fileLocation))
            {
                file.Seek(file.Find("HSEM") + 4, SeekOrigin.Begin);
                uint version = file.ReadUint(true);
                if (version != 0xAF) { Logger.Error("File not supported! GSC Version != 0xAF. You have attempted to use this tool on a GSC that did not originate from Dimensions!"); return null; }

                RawMesh thisMesh = new RawMesh(file);
                thisMesh.Read();

                Console.WriteLine("Finished reading at: " + file.Position);

                return thisMesh.ConvertToHologramMesh();
            }

            //return new GSCFile();
        }
    }
}
