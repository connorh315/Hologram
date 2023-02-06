using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Hologram.Engine.UI
{
    public class RenderableString
    {
        public RenderableString(string text, Font font, float scale)
        {
            float[] vertices = new float[4 * 4 * text.Length];

            float xCursor = 0;
            float yCursor = 0;

            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);

            ushort[] indices = new ushort[6 * text.Length];
            
            for (int i = 0; i < text.Length; i++)
            {
                FontChar thisChar = font.Chars[asciiBytes[i]];

                float xPos = xCursor + thisChar.XOffset;
                float yPos = yCursor + thisChar.YOffset;
                
                vertices[i] = xPos; // Top Left
                vertices[i + 1] = yPos;
                vertices[i + 2] = thisChar.X;
                vertices[i + 3] = thisChar.Y;
                
                vertices[i + 4] = xPos + thisChar.Width; // Top Right
                vertices[i + 5] = yPos;
                vertices[i + 6] = thisChar.X + thisChar.Width;
                vertices[i + 7] = thisChar.Y;

                vertices[i + 8] = xPos; // Bottom Left
                vertices[i + 9] = yPos + thisChar.Height;
                vertices[i + 10] = thisChar.X;
                vertices[i + 11] = thisChar.Y + thisChar.Height;

                vertices[i + 12] = xPos + thisChar.Width; // Bottom right
                vertices[i + 13] = yPos + thisChar.Height;
                vertices[i + 14] = thisChar.X + thisChar.Width;
                vertices[i + 15] = thisChar.Y + thisChar.Height;

                xCursor += thisChar.XAdvance;

                int idcOffset = (i * 6);
                int vertOffset = (i * 4);
                indices[idcOffset] = (ushort)(vertOffset);
                indices[idcOffset + 1] = (ushort)(vertOffset + 1);
                indices[idcOffset + 2] = (ushort)(vertOffset + 2);
                indices[idcOffset + 3] = (ushort)(vertOffset);
                indices[idcOffset + 4] = (ushort)(vertOffset + 2);
                indices[idcOffset + 5] = (ushort)(vertOffset + 3);
            }
            
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            int vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 16, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 16, 8);

            int indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, );
        }
    }
}
