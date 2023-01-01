using Hologram.FileTypes;
using Hologram.Objects;
using ModLib;

namespace Hologram.Resources
{
    public class BakedEntity
    {
        public virtual byte[] ModelData { get; set; }

        public Entity LoadEntity()
        {
            using (ModFile file = new ModFile(ModelData))
            {
                Entity ent = HOB.Parse(file);
                return ent;
            }
        }
    }
}
