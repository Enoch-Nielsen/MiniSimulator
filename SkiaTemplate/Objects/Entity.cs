using Silk.NET.Maths;
using SkiaTemplate.Lib;

namespace SkiaTemplate.Objects;

public abstract class Entity
{
    public Transform Transform { get; private set; }

    public Entity(Transform transform)
    {
        Transform = transform;
        WindowManager.OnUpdate += Update;
    }

    public abstract void Update(double deltaTime);
    
    public static float Dist(Entity a, Entity b)
    {
        float x = b.Transform.Position.X - a.Transform.Position.X;
        float y = b.Transform.Position.Y - a.Transform.Position.Y;

        return MathF.Sqrt((x * x) + (y * y));
    }
}