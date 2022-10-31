using Hologram.Objects;
using ModLib;
using OpenTK.Mathematics;
using System;
using Half = System.Half;
using System.Collections.Generic;

namespace Hologram.FileTypes.GSC.GSCWriter.MESH.DXTV
{
    public abstract class DXTV
    {
        protected abstract int Version { get; }

        protected ModFile file;

        protected uint vertexCount;
        protected VertexDefinition[] definitions;

        /// <summary>
        /// Whether vertex data should be written as big endian.
        /// </summary>
        public bool BigEndian = false;

        public abstract void Read(ModFile file, uint vertexCount);

        public abstract void Write(bool shouldClear, List<PartData> parts);

        /// <summary>
        /// "Dictionary" of the storage size each storage type that GSCs have to offer.
        /// </summary>
        protected static byte[] StorageSize = new byte[] { 0, 0, 8, 12, 16, 4, 8, 4, 4, 4 }; // Check the vec4char one

        /// <summary>
        /// Reads the mesh and returns the best currently offered by Hologram.
        /// </summary>
        /// <remarks>Fix meshes to offer a uniform Vertex struct/class</remarks>
        /// <typeparam name="T">The type the value should be returned in.</typeparam>
        /// <param name="mesh">The active mesh</param>
        /// <param name="variable">The requested variable for the vertex</param>
        /// <param name="vertexId">The vertex number</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected static T GetValue<T>(MeshX mesh, VertexDefinition.VariableEnum variable, int vertexId)
        {
            Type returnType = typeof(T);
            object value;
            switch (variable)
            {
                case VertexDefinition.VariableEnum.position:
                    value = mesh.Vertices[vertexId].Position;
                    break;
                case VertexDefinition.VariableEnum.normal:
                    value = mesh.Vertices[vertexId].Normal;
                    break;
                case VertexDefinition.VariableEnum.colorSet0:
                    //value = mesh.Vertices[vertexId].Color; One day
                    value = mesh.Color;
                    break;
                case VertexDefinition.VariableEnum.tangent:
                    value = new Vector4(-1, -1, -1, -1);
                    break;
                case VertexDefinition.VariableEnum.uvSet01:
                    value = mesh.Vertices[vertexId].UV;
                    break;
                case VertexDefinition.VariableEnum.uvSet2:
                    value = Vector2.Zero;
                    break;
                default:
                    throw new Exception("Original GSC uses unknown variable type");

            }

            Type valueType = value.GetType();
            if (valueType == returnType) return (T)value;
            if (valueType == typeof(Vector2) && returnType == typeof(Vector3)) return (T)(object)new Vector3((Vector2)value);
            if (valueType == typeof(Vector2) && returnType == typeof(Vector4)) return (T)(object)new Vector4(((Vector2)value).X, ((Vector2)value).Y, ((Vector2)value).X, ((Vector2)value).Y);
            if (valueType == typeof(Vector3) && returnType == typeof(Vector4)) return (T)(object)new Vector4((Vector3)value, 1);

            throw new Exception($"Reached end of GetValue and could not identify a suitable cast from {valueType} to {returnType} for a {variable}");
        }

        protected byte GetMinified(float toMinify)
        {
            return (byte)Math.Clamp((toMinify + 1) * 128, 0, 255);
        }

        protected void vec2float(Vector2 data)
        {
            file.WriteFloat(data.X, BigEndian); // I think it's little endian
            file.WriteFloat(data.Y, BigEndian);
        }

        protected void vec3float(Vector3 data)
        {
            file.WriteFloat(data.X, BigEndian);
            file.WriteFloat(data.Y, BigEndian);
            file.WriteFloat(data.Z, BigEndian);
        }

        protected void vec4float(Vector4 data)
        {
            file.WriteFloat(data.X, BigEndian);
            file.WriteFloat(data.Y, BigEndian);
            file.WriteFloat(data.Z, BigEndian);
            file.WriteFloat(data.W, BigEndian);
        }

        protected void vec2half(Vector2 data)
        {
            file.WriteHalf((Half)data.X, BigEndian);
            file.WriteHalf((Half)data.Y, BigEndian);
        }

        protected void vec4half(Vector4 data)
        {
            file.WriteHalf((Half)data.X, BigEndian);
            file.WriteHalf((Half)data.Y, BigEndian);
            file.WriteHalf((Half)data.Z, BigEndian);
            file.WriteHalf((Half)data.W, BigEndian);
        }

        protected void vec4char(Vector4 data)
        {
            file.WriteUint(0xffffffff); // Couldn't find out what vec4char actually means
        }

        protected void vec4mini(Vector4 data)
        {
            file.WriteByte(GetMinified(data.X));
            file.WriteByte(GetMinified(data.Y));
            file.WriteByte(GetMinified(data.Z));
            file.WriteByte(GetMinified(data.W));
        }

        protected void color4char(Color4 data)
        {
            file.WriteByte((byte)(data.B * 255));
            file.WriteByte((byte)(data.G * 255));
            file.WriteByte((byte)(data.R * 255));
            file.WriteByte((byte)(data.A * 255));
        }
    }
}
