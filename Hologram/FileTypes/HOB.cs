using Hologram.Objects;
using Hologram.Rendering;
using ModLib;
using OpenTK.Mathematics;
using Half = System.Half;

namespace Hologram.FileTypes;

public class HOB
{
    public static void Write(string fileLocation, Entity entity)
    {
        MeshX mesh = entity.Mesh;
        using (ModFile file = ModFile.Create(fileLocation))
        {
            file.WritePascalString(entity.Name);
            file.WriteInt(mesh.VertexCount, true);
            foreach (Vertex vert in mesh.Vertices)
            {
                file.WriteFloat(vert.Position.X, true);
                file.WriteFloat(vert.Position.Y, true);
                file.WriteFloat(vert.Position.Z, true);
                file.WriteHalf((Half)vert.Normal.X, true);
                file.WriteHalf((Half)vert.Normal.Y, true);
                file.WriteHalf((Half)vert.Normal.Z, true);
                file.WriteHalf((Half)vert.UV.X, true);
                file.WriteHalf((Half)vert.UV.Y, true);
                file.WriteByte((byte)(vert.Color.R * 255));
                file.WriteByte((byte)(vert.Color.G * 255));
                file.WriteByte((byte)(vert.Color.B * 255));
            }
            file.WriteInt(mesh.IndicesCount, true);
            foreach (ushort indice in mesh.Indices)
            {
                file.WriteUshort(indice, true);
            }
        }
    }

    public static Entity Parse(ModFile file)
    {
        string name = file.ReadPascalString(true);
        int verticesCount = file.ReadInt(true);
        MeshX mesh = new MeshX();
        mesh.Vertices = new Vertex[verticesCount];
        for (int i = 0; i < verticesCount; i++)
        {
            mesh.Vertices[i] = new Vertex(
                new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true)),
                new Vector3((float)file.ReadHalf(true), (float)file.ReadHalf(true), (float)file.ReadHalf(true)),
                new Vector2((float)file.ReadHalf(true), (float)file.ReadHalf(true)),
                new Color4(file.ReadByte(), file.ReadByte(), file.ReadByte(), 255)
            );
        }

        int indicesCount = file.ReadInt(true);
        mesh.Indices = new ushort[indicesCount];
        for (int i = 0; i < indicesCount; i++)
        {
            mesh.Indices[i] = file.ReadUshort(true);
        }

        Entity ent = new Entity(Matrix4.Identity)
        {
            Bounds = new CameraBounds()
            {
                Center = Vector3.Zero,
                DistSqrd = 1000000
            },
            Material = new Material()
            {
                Color = Color4.White,
                Diffuse = Texture.WhiteTexture,
                Normal = Texture.WhiteTexture,
                ShaderName = "HOB"
            },
            Mesh = mesh,
            Name = name,
        };

        ent.Mesh.Setup();
        
        return ent;
    }
}