﻿using OpenTK.Mathematics;
using Hologram.Rendering;
using Hologram.Objects;
using Hologram.Rendering.Shaders;
using ModLib;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Objects.Entities;

namespace Hologram.Engine;

public static class Physics
{
    private static Shader colorPicker = new Shader(Colored.VertexCode, Colored.FragmentCode);
    
    public static Entity? Pick(Entity[] entities, Entity[] engineEntities, Camera camera, Vector2 point)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(colorPicker);
        
        int projectionLoc = GL.GetUniformLocation(colorPicker, "projection");
        Matrix4 projectionMat = camera.ProjectionMatrix;
        GL.UniformMatrix4(projectionLoc, false, ref projectionMat);

        int viewLoc = GL.GetUniformLocation(colorPicker, "view");
        Matrix4 viewMat = camera.ViewMatrix;
        GL.UniformMatrix4(viewLoc, false, ref viewMat);

        int color = GL.GetUniformLocation(colorPicker, "PickingColor");
        int worldLoc = GL.GetUniformLocation(colorPicker, "world");
        for (int i = 0; i < entities.Length + engineEntities.Length; i++)
        {
            Entity ent = (i < entities.Length) ? entities[i] : engineEntities[i - entities.Length];
            if (i == entities.Length) GL.Clear(ClearBufferMask.DepthBufferBit);
            int id = i + 1;
            MeshX mesh = ent.Mesh;
            Matrix4 transformation = ent.Transformation;
            GL.UniformMatrix4(worldLoc, false, ref transformation);
            GL.Uniform3(color, new Vector3((((id) & 0xff0000) >> 16) / 255f, (((id) & 0xff00) >> 8) / 255f, ((id) & 0xff) / 255f));
            mesh.Draw();
        }
        GL.UseProgram(0);
        GL.Flush();
        GL.Finish();

        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        
        byte[] pixel = new byte[4];
        GL.ReadPixels((int)Math.Floor(point.X), (int)Math.Floor(point.Y), 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);
        
        if ((pixel[2] == 0) && (pixel[1] == 0) && (pixel[0] == 0)) return null;

        int entityId = ((pixel[0] * 65536) + (pixel[1] * 256) + pixel[2]) - 1;

        return (entityId < entities.Length) ? entities[entityId] : engineEntities[entityId - entities.Length];
    }
}
