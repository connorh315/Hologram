using Hologram.Objects;
using Hologram.FileTypes.GSC;
using ModLib;

namespace Hologram.FileTypes
{
    public static class FileLoader
    {
        public static Entity[]? LoadModelFile(string fileLocation)
        {
            string extension = Path.GetExtension(fileLocation);
            switch (extension.ToLower())
            {
                case ".obj":
                    OBJ obj = OBJ.Parse(fileLocation);
                    return new Entity[] { obj.Entity };
                case ".gsc":
                    GSC.GSC gsc = GSC.GSC.Parse(fileLocation);
                    return gsc.entities;
                case ".hob":
                    Entity hob;
                    using (ModFile file = ModFile.Open(fileLocation))
                    {
                        hob = HOB.Parse(file);
                    }
                    return new Entity[] { hob };
                default:
                    Logger.Error($"Not sure what to do with file extension: {extension}");
                    return null;
            }
        }
    }
}
