using Hologram.Objects;
using ModLib;

using Hologram.FileTypes.GSC;
using Hologram.Settings;

namespace Hologram.FileTypes.GSCWrite;

public class GSCWriter
{
    public static void Write(string fileLocation, Entity[] entities)
    {
        using (ModFile file = ModFile.Create(fileLocation))
        {
            using (ModFile res = ModFile.Open(@"A:\Dimensions\resheader.frag"))
            {
                res.fileStream.CopyTo(file.fileStream);
            }

            file.WritePadding(4); // This will be the header size, we'll come back and fill it once we've built the header

            long startOfMain = file.Position;

            file.WriteInt(1, true); // NU20 count (only ever 1)
            file.WriteString("02UN");
            file.WriteInt(0x4f, true);

            file.WriteString("OFNI");
            file.WriteInt(3, true);

            file.WritePascalString(HoloSettings.Author, 1);
            file.WritePascalString($"CONVDATE[{DateTime.Now.ToString("F")}]", 1);
            file.WritePascalString("", 1);

            file.WriteString("LBTN");
            file.WriteInt(0x4f, true);

            file.WriteShort(0);
            file.WritePascalString("default_string", 1);

            file.WritePadding(4);

            file.WriteString("ROTV");
            file.WriteInt(0);
            file.WriteString("ROTV");
            file.WriteInt(0);

            file.WriteInt(1, true);
            file.WriteString("HSEM");
            file.WriteInt(0xaf, true);

            Dictionary<MeshX, int> meshDictionary = new();
            int meshCount = 0;
            foreach (Entity ent in entities)
            {
                if (!meshDictionary.ContainsKey(ent.Mesh))
                {
                    meshDictionary.Add(ent.Mesh, meshCount++);
                }
            }

            file.WriteInt(meshCount, true);

            foreach (KeyValuePair<MeshX, int> meshIndexPair in meshDictionary)
            {
                MeshX mesh = meshIndexPair.Key;

                file.WriteInt(1, true); // i guess it shows there's only 1 block?
                file.WriteInt(1, true); // only use 1 vertex list
                file.WriteInt(1, true); // not sure
                file.WriteInt(0x202, true); // either 102, 202 or 502

                file.WriteInt(mesh.VertexCount, true);
                file.WriteString("DXTV");
                file.WriteInt(0xa9, true);

                file.WriteInt(0x4, true); // definitions count
                file.WriteInt(0x00060001, true); // 0 6 0 1
                file.WriteInt(0x08080209, true); // 8 8 2 9
                file.WriteInt(0x0c050510, true); // c 5 5 10
                file.WritePadding(6);

                foreach (var vertex in mesh.Vertices)
                {
                    Format.vec4half(file, vertex.Position);

                    Format.vec4mini(file, vertex.Normal);

                    Format.color4char(file, vertex.Color);

                    Format.vec2half(file, vertex.UV);
                }

                file.WriteInt(0);
                file.WriteInt(0);

                file.WriteInt(1, true);
                file.WriteInt(2, true);

                file.WriteInt(mesh.IndicesCount, true);
                file.WriteInt(2, true);

                foreach (ushort indice in mesh.Indices)
                {
                    file.WriteUshort(indice, true);
                }

                file.WriteInt(0); // indices offset
                file.WriteInt(mesh.IndicesCount, true); // indices count
                file.WriteInt(0); // vertex offset
                file.WriteShort(0); // primitive type
                file.WriteInt(mesh.VertexCount, true); // vertex count

                file.WriteInt(0);
                file.WriteInt(0);
                file.WriteInt(0);
                file.WriteInt(0);

                file.WriteFloat(0.15f, true);
                file.WriteFloat(0.30f, true);
                file.WriteFloat(-0.11f, true);
                file.WriteFloat(1, true);
                file.WriteFloat(0.16f, true);
                file.WriteFloat(0.30f, true);
                file.WriteFloat(0.27f, true);
                file.WriteFloat(0, true);
                file.WriteFloat(0.01f, true);
            }

            file.WriteInt(0);

            file.WriteInt(1, true);
            file.WriteString("LTMU");
            file.WriteInt(0xe4, true);

            using (ModFile matFile = ModFile.Open(@"A:\Dimensions\materialfile.frag"))
            {
                matFile.fileStream.CopyTo(file.fileStream);
            }

            file.WriteString("ROTV");
            file.WriteInt(0);
            file.WriteByte(0);
            file.WriteInt(0x6517755e, true);
            file.WriteInt(0xc, true);
            file.WriteInt(1, true);
            file.WriteString("TDML");
            file.WriteInt(3, true);
            file.WriteString("ROTV");
            file.WriteInt(1, true);
            file.WriteInt(6, true);
            file.WriteInt(-1, true);
            file.WriteInt(0);
            file.WriteInt(0);
            file.WriteInt(0);
            file.WriteInt(1, true);
            file.WriteInt(2, true);
            file.WriteInt(0);
            file.WriteInt(0);
            file.WriteFloat(1, true);
            file.WriteFloat(1, true);
            file.WriteInt(1, true);
            file.WriteString("SUPC");
            file.WriteInt(4, true);
            file.WriteString("ROTV");
            file.WriteInt(0);

            file.WriteString("PSID");
            file.WriteInt(0x20, true);

            // Defunct command queue
            file.WriteString("ROTV");
            file.WriteInt(11 + (2 * entities.Length), true); // command count

            WriteCommand(file, Command.Terminate, 0);
            WriteCommand(file, Command.MaterialClip, 4);
            WriteCommand(file, Command.Material, 0);
            WriteCommand(file, Command.DynamicGeo, 0);
            WriteCommand(file, Command.Terminate, 0);
            WriteCommand(file, Command.MaterialClip, 8);
            WriteCommand(file, Command.Material, 1);
            WriteCommand(file, Command.DynamicGeo, 0);
            WriteCommand(file, Command.Terminate, 0);
            WriteCommand(file, Command.Dummy, 0);

            for (int i = 0; i < entities.Length; i++)
            {
                WriteCommand(file, Command.Matrix, i);
                WriteCommand(file, Command.Mesh, meshDictionary[entities[i].Mesh]);
            }

            WriteCommand(file, Command.End, 0);

            // Clip items
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                file.WriteShort(1, true);
                file.WriteInt(11 + (i * 2), true);
                file.WriteInt(1, true);
            }

            // Instances
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity ent = entities[i];
                var pos = ent.Position;
                file.WritePascalString(ent.Name, 1);

                // Matrix
                file.WriteFloat(1, true);
                file.WritePadding(16);
                file.WriteFloat(1, true);
                file.WritePadding(16);
                file.WriteFloat(1, true);
                file.WriteFloat(0, true);
                file.WriteFloat(pos.X, true);
                file.WriteFloat(pos.Y, true);
                file.WriteFloat(pos.Z, true);
                file.WriteFloat(1, true);

                // Min
                file.WriteFloat(-0.3f, true);
                file.WriteFloat(-0.3f, true);
                file.WriteFloat(-0.3f, true);
                file.WriteFloat(1, true);

                // Max
                file.WriteFloat(0.3f, true);
                file.WriteFloat(0.3f, true);
                file.WriteFloat(0.3f, true);
                file.WriteFloat(1, true);

                // Maybe sphere
                file.WriteFloat(0.15f, true);
                file.WriteFloat(0.15f, true);
                file.WriteFloat(0.15f, true);
                file.WriteFloat(0.4f, true);

                file.WriteInt(i, true);
                file.WriteUint(0x80000412, true);
                file.WriteInt(0);
                file.WriteInt(i, true);
                file.WriteInt(-1, true);
                file.WriteInt(0);

            }

