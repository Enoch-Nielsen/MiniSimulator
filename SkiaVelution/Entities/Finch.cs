using Silk.NET.Maths;
using SkiaSharp;
using SkiaVelution.Boids;
using SkiaVelution.Objects;

namespace SkiaVelution.Entities;

public class Finch : Entity
{
    public FinchBoid Boid { get; private set; }
    private FinchBehaviour _finchBehaviour;

    public Finch(int group,int id, Vector2D<float> position, float scale, SKColor color)
    {
        _finchBehaviour = Model.FinchBehaviour;
        Boid = new FinchBoid(group, id, position, scale, 0f, color);
    }

    public override void Update(double deltaTime)
    {
        Boid.Update(deltaTime);
    }
}