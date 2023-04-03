using Hologram.Rendering;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Objects.Entities;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using Hologram.Extensions;
using Hologram.Rendering.Shaders;
using OpenTK.Input;

namespace Hologram.Engine;

public class SceneManager : Manager
{
    public Camera Camera = new Camera(new Vector3(30, 30, 30), Vector3.Zero, new Vector2i(0, 0));
    private Shader primaryShader = new Shader(Textured.VertexCode, Textured.FragmentCode);

    public List<Entity> Entities = new List<Entity>();
    public List<Entity> EngineEntities = new List<Entity>();

    public SceneManager(MainWindow parent, int width, int height) : base(parent, width, height)
    {

    }

    protected override void RebuildMatrix()
    {
        Camera.ResizeViewport(new Vector2i(Width, Height));

        Matrix4 projection = Camera.ProjectionMatrix;

        ShaderManager.Use(primaryShader);
        GL.UniformMatrix4(primaryShader.GetUniformLocation("projection"), false, ref projection);
    }

    const float cameraHSpeed = 24f;
    const float cameraVSpeed = 24f;

    public override void Update(double deltaTime)
    {
        if (!HasHover()) return;

        var input = Parent.KeyboardState;

        if (input.IsKeyDown(Keys.Space))
        {
            Camera.Translate(Vector3.UnitY * cameraVSpeed * (float)deltaTime);
        }
        else if (input.IsKeyDown(Keys.LeftControl))
        {
            //camera.Translate(Vector3.UnitY * -cameraVSpeed * (float)args.Time);
        }

        float adjustedSpeed = cameraHSpeed * (float)deltaTime;

        if (input.IsKeyDown(Keys.LeftShift))
        {
            adjustedSpeed *= 2;
        }

        if (input.IsKeyDown(Keys.W))
        {
            Camera.Translate(adjustedSpeed * Camera.Forward);
        }
        if (input.IsKeyDown(Keys.S))
        {
            Camera.Translate(-adjustedSpeed * Camera.Forward);
        }
        if (input.IsKeyDown(Keys.A))
        {
            Camera.Translate(-adjustedSpeed * Camera.Right);
        }
        if (input.IsKeyDown(Keys.D))
        {
            Camera.Translate(adjustedSpeed * Camera.Right);
        }

        if (Parent.MouseState.IsButtonDown(MouseButton.Right))
        {
            if (!Parent.LockCursor(Parent.ScaleFBToPixels(Center)))
            {
                float deltaX = Parent.MouseState.X - Parent.LockPos.X;
                float deltaY = Parent.MouseState.Y - Parent.LockPos.Y;

                if (deltaX != 0 || deltaY != 0)
                {
                    Camera.RotateCamera(deltaX.Deg2Rad() * 0.1f, deltaY.Deg2Rad() * 0.1f);
                }
            }

        }
        else
        {
            Parent.UnlockCursor();
        }

        if (Camera.CalculateViewMatrix())
        {
            Matrix4 viewMat = Camera.ViewMatrix;

            ShaderManager.Use(primaryShader);
            GL.UniformMatrix4(primaryShader.GetUniformLocation("view"), false, ref viewMat);
        }
    }

    public override void Draw()
    {
        ShaderManager.Use(primaryShader);

        GL.Uniform3(primaryShader.GetUniformLocation("cameraDir"), Camera.Forward);
        GL.Uniform3(primaryShader.GetUniformLocation("meshColor"), new Vector3(1, 1, 1));

        int worldLoc = primaryShader.GetUniformLocation("world");
        foreach (Entity entity in Entities)
        {
            if (Vector3.DistanceSquared(Camera.Position, entity.Bounds.Center) <= entity.Bounds.DistSqrd)
            {
                entity.Draw(worldLoc);
            }
        }

        GL.Clear(ClearBufferMask.DepthBufferBit);

        foreach (Entity engineEntity in EngineEntities)
        {
            engineEntity.Draw(worldLoc);
        }
    }

    public override void OnMouseDown(HologramMouse mouse)
    {
        throw new NotImplementedException();
    }

    public override void OnMouseEnter(Vector2 mouse)
    {
        
    }

    public override void OnMouseLeave(Vector2 mouse)
    {
        
    }

    public override bool OnMouseOver(Vector2 mouse)
    {
        return true;
    }

    public override void OnMousePress(HologramMouse mouse)
    {
        throw new NotImplementedException();
    }

    public override void OnMouseRelease(HologramMouse mouse)
    {
        throw new NotImplementedException();
    }
}
