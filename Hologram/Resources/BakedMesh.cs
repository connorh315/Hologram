using Hologram.FileTypes;
using Hologram.Objects;
using Hologram.Objects.Entities;
using ModLib;

namespace Hologram.Resources
{
    public class BakedMesh
    {
        protected virtual string Name { get; }

        public virtual byte[] ModelData { get; }

        private static Dictionary<string, MeshX> loadedMeshes = new();

        public static MeshX GetMesh(BakedMesh mesh)
        {
            if (loadedMeshes.ContainsKey(mesh.Name)) return loadedMeshes[mesh.Name];

            using (ModFile file = new ModFile(mesh.ModelData))
            {
                MeshX result = HOB.ParseMesh(file);
                loadedMeshes[mesh.Name] = result;
                result.Setup();
                return result;
            }
        }
    }
}
