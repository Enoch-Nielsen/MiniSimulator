using SkiaSharp;
using SkiaTemplate.Lib;

namespace SkiaTemplate.Objects;

public abstract class VisualEntity : Entity
{
    protected VisualEntity(Transform transform) : base(transform) => WindowManager.OnDraw += Draw;

    protected abstract void Draw(SKCanvas canvas, SKPaint skPaint);
}