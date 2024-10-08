using Silk.NET.Maths;

namespace SkiaTemplate.Lib;

public class Transform
{
    public Vector2D<float> Position;
    public float Rotation;
    public float Scale;

    public Transform(Vector2D<float> position = default, float scale = 1f, float rotation = 0f)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
    }
}