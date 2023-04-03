using System;
using Hologram.Objects;
using OpenTK.Mathematics;

namespace Hologram.FileTypes.DNO;

public class DMesh
{
    private Mesh physicsMesh;

    public DQuad[] Quads;

    public Vector3[] Vertices;

    public DBucket[] Buckets;

    public DNode[] Nodes;

    public DMaterial[] Materials;

    public Mesh GetPhysicsMesh()
    {
        if (physicsMesh != null) return physicsMesh;

        physicsMesh = new Mesh(Vertices.Length, Quads.Length, FaceType.Quads);

        physicsMesh.Vertices = Vertices;

        for (int i = 0; i < Quads.Length; i++)
        {
            physicsMesh.Faces[i] = Quads[i].Quad;
        }

        return physicsMesh;
    }
}
