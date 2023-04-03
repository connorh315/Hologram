using Hologram.Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Objects.Entities;

public abstract class EngineEntity : Entity
{
    public EngineEntity(Matrix4 transformation) : base(transformation)
    {

    }

    public abstract void OnMousePressed(HologramMouse mouseState);

    public abstract void OnMouseDown(HologramMouse mouseState);

    public abstract void OnMouseReleased(HologramMouse mouseState);

    public abstract void Update();
}
