using SkiaTemplate.Settings;
using SkiaTemplate.Xml;

namespace SkiaTemplate.Definitions;

public class SettingsDefinitions
{
    public static ProgramSettings ProgramSettings { get; private set; } = new();
    public XmlManager _xmlManager = new();
}