            // Nodes
            file.WriteString("ROTV");
            file.WriteInt(0);

            // Bounds center and dist sqrd
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                var pos = entities[i].Position;
                file.WriteFloat(pos.X, true);
                file.WriteFloat(pos.Y, true);
                file.WriteFloat(pos.Z, true);
                file.WriteFloat(10000, true);
            }

            // Bounds extents and radius
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                file.WriteFloat(0.13f, true);
                file.WriteFloat(0.2f, true);
                file.WriteFloat(0.145f, true);
                file.WriteFloat(0.22f, true);
            }

            // Instances
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                file.WriteInt(0);
                file.WriteInt(0x40300000 + i, true);
                file.WriteFloat(25, true);
                file.WriteFloat(22.5f, true);
                file.WriteFloat(25, true);
                file.WriteFloat(25, true);
                file.WriteFloat(25, true);
                file.WriteFloat(0.2f, true);
                file.WriteFloat(1f, true);
                file.WriteInt(0);
                file.WriteInt(0);
                file.WriteInt(0);
                file.WriteUint(0xffff00ff, true);
                file.WriteUint(0xffffff00, true);
                file.WriteUint(0xff00ffff, true);
                file.WriteUint(0xff0000ff, true);
            }

            // Instances LOD Fixups
            file.WriteString("ROTV");
            file.WriteInt(0);

            // Anim mtls
            file.WriteString("ROTV");
            file.WriteInt(0);

            // Matrices
            file.WriteString("ROTV");
            file.WriteInt(entities.Length, true);
            for (int i = 0; i < entities.Length; i++)
            {
                var pos = entities[i].Position;
                file.WriteFloat(1, true);
                file.WriteFloat(0, true);
                file.WriteFloat(0, true);
                file.WriteFloat(0, true);
                file.WriteFloat(0, true);
                file.WriteFloat(1, true);
                file.WriteFloat(0, true);
                file.WriteFloat(0, true);
                file.WriteFloat(pos.X, true);
                file.WriteFloat(pos.Y, true);
                file.WriteFloat(pos.Z, true);
                file.WriteFloat(1, true);
            }

            file.WriteString("ROTV");
            file.WriteInt(0);

            file.WriteString("BNAT");
            file.WriteInt(5, true);


            file.WriteInt(0);
            file.WriteInt(0x41f00000, true);


            file.WriteInt(1, true);
            file.WriteString("3ALA");
            file.WriteInt(3, true);
            file.WriteInt(0);
            file.WriteInt(0);
            file.WriteInt(0);
            file.WriteInt(0);

            file.WriteString("BCSB");
            file.WriteInt(3, true);

            file.WriteInt(0);

            file.WriteString("BCCO");
            file.WriteInt(3, true);

            file.WriteString("ROTV");
            file.WriteInt(0);

            file.WriteString("5LVI");
            file.WriteInt(1, true);

            file.WriteInt(0);
            
            file.WriteString("ROTV");
            file.WriteInt(0);
            file.WriteInt(0);

            file.WriteString("ATEM");
            file.WriteInt(0x4f, true);

            string[] textureStrings = new string[]
            {
                @"LEGO_Zeus\LEGO_Zeus_Images_Nut\engine\forcedDummyLightmapTex.nut",
                @"lego_zeus\lego_zeus_images_nut\engine\forcedsmoothdummylightmaptex.nut",
                @"lego_zeus\lego_zeus_images_nut\engine\forcedaodummylightmaptex.nut",
                @"lego_models_extended/images/undersiderenders/commonconedrill_occlusion.nut",
                @"lego_models_extended/images/undersiderenders/commonconedrill_normal.nut",
                @"LEGO_Zeus/LEGO_Zeus_Data/LEGOTpage/LEGO_Zeus_Tpage.nut",
                @"LEGO_Zeus/LEGO_Zeus_Data/LEGOTpage/LEGO_Zeus_Tpage_Nrm.nut"
            };

            file.WriteString("ROTV");
            file.WriteInt(textureStrings.Length, true);
            for (int i = 0; i < textureStrings.Length; i++)
            {
                file.WritePascalString(textureStrings[i], 1);
            }

            file.WritePadding(9);

            file.WriteString("ROTV");
            file.WriteInt(0);
            file.WriteInt(0);

            long endOfMain = file.Position;

            file.WriteString("NUS: 17:08:33 18-06-2015 Converted: 17:08:41 18-06-2015");

            file.WritePadding(0x3d);

            file.WriteUint(0x11fb8255, true);
            file.WriteUint(0x19fb8255, true);
            file.WriteString("META");

            file.Seek(startOfMain - 4, SeekOrigin.Begin);
            file.WriteInt((int)(endOfMain - startOfMain), true);
        }
    }

    private static void WriteCommand(ModFile file, Command command, int index)
    {
        file.WriteUshort((ushort)command, true);
        file.WriteShort(0);
        file.WriteUshort((ushort)index, true);
    }

    internal enum Command
    {
        Material = 0x8003,
        GeoCall = 0x8200,
        Matrix = 0x8300,
        Terminate = 0x8404,
        MaterialClip = 0x8501,
        Dummy = 0x8700,
        DynamicGeo = 0x8b01,
        End = 0x8e00,
        FaceOn = 0x8f00,
        LightMap = 0xb000,
        Mesh = 0xb300,
        Unknown2 = 0xb500,
        Other = 0x0000
    }
}
