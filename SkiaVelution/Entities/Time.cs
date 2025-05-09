using SkiaVelution.Lib;
using SkiaVelution.Objects;

namespace SkiaVelution.Entities;

public class Time : Entity
{
    public Time(Transform transform) : base(transform){}

    public static double Runtime { get; private set; }

    public override void Update(double deltaTime)
    {
        Runtime += deltaTime;
    }
}