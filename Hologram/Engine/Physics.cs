using OpenTK.Mathematics;
using Hologram.Rendering;
using Hologram.Objects;

namespace Hologram.Engine
{
    public static class Physics
    {
        // I did something wrong here. Not sure what though...
        //public static int Raycast2(Camera camera, Vector3 direction, Mesh mesh)
        //{
        //    int lastHit = -1;
        //    float hitDist = 9999;

        //    for (int i = 0; i < mesh.FaceCount; i++)
        //    {
        //        Face face = mesh.Faces[i];
        //        //if (Math.Abs(Vector3.Dot(face.normal, direction)) <= 0.01f) continue; // Discard if plane is close to parallel to line

        //        Vector3 vert1 = mesh.Vertices[face.vert1];
        //        Vector3 vert2 = mesh.Vertices[face.vert2];
        //        Vector3 vert3 = mesh.Vertices[face.vert3];

        //        float D = Vector3.Dot(face.normal, vert1);

        //        float t = -(Vector3.Dot(face.normal, camera.Position) + D) / Vector3.Dot(face.normal, direction);

        //        Vector3 hit = camera.Position + (t * direction);

        //        Vector3 edge0 = vert2 - vert1;
        //        Vector3 edge1 = vert3 - vert2;
        //        Vector3 edge2 = vert1 - vert3;

        //        Vector3 C0 = hit - vert1;
        //        Vector3 C1 = hit - vert2;
        //        Vector3 C2 = hit - vert3;
        //        float val1 = Vector3.Dot(face.normal, Vector3.Cross(edge0, C0));
        //        float val2 = Vector3.Dot(face.normal, Vector3.Cross(edge1, C1));
        //        float val3 = Vector3.Dot(face.normal, Vector3.Cross(edge2, C2));
        //        if (i == 3)
        //        {
        //            if (val1 <= 0)
        //            {
        //                Console.WriteLine("edge0 failed");
        //            }
        //            if (val2 <= 0)
        //            {
        //                Console.WriteLine("edge1 failed");
        //            }
        //            if (val3 <= 0)
        //            {
        //                Console.WriteLine("edge2 failed");
        //            }
        //        }

        //        if (val1 > 0 &&
        //            val2 > 0 &&
        //            val3 > 0)
        //        {
        //            return i;
        //        }
        //    }

        //    return lastHit;
        //}

        public static int Raycast(Camera camera, Vector3 direction, Mesh mesh)
        {
            int lastHit = -1;
            float hitDist = 99999f;
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];

                Vector3 vert1 = mesh.Vertices[face.vert1];
                Vector3 vert2 = mesh.Vertices[face.vert2];
                Vector3 vert3 = mesh.Vertices[face.vert3];

                Vector3 edge1 = vert2 - vert1;
                Vector3 edge2 = vert3 - vert1;

                Vector3 p = Vector3.Cross(direction, edge2);

                float det = Vector3.Dot(edge1, p);

                if (det < 0.0001 && det > -0.0001) continue;

                float invDet = 1f / det;

                Vector3 t = camera.Position - vert1;

                float u = Vector3.Dot(t, p) * invDet;

                if (u < 0 || u > 1) continue;

                Vector3 q = Vector3.Cross(t, edge1);

                float v = Vector3.Dot(direction, q) * invDet;

                if (v < 0 || u + v > 1) continue;

                float dot = Vector3.Dot(edge2, q);
                if (dot > 0.0001)
                {
                    float dist = invDet * dot;
                    if (dist < hitDist)
                    {
                        lastHit = i;
                        hitDist = dist;
                    }
                }
            }

            return lastHit;
        }
    }
}
