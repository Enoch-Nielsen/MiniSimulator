using Silk.NET.Maths;
using SkiaSharp;
using SkiaTemplate.Entities;
using SkiaTemplate.Entities.UI;
using SkiaTemplate.Lib;

namespace SkiaTemplate.Definitions;

public class EntityDefinitions
{
    public EntityDefinitions()
    {
        Time time = new(new Transform());
        FPSCounter fpsCounter = new(new Transform(new Vector2D<float>(15f, 25f), 18f), SKColors.Chartreuse);
        MessageHandler messageHandler = new(new Transform(new Vector2D<float>(15f, WindowManager.HEIGHT - 25f), 16f));
    }
}