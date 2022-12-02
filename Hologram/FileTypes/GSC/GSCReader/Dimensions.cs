using ModLib;
using System;
using System.IO;
using OpenTK.Mathematics;
using System.Text;
using System.Collections.Generic;
using Hologram.Objects;
using Hologram.Rendering;
using WiiUTexturesTool;

// Needs some huge abstractions, but for now this will do.

namespace Hologram.FileTypes.GSC.GSCReader
{
    public static class Dimensions
    {
        public static void Read(ModFile file, GSC gsc)
        {
            file.CheckString("OFNI", Locale.GSCStrings.ExpectedINFO);

            Logger.Log("Metadata:");
            uint stringsCount = file.ReadUint(true);
            for (int i = 0; i < stringsCount; i++)
            {
                Logger.Log(new LogSeg(file.ReadPascalString(), ConsoleColor.Gray));
            }

            file.CheckString("LBTN", Locale.GSCStrings.ExpectedNTBL);
            int ntblVersion = file.ReadInt(true);
            if (ntblVersion != 0x4f && ntblVersion != 0x50 && ntblVersion != 0x53)
            {
                Logger.Error(Locale.GSCStrings.ExpectedNTBLVersion);
            }

            file.Seek(file.ReadInt(true), SeekOrigin.Current); // Big blob of strings

            file.Seek(24, SeekOrigin.Current); // ROTV

            file.CheckString("HSEM", Locale.GSCStrings.ExpectedMESH);
            file.CheckInt(0xAF, Locale.GSCStrings.ExpectedMESHVersion);

            uint partCount = file.ReadUint(true);
            gsc.parts = new Part[partCount];
            for (int partId = 0; partId < partCount; partId++)
            {
                uint one = file.ReadUint(true);

                uint vertexListCount = file.ReadUint(true);

                Part part = new Part(vertexListCount);
                gsc.parts[partId] = part;
                for (int vertexListId = 0; vertexListId < vertexListCount; vertexListId++)
                {
                    part.VertexListReferences[vertexListId] = gsc.GetVertexListReference();
                }
                file.Seek(4, SeekOrigin.Current);
                part.IndexListReference = gsc.GetIndexListReference();
                //Console.WriteLine("part offset: " + file.Position);
                part.OffsetIndices = file.ReadInt(true);
                part.IndicesCount = file.ReadInt(true);
                part.OffsetVertices = file.ReadInt(true);

                //Console.WriteLine(part.OffsetIndices);
                //Console.WriteLine(part.IndicesCount);
                //Console.WriteLine(part.OffsetVertices);

                ushort primiviteType = file.ReadUshort(true);
                if (primiviteType != 0) throw new Exception("Unknown primitiveType!");

                part.VerticesCount = file.ReadInt(true);

                //Console.WriteLine(part.VerticesCount);

                file.Seek(4, SeekOrigin.Current);

                int num = file.ReadInt(true);
                if (num > 0)
                {
                    Logger.Warn("Using assumption XXXXXX");
                    file.Seek(num, SeekOrigin.Current);
                }

                int num2 = file.ReadInt(true);
                if (num2 > 0)
                {
                    int num3 = gsc.ReadRelativePositionList();
                    gsc.referenceCounter += num3;
                }

                //Logger.Log(new LogSeg(file.Position.ToString(), ConsoleColor.DarkBlue));
                file.Seek(40, SeekOrigin.Current);

                gsc.referenceCounter += 2;
            }

            file.Seek(8, SeekOrigin.Current);
            Material[] materials = ReadMaterialData(file);

            file.Seek(0x15, SeekOrigin.Current);
            file.CheckString("TDML", "Expected LMDT");
            file.CheckInt(0x3, "Expected LMDT version 3");
            file.CheckString("ROTV", "Expected ROTV");
            ReadLightmapData(file);

            file.Seek(4, SeekOrigin.Current);
            file.CheckString("SUPC", "Expected CPUS");
            file.CheckInt(0x4, "Expected CPUS version 4");
            // No implementation for CPU Skinned yet
            file.CheckString("ROTV", "Expected ROTV");
            file.CheckInt(0, "ROTV is not zero!!"); // This is where the implementation should handle

            file.CheckString("PSID", "Expected DISP");
            file.CheckInt(0x20, "Expected DISP version 20");

            file.CheckString("ROTV", "Expected ROTV");
            DisplayCommand[] commands = ReadDefunctItems(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadClipItems(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadSpecialObject(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadSpecialGroupNodes(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadBoundsCenterAndDist(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadBoundsExtentsAndRadius(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadInstances(file);

            if (!file.CheckString("ROTV", "Expected ROTV"))
            {
                file.Seek(file.Find("ROTV") + 4, SeekOrigin.Current);
            }
            ReadInstancesLODFixups(file);

            file.CheckString("ROTV", "Expected ROTV");
            ReadAnimMtls(file);

            file.CheckString("ROTV", "Expected ROTV");
            Matrix4x3[] positions = ReadMatrices(file);

            DDSFile[] ddsFiles = WiiUTextures.RetrieveTextures(@"A:\Dimensions\EXTRACT\LEVELS\TARDIS\TARDIS11\TARDIS11_NXG.WIIU_TEXTURES");
            Texture[] textures = new Texture[ddsFiles.Length];
            for (int i = 0; i < ddsFiles.Length; i++)
            {
                if (ddsFiles[i].File == null) continue;
                textures[i] = DDS.DDS.Load(ddsFiles[i].File, false);
            }

            //textures[16].Use();

            List<Entity> entities = new List<Entity>();
            StringBuilder builder = new StringBuilder();
            int matrixId = -1;
            int materialId = -1;
            int meshCount = 0;
            int vertOffset = 1;
            for (int commandId = 0; commandId < commands.Length; commandId++)
            {
                DisplayCommand command = commands[commandId];
                switch (command.Command)
                {
                    case Command.Material:
                        materialId = command.Index;
                        break;
                    case Command.MaterialClip:
                        //Logger.Log(new LogSeg(command.Index.ToString(), ConsoleColor.Red));
                        break;
                    case Command.Matrix:
                        matrixId = command.Index;
                        break;
                    case Command.Mesh:
                        meshCount++;
                        Matrix4x3 local = positions[matrixId];
                        MeshX mesh = gsc.ConvertPart(gsc.parts[command.Index]);
                        Entity ent = new Entity(new Matrix4(new Vector4(local.Row0, 0), new Vector4(local.Row1, 0), new Vector4(local.Row2, 0), new Vector4(local.Row3, 1)));
                        ent.Mesh = mesh;
                        if (materials[materialId].Texture != 255)
                        {
                            ent.Texture = textures[materials[materialId].Texture];
                        }
                        else
                        {
                            ent.Texture = textures[0];
                        }
                        mesh.Setup();
                        entities.Add(ent);
                        //foreach (var vertex in mesh.Vertices)
                        //{
                        //    Vector4 vec4 = new Vector4(vertex.Position, 1);
                        //    Vector3 global = new Vector3(Vector4.Dot(local.Column0, vec4), Vector4.Dot(local.Column1, vec4), Vector4.Dot(local.Column2, vec4));
                        //    builder.AppendLine($"v {global.X} {global.Y} {global.Z}");
                        //}
                        //for (int indiceId = 0; indiceId < mesh.IndicesCount; indiceId += 3)
                        //{
                        //    builder.AppendLine($"f {mesh.Indices[indiceId] + vertOffset} {mesh.Indices[indiceId + 1] + vertOffset} {mesh.Indices[indiceId + 2] + vertOffset}");
                        //}
                        vertOffset += mesh.VertexCount;
                        break;

                }
            }
            gsc.entities = entities.ToArray();
        }

        private static Material[] ReadMaterialData(ModFile file)
        {
            file.CheckString("LTMU", string.Empty);
            uint version = file.ReadUint(true);
            uint count = file.ReadUint(true);

            Material[] materials = new Material[count];
            Material firstMat = new Material();
            file.Seek(0x1ad, SeekOrigin.Current);
            firstMat.Texture = file.ReadByte();
            file.Seek(0x25C, SeekOrigin.Current);
            materials[0] = firstMat;
            file.ReadPascalString();
            //Logger.Log(new LogSeg(file.ReadPascalString(), ConsoleColor.Gray));
            file.Seek(0x49C, SeekOrigin.Current);

            for (int id = 0; id < count - 1; id++) // We already handled 1 prior to the loop, so we remove 1
            {
                Material mat = new Material();
                materials[1 + id] = mat;

                file.CheckString("DXTV", "Expected DXTV");
                file.CheckInt(0xA9, "Expected DXTV version A9");
                uint defCount = file.ReadUint(true);
                file.Seek(defCount * 3, SeekOrigin.Current);
                if (version == 0xe5)
                {
                    file.Seek(0x1fd, SeekOrigin.Current);
                }
                else if (version == 0xe4)
                {
                    file.Seek(0x1fc, SeekOrigin.Current);
                }
                mat.Texture = file.ReadByte();
                file.Seek(0x25c, SeekOrigin.Current);
                Console.WriteLine(file.ReadPascalString());
                //Logger.Log(new LogSeg(file.ReadPascalString(), ConsoleColor.Gray));
                file.Seek(0x49C, SeekOrigin.Current);
            }

            file.CheckString("DXTV", "Expected DXTV");
            file.CheckInt(0xA9, "Expected DXTV version A9");
            uint finalDefCount = file.ReadUint(true);
            file.Seek(finalDefCount * 3, SeekOrigin.Current);
            file.Seek(0x1a, SeekOrigin.Current);
            if (file.ReadByte() == 0x8) // idk
            {
                file.Seek(0x35, SeekOrigin.Current);
            }
            else
            {
                file.Seek(0x34, SeekOrigin.Current);
            }

            return materials;
            //Console.WriteLine(file.Position);
        }

        private static void ReadLightmapData(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                uint type = file.ReadUint(true);
                int meshInstanceId = file.ReadInt(true);
                int directionalTIDs0 = file.ReadInt(true);
                int directionalTIDs1 = file.ReadInt(true);
                int directionalTIDs2 = file.ReadInt(true);
                int smoothTID = file.ReadInt(true);
                int aoTID = file.ReadInt(true);

                float texCoordOffset0 = file.ReadFloat(true);
                float texCoordOffset1 = file.ReadFloat(true);
                float texCoordScale0 = file.ReadFloat(true);
                float texCoordScale1 = file.ReadFloat(true);
            }
        }

        private static DisplayCommand[] ReadDefunctItems(ModFile file)
        {
            uint count = file.ReadUint(true);
            DisplayCommand[] commands = new DisplayCommand[count];
            for (int id = 0; id < count; id++)
            {
                byte type = file.ReadByte();
                file.Seek(3, SeekOrigin.Current);
                ushort index = file.ReadUshort(true);
                commands[id] = new DisplayCommand
                {
                    Command = (Command)type,
                    Index = index
                };
            }

            return commands;
        }

        private static void ReadClipItems(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                ushort itemsCount = file.ReadUshort(true);
                for (int itemId = 0; itemId < itemsCount; itemId++)
                {
                    uint geomIndex = file.ReadUint(true);
                    uint matIndex = file.ReadUint(true);
                }
            }
        }

        private static void ReadSpecialObject(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                string name = file.ReadPascalString();
                Matrix4 matrix = new Matrix4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                Vector4 min = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                Vector4 max = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                file.Seek(0x1c, SeekOrigin.Current); // Supposedly "m_Sphere"
                file.Seek(12, SeekOrigin.Current); // Undeterminable, too many variables: "m_clipObjectIdx", "m_Flags", "m_instanceIdx", "m_AnimIdx", "m_WindSpeed", "m_WindScale"
            }
        }
        
        private static void ReadSpecialGroupNodes(ModFile file)
        {
            uint count = file.ReadUint(true);
            if (count > 0)
            {
                throw new Exception("Count > 0 on a section that was undeterminable!");
            }
        }

        private static void ReadBoundsCenterAndDist(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                Vector4 vec = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
            }
        }

        private static void ReadBoundsExtentsAndRadius(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                Vector4 vec = new Vector4(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
            }
        }

        private static void ReadInstances(ModFile file)
        {
            //Logger.Log(new LogSeg(file.Position.ToString(), ConsoleColor.DarkYellow));
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                file.Seek(0x20, SeekOrigin.Current);
                byte val = file.ReadByte();
                if (val == 1)
                {
                    file.Seek(0x43, SeekOrigin.Current);
                }
                else
                {
                    file.Seek(0x1F, SeekOrigin.Current);
                }
            }
            //int flags = file.ReadInt(true); // First section here is undeterminable, makes zero sense how variables are mapped.
            //Vector3 size = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
            //file.Seek(32, SeekOrigin.Current); // LODs I think??
            //int vertexControlledTint0 = file.ReadInt(true);
            //int vertexControlledTint1 = file.ReadInt(true);
            //int vertexControlledTint2 = file.ReadInt(true);
            //int vertexControlledTint3 = file.ReadInt(true);
        }

        private static void ReadInstancesLODFixups(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                Vector3 fixup = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true)); // This is probably wrong, Ghidra suggests 12 bytes are used but I haven't seen a file that uses this block yet
            }
        }

        private static void ReadAnimMtls(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int id = 0; id < count; id++)
            {
                int mtlId = file.ReadInt(true);
            }
        }

        private static Matrix4x3[] ReadMatrices(ModFile file)
        {
            uint count = file.ReadUint(true);
            Matrix4x3[] matrices = new Matrix4x3[count];
            for (int id = 0; id < count; id++)
            { // Careful, this seems wrong bc it's only 4x3 mtx so I just stuck zeroes on the end which might be wrong
                Matrix4x3 matrix = new Matrix4x3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                matrices[id] = matrix;
            }
            return matrices;
        }
    }

    public enum Command
    {
        Material = 0x80,
        GeoCall = 0x82,
        Matrix = 0x83,
        Terminate = 0x84,
        MaterialClip = 0x85,
        Dummy = 0x87,
        DynamicGeo = 0x8b,
        End = 0x8e,
        FaceOn = 0x8f,
        LightMap = 0xb0,
        Mesh = 0xb3,
        Unknown2 = 0xb5,
        Other = 0x0
    }

    public class DisplayCommand
    {
        public Command Command;
        public ushort Index;
    }

    public class Material
    {
        public int Texture;
    }
}
