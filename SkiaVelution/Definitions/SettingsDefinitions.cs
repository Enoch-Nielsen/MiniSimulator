using SkiaTemplate.Settings;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Definitions;

public class SettingsDefinitions
{
    public XmlManager _xmlManager = new();
    public static ProgramSettings ProgramSettings { get; private set; } = new();
    public static BoidSettings BoidSettings { get; private set; } = new();
}