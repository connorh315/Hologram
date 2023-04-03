using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Hologram.Rendering;
using Hologram.Extensions;

namespace Hologram.Objects.Entities;

public class Entity : BaseEntity
{
    public Entity(Matrix4 transformation) : base(transformation)
    {

    }

    public List<Entity> Children = new List<Entity>();

    public Entity? Parent;

    public void SetParent(Entity? parent)
    {
        Parent?.Children.Remove(this);

        Parent = parent;
        Parent?.Children.Add(this);
    }

    public void AddChild(Entity child)
    {
        Children.Add(child);
        child.Parent?.Children.Remove(child);
        child.Parent = this;
    }

    /// <summary>
    /// Translate this entity and all children
    /// </summary>
    /// <param name="translation">The amount to translate by</param>
    public void Translate(Vector3 translation)
    {
        Position += translation;
        foreach (Entity child in Children)
        {
            child.Translate(translation);
        }
    }
}
