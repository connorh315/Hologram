using Hologram.Objects;
using ModLib;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using Hologram.FileTypes.GSC.GSCWriter.MESH.DXTV;

namespace Hologram.FileTypes.GSC.GSCWriter.MESH;

public class MESHAF : MESH
{
    protected override int Version => 0xAF;
    //protected override bool BigEndian => true;

    protected int referenceId = 5;

    protected override bool Setup()
    {
        parts = new PartData[partCount];

        for (int partId = 0; partId < partCount; partId++)
        {
            uint vtxdVersion = file.ReadUint(true); // 1

            PartData part = ReadVertexData(partId);

            file.Seek(4, SeekOrigin.Current); // 0

            ReadIndicesAndPartData(part);

            file.Seek(70, SeekOrigin.Current);

            referenceId += 2;
        }

        return true;
    }

    protected PartData ReadVertexData(int partId)
    {
        uint listCount = file.ReadUint(true);

        PartData part = new PartData(listCount, meshes[partId]);
        parts[partId] = part;

        for (int listId = 0; listId < listCount; listId++)
        {
            Reference vtxReference;
            uint vtxMarker = file.ReadUint(true);
            if ((vtxMarker & 0xc0000000) != 0)
            {
                vtxMarker &= 0xffff;
                part.VertexReferences[listId] = vtxMarker;
                vtxReference = references[(int)vtxMarker];
                vtxReference.Parts.Add(part);
                file.Seek(8, SeekOrigin.Current);
                continue;
            }

            part.VertexReferences[listId] = (uint)referenceId;
            vtxReference = new Reference();
            references[referenceId] = vtxReference;
            vtxReference.Parts.Add(part);
            referenceId++;

            uint unknown = file.ReadUint(true); // Usually 0x502 or 0x202 or 0x102

            uint vtxCount = file.ReadUint(true);
            file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
            file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion); // VertexDefinition lists have been invariable for some time now.

            var vertexList = new DXTVA9();
            vertexList.BigEndian = BigEndian;
            vertexList.Read(file, vtxCount);
            vtxReference.vertexList = vertexList;
        }

