using Silk.NET.Maths;
using SkiaSharp;
using SkiaTemplate.Definitions;
using SkiaTemplate.Entities.UI;
using SkiaVelution.Entities.Boids;

namespace SkiaTemplate;

public class Model
{
    private EntityDefinitions entityDefinitions = new();
    private SettingsDefinitions settingsDefinitions = new();

    private FinchManager finchManager = new();
}