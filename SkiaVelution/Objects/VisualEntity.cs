using Silk.NET.Maths;
using SkiaSharp;

namespace SkiaVelution.Objects;

public abstract class VisualEntity : Entity
{
    public float Scale;
    public float Rotation;
    
    protected VisualEntity(Vector2D<float> position, float scale, float rotation)
    {
        Position = position;
        Scale = scale;
        rotation = rotation;
    }

    public abstract void Draw(SKCanvas canvas, SKPaint skPaint);
}