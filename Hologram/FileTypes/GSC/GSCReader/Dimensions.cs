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
            if (ntblVersion != 0x4e && ntblVersion != 0x4f && ntblVersion != 0x50 && ntblVersion != 0x53)
            {
                Logger.Error(Locale.GSCStrings.ExpectedNTBLVersion);
            }

            file.Seek(file.ReadInt(true), SeekOrigin.Current); // Big blob of strings

            //file.Seek(24, SeekOrigin.Current); // ROTV
            file.Seek(4, SeekOrigin.Current); // padding

            // Still not sure what this section is for
            file.CheckString("ROTV", "Expected ROTV");
            uint count1 = file.ReadUint(true);
            for (int i = 0; i < count1; i++)
            {
                string title = file.ReadPascalString();

                file.CheckString("ROTV", "Expected ROTV");
                uint count2 = file.ReadUint(true);
                file.Seek(12 * count2, SeekOrigin.Current);
                if (ntblVersion == 0x53 || ntblVersion == 0x50)
                {
                    file.ReadByte(); // just a zero
                }
            }

            file.CheckString("ROTV", "Expected ROTV");
            file.CheckInt(0, "Haven't encountered ROTV block > 0 yet!"); // Splines?

            file.CheckInt(1, "MESH block count > 1!");

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

                ushort primitiveType = file.ReadUshort(true);
                if (primitiveType != 0) throw new Exception("Unknown primitiveType!");

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


            Texture[] textures = LoadTextures(Path.ChangeExtension(file.Location, "WIIU_TEXTURES"));

            Material[] materials = ReadMaterialData(file, textures);

            file.CheckString("ROTV", "Expected ROTV0");
            uint embeddedTexCount = file.ReadUint(true); // 4doctorwhoa1_nxg
            for (int i = 0; i < embeddedTexCount; i++)
            {
                uint number = file.ReadUint(true); // not sure
                string location = file.ReadPascalString();
                string name = file.ReadPascalString();
            }

            file.Seek(13, SeekOrigin.Current);

            file.CheckString("TDML", "Expected LMDT");
            file.CheckInt(0x3, "Expected LMDT version 3");
            file.CheckString("ROTV", "Expected ROTV1");
            ReadLightmapData(file);

            file.Seek(4, SeekOrigin.Current);
            file.CheckString("SUPC", "Expected CPUS");
            file.CheckInt(0x4, "Expected CPUS version 4");
            // No implementation for CPU Skinned yet
            file.CheckString("ROTV", "Expected ROTV2");
            file.CheckInt(0, "ROTV is not zero!!"); // This is where the implementation should handle

            file.CheckString("PSID", "Expected DISP");
            file.CheckInt(0x20, "Expected DISP version 20");

            file.CheckString("ROTV", "Expected ROTV3");
            DisplayCommand[] commands = ReadDefunctItems(file);

            file.CheckString("ROTV", "Expected ROTV4");
            ClipItem[] items = ReadClipItems(file, commands);

            file.CheckString("ROTV", "Expected ROTV5");
            ReadSpecialObject(file);

            file.CheckString("ROTV", "Expected ROTV6");
            ReadSpecialGroupNodes(file);

            file.CheckString("ROTV", "Expected ROTV7");
            CameraBounds[] bounds = ReadBoundsCenterAndDistSqrd(file);

            file.CheckString("ROTV", "Expected ROTV8");
            ReadBoundsExtentsAndRadius(file);

            file.CheckString("ROTV", "Expected ROTV9");
            ReadInstances(file);

            if (!file.CheckString("ROTV", "Expected ROTVInst"))
            {
                file.Seek(file.Find("ROTV") + 4, SeekOrigin.Current);
            }
            ReadInstancesLODFixups(file);

            file.CheckString("ROTV", "Expected ROTV10");
            ReadAnimMtls(file);

            file.CheckString("ROTV", "Expected ROTV11");
            Matrix4x3[] positions = ReadMatrices(file);

            List<Entity> entities = new List<Entity>();
            StringBuilder builder = new StringBuilder();
            int matrixId = -1;
            int materialId = -1;
            int meshCount = 0;
            int vertOffset = 1;
            int dynamicCount = 0;
            Console.WriteLine("START!");
            Dictionary<int, Entity> entitiesKeyed = new Dictionary<int, Entity>();
            for (int commandId = 0; commandId < commands.Length; commandId++)
            {
                DisplayCommand command = commands[commandId];
                Console.WriteLine(command.Command);
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
                    case Command.DynamicGeo:
                        dynamicCount++;
                        //Logger.Log(new LogSeg(command.Index.ToString(), ConsoleColor.Red));
                        break;
                    case Command.Mesh:
                        meshCount++;
                        if (meshCount == 349)
                        {
                            Console.WriteLine();
                        }
                        Matrix4x3 local = positions[matrixId];
                        MeshX mesh = gsc.ConvertPart(gsc.parts[command.Index]);
                        Entity ent = new Entity(new Matrix4(new Vector4(local.Row0, 0), new Vector4(local.Row1, 0), new Vector4(local.Row2, 0), new Vector4(local.Row3, 1)));
                        ent.Mesh = mesh;
                        ent.Material = materials[materialId];
                        //if (textures.Length != 0)
                        //{
                        //    if (materials[materialId].DiffuseTexture != 255)
                        //    {
                        //        ent.Texture = textures[materials[materialId].DiffuseTexture];
                        //    }
                        //    else
                        //    {
                        //        ent.Texture = Texture.WhiteTexture;
                        //        ent.Mesh.Color = materials[materialId].Color; // Debatable
                        //    }

                        //}
                        //else
                        //{
                        //    ent.Texture = Texture.MissingTexture;
                        //}
                        mesh.Setup();
                        entities.Add(ent);
                        entitiesKeyed[commandId] = ent;
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

            for (int id = 0; id < items.Length; id++)
            {
                foreach (ClipElement element in items[id].Elements)
                {
                    if (entitiesKeyed.ContainsKey(element.GeometryIndex))
                    {
                        if (id < bounds.Length)
                        {
                            entitiesKeyed[element.GeometryIndex].Bounds = bounds[id];
                        }
                        else
                        {
                            entitiesKeyed[element.GeometryIndex].Bounds = new CameraBounds()
                            {
                                Center = Vector3.Zero,
                                DistSqrd = 100000
                            };
                        }
                        entitiesKeyed[element.GeometryIndex].Material = materials[element.MaterialIndex];
                    }
                }
            }

            gsc.entities = entities.ToArray();
        }

        private static Texture[] LoadTextures(string texturesFile)
        {
            Texture[] textures = new Texture[0];
            if (File.Exists(texturesFile))
            {
                DDSFile[] ddsFiles = WiiUTextures.RetrieveTextures(texturesFile);
                textures = new Texture[ddsFiles.Length];

                string[] locationSplit = texturesFile.Split("LEVELS");

                string root = locationSplit[0].Substring(0, locationSplit[0].Length - 1);

                for (int i = 0; i < ddsFiles.Length; i++)
                {
                    if (ddsFiles[i].File == null)
                    {
                        string path = root + ddsFiles[i].Attributes.Path;
                        textures[i] = DDS.DDS.Load(path);
                        textures[i].File = ModFile.Open(path);
                    }
                    else
                    {
                        textures[i] = DDS.DDS.Load(ddsFiles[i].File, false);
                        textures[i].File = ddsFiles[i].File;
                    }

                    if (textures[i] == null)
                    {
                        textures[i] = Texture.ProblemTexture;
                    }
                    else
                    {
                        textures[i].Name = ddsFiles[i].Attributes.Name;
                    }
                }
            }

            return textures;
        }

        private static Texture GetValidTexture(Texture[] textures, int index)
        { // Ensures that a valid texture is ALWAYS returned
            if (index == 255) return Texture.WhiteTexture;
            if (textures.Length == 0) return Texture.MissingTexture;
            if (textures[index] == null) return Texture.ProblemTexture;
            return textures[index];
        }

        private static Material[] ReadMaterialData(ModFile file, Texture[] textures)
        {
            file.CheckString("LTMU", string.Empty);
            uint version = file.ReadUint(true);
            uint count = file.ReadUint(true);

            Material[] materials = new Material[count];

            for (int matId = 0; matId < count; matId++)
            {
                Material mat = new Material();
                materials[matId] = mat;

                if (version == 0xf2)
                {
                    file.Seek(0x1d4, SeekOrigin.Current); // unknown mat data
                }
                else if (version == 0xf0)
                {
                    file.Seek(0x1cf, SeekOrigin.Current);
                }
                else if (version == 0xde)
                {
                    file.Seek(0x1ac, SeekOrigin.Current);
                }
                else
                {
                    file.Seek(0x1ad, SeekOrigin.Current); // unknown mat data
                }

                byte diffuseTexture = file.ReadByte();
                mat.Diffuse = GetValidTexture(textures, diffuseTexture);

                if (version == 0xf2)
                {
                    file.Seek(0x294, SeekOrigin.Current);
                }
                else if (version == 0xf0)
                {
                    file.Seek(0x297, SeekOrigin.Current);
                }
                else if (version == 0xe2)
                {
                    file.Seek(0x25b, SeekOrigin.Current);
                }
                else if (version == 0xde)
                {
                    file.Seek(0x26b, SeekOrigin.Current);
                }
                else
                {
                    file.Seek(0x17, SeekOrigin.Current);
                    byte normalTexture = file.ReadByte();
                    mat.Normal = GetValidTexture(textures, normalTexture);
                    file.Seek(0x146, SeekOrigin.Current);
                    mat.Color.B = ((float)file.ReadByte()) / 255;
                    mat.Color.G = ((float)file.ReadByte()) / 255;
                    mat.Color.R = ((float)file.ReadByte()) / 255;
                    mat.Color.A = 1;
                    file.Seek(0x25c - 0x18 - 3 - 0x146, SeekOrigin.Current);
                }
                
                mat.ShaderName = file.ReadPascalString();
                file.Seek(0x49c, SeekOrigin.Current);
                file.CheckString("DXTV", "Expected DXTV");
                file.CheckInt(0xA9, "Expected DXTV version A9");
                uint defCount = file.ReadUint(true);
                file.Seek(defCount * 3, SeekOrigin.Current);

                if (version >= 0xe5)
                {
                    file.Seek(0x50, SeekOrigin.Current);
                }
                else if (version == 0xe4 || version == 0xe2 || version == 0xe3 || version == 0xde) // assumed for 0xe3
                {
                    file.Seek(0x4f, SeekOrigin.Current);
                }

            }

            return materials;



            Material firstMat = new Material();
            file.Seek(0x1ad, SeekOrigin.Current);
            //firstMat.DiffuseTexture = file.ReadByte();
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
                //mat.DiffuseTexture = file.ReadByte();
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
            if (file.ReadByte() != 0x0) // so far it has been when it's 8 and 1
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

        private static ClipItem[] ReadClipItems(ModFile file, DisplayCommand[] commands)
        {
            uint count = file.ReadUint(true);
            ClipItem[] items = new ClipItem[count];

            for (int id = 0; id < count; id++)
            {
                ushort elementsCount = file.ReadUshort(true);
                items[id] = new ClipItem(elementsCount);
                for (int elementId = 0; elementId < elementsCount; elementId++)
                {
                    items[id].Elements[elementId].GeometryIndex = file.ReadInt(true);
                    items[id].Elements[elementId].MaterialIndex = file.ReadInt(true);
                }
            }

            return items;
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
                file.Seek(0x18, SeekOrigin.Current); // Supposedly "m_Sphere"
                uint aCount = file.ReadUint(true); // not sure on what this counts
                for (int i = 0; i < aCount; i++)
                {
                    file.ReadUint(true);
                }
                uint clipObjectIndex = file.ReadUint(true);
                file.Seek(8, SeekOrigin.Current); // Undeterminable, too many variables: "m_clipObjectIdx", "m_Flags", "m_instanceIdx", "m_AnimIdx", "m_WindSpeed", "m_WindScale"
            }
        }
        
        private static void ReadSpecialGroupNodes(ModFile file)
        {
            uint count = file.ReadUint(true);
            for (int nodeId = 0; nodeId < count; nodeId++)
            {
                uint indiceCount = file.ReadUint(true);
                for (int id = 0; id < indiceCount; id++)
                {
                    ushort indice = file.ReadUshort(true);
                }
            }
        }

        private static CameraBounds[] ReadBoundsCenterAndDistSqrd(ModFile file)
        {
            // OBJ obj = OBJ.Parse(@"A:\icosphere.obj");
            // Mesh mesh = obj.PhysicsMesh;
            uint count = file.ReadUint(true);
            CameraBounds[] boundsList = new CameraBounds[count];
            StringBuilder visualiser = new StringBuilder();
            int totalVerts = 1;
            for (int id = 0; id < count; id++)
            {
                Vector3 center = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                float distSqrd = file.ReadFloat(true);
                boundsList[id] = new CameraBounds()
                {
                    Center = center,
                    DistSqrd = distSqrd,
                };
                // foreach (var vert in mesh.Vertices)
                // {
                //     float result = Math.Min(distSqrd * 0.001f, 2);
                //     Vector3 scaled = result * vert;
                //     scaled += center;
                //     visualiser.AppendLine($"v {scaled.X} {scaled.Y} {scaled.Z}");
                // }
                //
                // foreach (var face in mesh.Faces)
                // {
                //     visualiser.AppendLine($"f {face.vert1 + totalVerts} {face.vert2 + totalVerts} {face.vert3 + totalVerts}");
                // }
                //
                // totalVerts += mesh.VertexCount;
            }

            // File.WriteAllText(@"A:\spheres.obj", visualiser.ToString());

            return boundsList;
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
            { // Pretty sure it's just file.Seek(0x6), and then check if the ushort there is equal to 0xffff
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

    public class ClipItem
    {
        public ClipElement[] Elements;
        public ClipItem(uint count)
        {
            Elements = new ClipElement[count];
        }
    }

    public struct ClipElement
    {
        public int GeometryIndex;
        public int MaterialIndex;
    }
}
