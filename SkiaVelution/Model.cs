using SkiaVelution.Definitions;
using SkiaVelution.Entities.Boids;

namespace SkiaVelution;

public class Model
{
    private EntityDefinitions entityDefinitions = new();
    private SettingsDefinitions settingsDefinitions = new();

    private FinchManager finchManager = new();
}