using SkiaVelution.Settings;
using SkiaVelution.Xml;

namespace SkiaVelution.Definitions;

public class SettingsDefinitions
{
    public XmlManager _xmlManager = new();
    public static ProgramSettings ProgramSettings { get; private set; } = new();
    public static BoidSettings BoidSettings { get; private set; } = new();
}