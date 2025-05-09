using Silk.NET.Maths;
using SkiaSharp;
using SkiaVelution.Entities;
using SkiaVelution.Entities.UI;
using SkiaVelution.Lib;

namespace SkiaVelution.Definitions;

public class EntityDefinitions
{
    public EntityDefinitions()
    {
        Time time = new(new Transform());
        FPSCounter fpsCounter = new(new Transform(new Vector2D<float>(15f, 25f), 18f), SKColors.Chartreuse);
        MessageHandler messageHandler = new(new Transform(new Vector2D<float>(15f, WindowManager.HEIGHT - 15f), 16f));
    }
}