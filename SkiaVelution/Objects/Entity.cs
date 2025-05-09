using SkiaTemplate.Lib;

namespace SkiaTemplate.Objects;

public abstract class Entity
{
    public Entity(Transform transform)
    {
        Transform = transform;
        WindowManager.OnUpdate += Update;
    }

    public Transform Transform { get; }

    public abstract void Update(double deltaTime);

    public static float Dist(Entity a, Entity b)
    {
        var x = b.Transform.Position.X - a.Transform.Position.X;
        var y = b.Transform.Position.Y - a.Transform.Position.Y;

        return MathF.Sqrt(x * x + y * y);
    }
}