        return part;
    }

    protected void ReadIndicesAndPartData(PartData part)
    {
        Reference indicesReference;
        uint indicesMarker = file.ReadUint(true);
        if ((indicesMarker & 0xc0000000) != 0)
        {
            indicesMarker &= 0xffff;
            file.Seek(4, SeekOrigin.Current); // 1

            part.IndicesReference = indicesMarker;
            indicesReference = references[(int)indicesMarker];
            indicesReference.Parts.Add(part);
        }
        else
        {
            part.IndicesReference = (uint)referenceId;
            indicesReference = new Reference();
            references[referenceId] = indicesReference;
            indicesReference.Parts.Add(part);
            referenceId++;

            file.Seek(4, SeekOrigin.Current); // 2
            uint indicesCount = file.ReadUint(true);
            file.Seek(4, SeekOrigin.Current); // 2

            file.Seek(indicesCount * 2, SeekOrigin.Current);
        }
    }

    protected override bool Write()
    {
        referenceId = 5;

        for (int partId = 0; partId < partCount; partId++)
        {
            PartData part = parts[partId];

            uint vtxdVersion = file.ReadUint(true);
            uint listCount = file.ReadUint(true);

            for (int listId = 0; listId < listCount; listId++)
            {
                WriteVertexData();
            }

            file.Seek(4, SeekOrigin.Current);

            WriteIndicesAndPartData(part);
        }

        UpdateSize();

        return true;
    }

    protected virtual void WriteVertexData()
    {
        uint vtxMarker = file.ReadUint(true);
        if ((vtxMarker & 0xc0000000) != 0)
        {
            file.Seek(8, SeekOrigin.Current); // two unknown ints
            return;
        }

        Reference vtxReference = references[referenceId];
        referenceId++;

        uint unknown = file.ReadUint(true); // Usually 0x502 or 0x202 or 0x102

        bool shouldClearVtx = true; // Should remove all existing data.

        int totalVertices = 0;
        for (int meshId = 0; meshId < vtxReference.Parts.Count; meshId++)
        {
            if (vtxReference.Parts[meshId].PartMesh == null)
            {
                shouldClearVtx = false;
                continue;
            }
            totalVertices += vtxReference.Parts[meshId].PartMesh.VertexCount;
        }

        uint origVtxCount = file.ReadUint(true);
        file.Seek(-4, SeekOrigin.Current);
        file.WriteInt(shouldClearVtx ? totalVertices : (int)(totalVertices + origVtxCount), true);

        file.CheckString("DXTV", Locale.GSCStrings.ExpectedVTXD);
        file.CheckInt(0xA9, Locale.GSCStrings.ExpectedVTXDVersion);

        vtxReference.vertexList.Write(shouldClearVtx, vtxReference.Parts);
    }

    protected virtual void WriteIndicesAndPartData(PartData currentPart)
    {
        Reference idcReference;
        uint indicesMarker = file.ReadUint(true);
        if ((indicesMarker & 0xc0000000) != 0)
        {
            file.Seek(4, SeekOrigin.Current);
            if (currentPart.IndicesCount == 0) // No changes were made to this part, so skip.
            {
                file.Seek(70, SeekOrigin.Current);
            }
            else
            {
                file.WriteUint(currentPart.IndicesOffset, true);
                file.WriteUint(currentPart.IndicesCount, true);
                file.WriteUint(currentPart.VerticesOffset, true);
                file.Seek(2, SeekOrigin.Current);
                file.WriteUint(currentPart.VerticesCount, true);
                file.Seek(52, SeekOrigin.Current);
            }

            referenceId += 2;
            return;
        }

        idcReference = references[referenceId];
        referenceId++;

        file.Seek(4, SeekOrigin.Current); // 2

        bool shouldClearIdc = true; // Should remove all existing data.

        int totalIndices = 0;
        for (int meshId = 0; meshId < idcReference.Parts.Count; meshId++)
        {
            if (idcReference.Parts[meshId].PartMesh == null)
            {
                shouldClearIdc = false;
                continue;
            }
            totalIndices += idcReference.Parts[meshId].PartMesh.IndicesCount;
        }
        uint origIdcCount = file.ReadUint(true);
        file.Seek(-4, SeekOrigin.Current);
        file.WriteInt(shouldClearIdc ? totalIndices : (int)(totalIndices + origIdcCount), true);

        file.Seek(4, SeekOrigin.Current); // 2

        long startIdc = file.Position;
        file.Seek(2 * origIdcCount, SeekOrigin.Current);
        ModFile idcRemainder = file.GetRemainder();

        if (shouldClearIdc)
        {
            file.Seek(startIdc, SeekOrigin.Begin);
        } 

        uint idcOffset = shouldClearIdc ? 0 : origIdcCount;
        for (int meshId = 0; meshId < idcReference.Parts.Count; meshId++)
        {
            MeshX mesh = idcReference.Parts[meshId].PartMesh;
            if (mesh == null) continue;
            idcReference.Parts[meshId].IndicesOffset = idcOffset;
            idcReference.Parts[meshId].IndicesCount = (uint)mesh.IndicesCount;
            for (int ind = 0; ind < mesh.IndicesCount; ind++)
            {
                file.WriteUshort(mesh.Indices[ind], BigEndian);
            }

            idcOffset += (uint)mesh.IndicesCount;
        }

        file.AddRemainder(idcRemainder);

        if (currentPart.IndicesCount != 0 && currentPart.VerticesCount != 0)
        {
            file.WriteUint(currentPart.IndicesOffset, true);
            file.WriteUint(currentPart.IndicesCount, true);
            file.WriteUint(currentPart.VerticesOffset, true);
            file.Seek(2, SeekOrigin.Current); // 00
            file.WriteUint(currentPart.VerticesCount, true);

            file.Seek(52, SeekOrigin.Current);
        }
        else
        {
            file.Seek(70, SeekOrigin.Current);
        }

        referenceId += 2;
    }

    protected virtual void UpdateSize()
    {
        file.Seek(0, SeekOrigin.Begin);
        uint mainOffset = file.ReadUint(true);
        file.Seek(mainOffset, SeekOrigin.Current);
        file.WriteUint((uint)(file.Length - file.Position - 4), true);
    }
}
