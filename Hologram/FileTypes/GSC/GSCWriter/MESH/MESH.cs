using Hologram.Objects;
using ModLib;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using Half = System.Half;

/// What's the difference between this and GSCReader's MESH implementation?
/// It's that the GSCWriter's implementation "marks" significant points in the file to insert a mesh, whereas GSCReader will return a complete mesh.

namespace Hologram.FileTypes.GSC.GSCWriter.MESH
{
    public abstract class MESH
    {
        protected abstract int Version { get; }
        //protected abstract bool BigEndian { get; }
        public bool BigEndian;

        protected long partOffset;
        protected int partCount;
        protected Dictionary<int, Reference> references = new();

        protected ModFile file;
        protected MeshX[] meshes;
        protected PartData[] parts;


        public virtual bool Write(ModFile file, MeshX[] meshes)
        {
            this.file = file;
            this.meshes = meshes;

            partCount = file.ReadInt(true);
            partOffset = file.Position;
            if (!Setup()) return false;

            file.Seek(partOffset, SeekOrigin.Begin);
            if (!Write()) return false;

            return true;
        }

        /// <summary>
        /// The setup function runs through the file and finds out which parts reference which lists, and builds a dictionary to ensure that meshes are inserted into the correct lists in the Write function.
        /// </summary>
        /// <returns>If successful</returns>
        protected abstract bool Setup();

        /// <summary>
        /// Uses the reference dictionary built by the Setup function to insert the mesh data into the correct lists.
        /// </summary>
        /// <returns>If successful</returns>
        protected abstract bool Write();
    }
}
