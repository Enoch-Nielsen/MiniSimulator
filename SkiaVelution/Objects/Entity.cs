using Silk.NET.Maths;
using SkiaVelution.Boids;

namespace SkiaVelution.Objects;

public abstract class Entity
{
    public Vector2D<float> Position;

    public abstract void Update(double deltaTime);
    
    public float Dist(Entity a, Entity b)
    {
        float x = b.Position.X - a.Position.X;
        float y = b.Position.Y - a.Position.Y;

        return MathF.Sqrt((x * x) + (y * y));
    }
}