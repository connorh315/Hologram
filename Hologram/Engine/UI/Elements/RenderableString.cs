﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hologram.Rendering;
using Hologram.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Hologram.Engine.UI.Elements
{
    public class RenderableString : UIElement
    {
        public override Shader Shader => UIDefaults.TextShader;

        public Color4 Color = Color4.White;

        private int vertexArray;
        private int indicesBuffer;
        private ushort indicesCount;

        private Font font = UIDefaults.Poppins;

        private ushort width;
        public ushort Width => (ushort)(width * XScale);

        public RenderableString(UIElement parent) : base(parent) { }

        public void SetFont(Font font)
        {
            this.font = font;
        }

        public void SetText(string text)
        {
            float[] vertices = new float[4 * 4 * text.Length];

            float xCursor = 0;
            float yCursor = font.BaseNum;

            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);

            ushort[] indices = new ushort[6 * text.Length];

            float textureWidth = font.ScaleWidth;
            float textureHeight = font.ScaleHeight;

            for (int i = 0; i < text.Length; i++)
            {
                FontChar thisChar = font.Chars[asciiBytes[i]];

                float xPos = xCursor + thisChar.XOffset;
                float yPos = yCursor - thisChar.YOffset; // y offset is how far DOWN the text should go.

                int vertOffset = i * 16;

                vertices[vertOffset] = xPos; // Top Left
                vertices[vertOffset + 1] = yPos;
                vertices[vertOffset + 2] = thisChar.X / textureWidth;
                vertices[vertOffset + 3] = thisChar.Y / textureHeight;

                vertices[vertOffset + 4] = xPos + thisChar.Width; // Top Right
                vertices[vertOffset + 5] = yPos;
                vertices[vertOffset + 6] = (thisChar.X + thisChar.Width) / textureWidth;
                vertices[vertOffset + 7] = thisChar.Y / textureHeight;

                vertices[vertOffset + 8] = xPos; // Bottom Left
                vertices[vertOffset + 9] = yPos - thisChar.Height;
                vertices[vertOffset + 10] = thisChar.X / textureWidth;
                vertices[vertOffset + 11] = (thisChar.Y + thisChar.Height) / textureHeight;

                vertices[vertOffset + 12] = xPos + thisChar.Width; // Bottom right
                vertices[vertOffset + 13] = yPos - thisChar.Height;
                vertices[vertOffset + 14] = (thisChar.X + thisChar.Width) / textureWidth;
                vertices[vertOffset + 15] = (thisChar.Y + thisChar.Height) / textureHeight;

                xCursor += thisChar.XAdvance - 16;

                int idcOffset = i * 6;
                int vertIdcOffset = i * 4;
                indices[idcOffset] = (ushort)vertIdcOffset;
                indices[idcOffset + 1] = (ushort)(vertIdcOffset + 1);
                indices[idcOffset + 2] = (ushort)(vertIdcOffset + 2);
                indices[idcOffset + 3] = (ushort)(vertIdcOffset + 2);
                indices[idcOffset + 4] = (ushort)(vertIdcOffset + 1);
                indices[idcOffset + 5] = (ushort)(vertIdcOffset + 3);
            }

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(vertexArray);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 16, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 16, 8);

            indicesBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 2 * indices.Length, indices, BufferUsageHint.StaticDraw);

            indicesCount = (ushort)indices.Length;
            width = (ushort)xCursor;
        }

        public float Height => YScale * font.Height;

        public void SetHeight(int height)
        {
            XScale = YScale = height / (float)font.Height;
        }

        public override void Draw()
        {
            ShaderManager.Use(UIDefaults.TextShader);

            GL.UniformMatrix4(GL.GetUniformLocation(UIDefaults.TextShader, "model"), false, ref elementMatrix);
            GL.Uniform4(UIDefaults.TextShader.GetUniformLocation("textColor"), Color);
            GL.BindVertexArray(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedShort, 0);
        }

        public override void DrawForHover(Color4 col)
        {
            throw new NotImplementedException();
        }
    }
}