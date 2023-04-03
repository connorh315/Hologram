using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Engine.UI.Elements;

public class ToolbarMenu : UIElement
{
    public override Shader Shader => UIDefaults.QuadShader;
    public override Shader HoverShader => UIDefaults.QuadShader;

    public ToolbarMenu(UIElement parent) : base(parent) 
    {
        Manager.RemoveElement(this);
        Overlay.AddElement(this);

        Enabled = false;

        boxWidth = minWidth;
    }

    public Color4 BackgroundColor = new Color4(30, 30, 30, 255);


    public RenderableString Title;

    public List<(RenderableString Text, Action Callback)> Options = new();

    private ushort boxHeight = 30;

    private ushort boxWidth = 0;

    private int currentY = 0;

    private ushort minWidth = 180;

    public void SetTitle(string text)
    {
        Title = new RenderableString(Parent);
        Title.SetText(text);
    }

    public void AddOption(string title, Action callback)
    {
        RenderableString text = new RenderableString(this);
        text.SetText(title);
        text.SetHeight(14);

        Options.Add((text, callback));
        PushOption(text);

        boxWidth = Math.Max((ushort)(text.Width + 32), boxWidth);

        currentY -= boxHeight;

        SetSize(boxWidth, Math.Abs(currentY));
        SetPos(XPos, currentY + boxHeight);
    }

    private void PushOption(RenderableString text)
    {
        text.SetPos(XPos + 16, YPos + currentY - boxHeight + ((boxHeight - text.Height) / 2));
    }

    private void DrawColor(Color4 col)
    {
        Surface.DrawRect(ref elementMatrix, col);
    }

    private bool drawHover = false;

    public override void Draw()
    {
        DrawColor(BackgroundColor);

        foreach ((RenderableString text, _) in Options)
        {
            Surface.DrawText(text);
        }
    }

    public override void DrawForHover(Color4 col)
    {
        DrawColor(col);
    }

    public override void OnMove(Vector2 originalPos)
    {
        currentY = 0;
        boxWidth = minWidth;

        foreach ((RenderableString text, Action callback) in Options)
        {
            PushOption(text);

            boxWidth = Math.Max((ushort)(text.Width + 32), boxWidth);

            currentY -= boxHeight;
        }

        SetSize(boxWidth, Math.Abs(currentY));
        YPos += currentY;
    }

    public override void OnMouseOver(MainWindow window)
    {
        int box = (int)((window.CorrectedFlippedMouse.Y - YPos) / boxHeight);
        drawHover = true;
    }
}
