using SkiaSharp;
using SkiaVelution.Lib;

namespace SkiaVelution.Objects;

public abstract class VisualEntity : Entity
{
    protected VisualEntity(Transform transform) : base(transform) => WindowManager.OnDraw += Draw;

    protected abstract void Draw(SKCanvas canvas, SKPaint skPaint);
}