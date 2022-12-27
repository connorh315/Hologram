using System.Text;
using Hologram.Extensions;
using Hologram.FileTypes.XML;
using Hologram.Objects;
using Hologram.Rendering;
using Hologram.Settings;

namespace Hologram.FileTypes.DAE;

public class DAE
{
    public static void Create(string fileLocation, Entity[] entities)
    {
        using (XML.XML file = XML.XML.Create(fileLocation, "COLLADA", new XMLAttribute[] {new ("xmlns", "http://www.collada.org/2005/11/COLLADASchema"), new ("version", "1.4.1"), new ("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")}))
        {
            // Header
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture);
            file.CreateChild("asset")
                .CreateChild("contributor")
                    .CreateSibling("author", null, HoloSettings.Author)
                    .CreateSibling("authoring_tool", null, "Hologram " + HoloSettings.Version)
                    .Close()
                .CreateSibling("created", null, currentDateTime)
                .CreateSibling("modified", null, currentDateTime)
                .CreateSibling("unit", new XMLAttribute[2] { new("name", "meter"), new("meter", "1") })
                .CreateSibling("up_axis", null, "Y_UP")
                .Close();

            // Effects (colours)
            file.CreateChild("library_effects");

            int matIndex = 1;
            Dictionary<Material, int> materials = new();
            Dictionary<string, Texture> textures = new();
            for (int i = 0; i < entities.Length; i++)
            {
                Entity ent = entities[i];
                if (materials.ContainsKey(ent.Material)) continue;
                materials.Add(ent.Material, matIndex);
                matIndex++;
                Texture diffuse = ent.Material.Diffuse;
                string diffuseName = GenerateCleanName(diffuse.Name != null ? diffuse.Name : "");
                
                file.CreateChild("effect", new XMLAttribute[] { new("id", "mat" + materials.Count + "-effect") })
                    .CreateChild("profile_COMMON");
                if (!diffuse.Internal)
                {
                    if (textures.ContainsKey(diffuseName))
                    {
                        if (textures[diffuseName] != diffuse)
                        {
                            while (true)
                            {
                                diffuseName += "1";
                                if (!textures.ContainsKey(diffuseName)) break;
                            }
                            textures.Add(diffuseName, diffuse);
                        }
                    }
                    else
                    {
                        textures.Add(diffuseName, diffuse);
                    }
                    file.CreateChild("newparam", new XMLAttribute[] { new("sid", diffuseName + "-surface") })
                        .CreateChild("surface", new XMLAttribute[] { new("type", "2D") })
                        .CreateSibling("init_from", null, diffuseName)
                        .Close(2)
                        .CreateChild("newparam", new XMLAttribute[] { new("sid", diffuseName + "-sampler") })
                        .CreateChild("sampler2D")
                        .CreateSibling("source", null, diffuseName + "-surface")
                        .Close(2);
                }

                file.CreateChild("technique", new XMLAttribute[] { new("sid", "common") })
                    .CreateChild("lambert")
                    .CreateChild("emission")
                    .CreateSibling("color", new XMLAttribute[] { new("sid", "emission") }, "0 0 0 1")
                    .Close()
                    .CreateChild("diffuse");
                if (!diffuse.Internal)
                {
                    file.CreateSibling("texture", new XMLAttribute[] { new("texture", diffuseName + "-sampler"), new ("texcoord", $"part{i + 1}-mesh-coords") });
                }
                else
                {
                    file.CreateSibling("color", new XMLAttribute[] { new("sid", "diffuse") },
                        ent.Material.Color.ToCleanString());
                }
                    
                file.Close()
                    .CreateChild("index_of_refraction")
                    .CreateSibling("float", new XMLAttribute[] { new("sid", "ior") }, "1.45")
                    .Close()
                    .Close(4);
            }

            file.Close();
            
            // Images
            file.CreateChild("library_images");

            foreach (KeyValuePair<string, Texture> keyedTexture in textures)
            {
                Console.WriteLine(keyedTexture.Key);
                file.CreateChild("image",
                        new XMLAttribute[] { new("id", keyedTexture.Key), new("name", keyedTexture.Key) })
                    .CreateSibling("init_from", null, keyedTexture.Key + ".dds")
                    .Close();
                keyedTexture.Value.File.Seek(0, SeekOrigin.Begin);
                //keyedTexture.Value.File.WriteToFile(Path.Join(Path.GetDirectoryName(fileLocation), keyedTexture.Key + ".dds"));
            }
            
            file.Close();
            
            // Materials
            file.CreateChild("library_materials");

            for (int i = 1; i <= materials.Count; i++)
            {
                file.CreateChild("material", new XMLAttribute[] { new("id", $"mat{i}-material"), new ("name", $"mat{i}") })
                    .CreateSibling("instance_effect", new XMLAttribute[] {new ("url", $"#mat{i}-effect")})
                    .Close();
            }

            file.Close();
            
            // Geometries
            file.CreateChild("library_geometries");
            
            for (int i = 0; i < entities.Length; i++)
            {
                Material mat = entities[i].Material;
                MeshX mesh = entities[i].Mesh;
                int id = i + 1;
                StringBuilder posList = new StringBuilder();
                StringBuilder colList = new StringBuilder();
                StringBuilder texList = new StringBuilder();
                foreach (var vert in mesh.Vertices)
                {
                    posList.Append($"{vert.Position.X} {vert.Position.Y} {vert.Position.Z} ");
                    colList.Append($"{vert.Color.R} {vert.Color.G} {vert.Color.B} {vert.Color.A} ");
                    texList.Append($"{vert.UV.X} {vert.UV.Y} ");
                }

                StringBuilder indices = new StringBuilder();
                foreach (ushort indice in mesh.Indices)
                {
                    indices.Append($"{indice} ");
                }
                
                file.CreateChild("geometry",
                        new XMLAttribute[] { new("id", $"part{id}-mesh"), new("name", $"part{id}") })
                    .CreateChild("mesh")
                    .CreateChild("source", new XMLAttribute[] { new("id", $"part{id}-mesh-positions") })
                    .CreateSibling("float_array",
                        new XMLAttribute[]
                        {
                            new("id", $"part{id}-mesh-positions-array"), new("count", (mesh.VertexCount * 3).ToString())
                        }, posList.ToString())
                    .CreateChild("technique_common")
                    .CreateChild("accessor", new XMLAttribute[] {new ("source", $"#part{id}-mesh-positions-array"), new ("count", mesh.VertexCount.ToString()), new ("stride", "3")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "X"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "Y"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "Z"), new ("type", "float")})
                    .Close(3)
                    .CreateChild("source", new XMLAttribute[] { new ("id", $"part{id}-mesh-colors") })
                    .CreateSibling("float_array", new XMLAttribute[] {new ("id", $"part{id}-mesh-colors-array"), new ("count", (mesh.VertexCount * 4).ToString())}, colList.ToString())
                    .CreateChild("technique_common")
                    .CreateChild("accessor", new XMLAttribute[] {new ("source", $"#part{id}-mesh-colors-array"), new ("count", mesh.VertexCount.ToString()), new ("stride", "4")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "R"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "G"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "B"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "A"), new ("type", "float")})
                    .Close(3)
                    .CreateChild("source", new XMLAttribute[] { new ("id", $"part{id}-mesh-coords") })
                    .CreateSibling("float_array", new XMLAttribute[] {new ("id", $"part{id}-mesh-coords-array"), new ("count", (mesh.VertexCount * 2).ToString())}, texList.ToString())
                    .CreateChild("technique_common")
                    .CreateChild("accessor", new XMLAttribute[] {new ("source", $"#part{id}-mesh-coords-array"), new ("count", mesh.VertexCount.ToString()), new ("stride", "2")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "U"), new ("type", "float")})
                    .CreateSibling("param", new XMLAttribute[] {new ("name", "V"), new ("type", "float")})
                    .Close(3)
                    .CreateChild("vertices", new XMLAttribute[] {new ("id", $"part{id}-mesh-vertices")})
                    .CreateSibling("input", new XMLAttribute[] { new ("semantic", "POSITION"), new ("source", $"#part{id}-mesh-positions")})
                    .Close()
                    .CreateChild("triangles", new XMLAttribute[] {new ("material", $"mat{materials[mat]}-material"), new ("count", (mesh.IndicesCount / 3).ToString())})
                    .CreateSibling("input", new XMLAttribute[] { new ("semantic", "VERTEX"), new ("source", $"#part{id}-mesh-vertices"), new ("offset", "0")})
                    .CreateSibling("input", new XMLAttribute[] { new ("semantic", "COLOR"), new ("source", $"#part{id}-mesh-colors"), new ("offset", "0")})
                    .CreateSibling("input", new XMLAttribute[] { new ("semantic", "TEXCOORD"), new ("source", $"#part{id}-mesh-coords"), new ("offset", "0")})
                    //.CreateSibling("input", new XMLAttribute[] { new ("semantic", "NORMAL"), new ("source", $"part{id}-mesh-normals"), new ("offset", "1")})
                    .CreateSibling("p", null, indices.ToString())
                    .Close()
                    .Close(2);
            }

            file.Close();

            file.CreateChild("library_visual_scenes")
                .CreateChild("visual_scene", new XMLAttribute[] { new("id", "Scene"), new("name", "Scene") });

            for (int i = 0; i < entities.Length; i++)
            {
                int matId = materials[entities[i].Material];
                int id = i + 1;
                file.CreateChild("node",
                        new XMLAttribute[] { new("id", $"part{id}"), new("name", $"part{id}"), new("type", "NODE") })
                    .CreateSibling("matrix", new XMLAttribute[] { new("sid", "transform") },
                        entities[i].Transformation.ToCleanString())
                    .CreateChild("instance_geometry",
                        new XMLAttribute[] { new("url", $"#part{id}-mesh"), new("name", $"part{id}") })
                    .CreateChild("bind_material")
                    .CreateChild("technique_common")
                    .CreateSibling("instance_material",
                        new XMLAttribute[]
                            { new("symbol", $"mat{matId}-material"), new("target", $"#mat{matId}-material") })
                    .Close(4);
            }

            file.Close();
            
            file.CreateChild("scene")
                .CreateSibling("instance_visual_scene", new XMLAttribute[] { new("url", "#Scene") })
                .Close();
            
            file.Close();
        }
    }

    private static string GenerateCleanName(string textureName)
    {
        if (textureName.Length == 0) return "UnnamedTexture";
        int start = 0;
        int end = 0;
        for (int i = 0; i < textureName.Length; i++)
        {
            if (textureName[i] == '/') start = i + 1;
            if (textureName[i] == '.') end = i;
        }

        return textureName.Substring(start, end - start);
    }
}