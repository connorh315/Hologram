using ModLib;

namespace Hologram.FileTypes.GSC.GSCWriter.NU20
{
    public abstract class NU20
    {
        protected abstract int Version { get; }

        protected ModFile file;

        public virtual bool Read(ModFile file)
        {
            this.file = file;

            if (!ReadINFO()) return false;
            if (!ReadNTBL()) return false;

            return true;
        }

        /// <summary>
        /// Reads the INFO block, which usually contains metadata such as the person who built the GSC and the date it was generated.
        /// </summary>
        /// <returns>If successful</returns>
        protected abstract bool ReadINFO();

        /// <summary>
        /// Reads the NTBL block, which contains the name of the GSC (usually the LEGO part number, or if it's a custom piece, it will be named descriptively)
        /// </summary>
        /// <returns>If successful</returns>
        protected abstract bool ReadNTBL();
    }
}
