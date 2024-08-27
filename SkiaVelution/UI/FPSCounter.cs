using Silk.NET.Maths;
using SkiaSharp;
using SkiaVelution.Objects;

namespace SkiaVelution.UI;

public class FPSCounter : VisualEntity
{
    SKFont font = new();

    public FPSCounter(Vector2D<float> position, float scale, float rotation) : base(position, scale, rotation)
    {
        font.Size = 24f;
    }

    public override void Update(double deltaTime)
    {
        
    }

    public override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        skPaint.Color = SKColors.Magenta;
        
        canvas.DrawText(Model.FPS.ToString(), Position.X, Position.Y, font, skPaint);
    }
}