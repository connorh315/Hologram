using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Hologram.Rendering;

namespace Hologram.Engine.UI.Elements;

public class Button : UIElement
{
    public override Shader Shader => UIDefaults.RoundedQuadShader;
    public override Shader HoverShader => UIDefaults.RoundedQuadShader;

    public RenderableString Text;
    public Color4 BackgroundColor = UIDefaults.ButtonBG;
    public Color4 ForegroundColor = UIDefaults.FG;

    public float Radius = 16f;

    public event Action Click;

    public float PaddingY = 0.2f;

    public Button(UIElement parent) : base(parent) { }

    public void SetText(string text)
    {
        Text = new RenderableString(this);
        Text.SetText(text);
        UpdateTextPos();
    }

    private void DrawColor(Color4 col)
    {
        Surface.DrawRoundedRect(ref elementMatrix, col, Radius);
    }

    public override void Draw()
    {
        DrawColor(BackgroundColor);

        //base.Draw();
    }

    public override void DrawForHover(Color4 col)
    {
        DrawColor(col);
    }

    public override void OnMouseEnter(MainWindow window)
    {
        BackgroundColor = new Color4(255, 0, 0, 255);
        window.SetCursor(CursorShape.Hand);
    }

    public override void OnMouseLeave(MainWindow window)
    {
        BackgroundColor = UIDefaults.ButtonBG;
        window.SetCursor(CursorShape.Arrow);
    }

    public override void OnMouseRelease(MainWindow window)
    {
        Click();
    }

    private void UpdateTextPos()
    {
        if (Text == null) return;

        int textHeight = (int)((1 - 2 * PaddingY) * YScale);
        Text.SetHeight(textHeight);
        Text.SetPos(XPos + XScale / 2 - Text.Width / 2f, YPos + YScale / 2 - textHeight / 2f);
    }

    public override void OnResize(Vector2 originalSize)
    {
        UpdateTextPos();
    }

    public override void OnMove(Vector2 originalPos)
    {
        UpdateTextPos();
    }
}
