using Silk.NET.Maths;
using SkiaSharp;
using SkiaTemplate;
using SkiaTemplate.Lib;
using SkiaTemplate.Objects;

namespace SkiaVelution.Entities.Boids;

public class Finch : Entity
{
    public FinchBoid Boid { get; private set; }
    private FinchBehaviour _finchBehaviour;

    public Finch(int group,int id, Transform transform, SKColor color) : base(transform)
    {
        _finchBehaviour = FinchManager.FinchBehaviour;
        Boid = new FinchBoid(group, id, transform, color);
    }

    public override void Update(double deltaTime)
    {
        Boid.Update(deltaTime);
    }
}