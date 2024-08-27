using SkiaTemplate.Lib;
using SkiaTemplate.Objects;

namespace SkiaTemplate.Entities;

public class Time : Entity
{
    public static double Runtime { get; private set; }
    
    public Time(Transform transform) : base(transform) { }
    
    public override void Update(double deltaTime)
    {
        Runtime += deltaTime;
    }
}